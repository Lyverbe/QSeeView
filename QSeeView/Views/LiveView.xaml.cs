using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.Tools.Models;
using QSeeView.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace QSeeView.Views
{
    public partial class LiveView : Window
    {
        private LiveViewModel _viewModel;

        private IDeviceManager _deviceManager;
        private PictureBox _originalZoomedPictureBox;
        private WindowsFormsHost _zoomedHost;
        private System.Drawing.Point _lastMouseLocation;
        private bool _isDragging;
        private Timer _updatePictureBoxSizeTimer;

        public LiveView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;

            _viewModel = new LiveViewModel(deviceManager);
            DataContext = _viewModel;

            _updatePictureBoxSizeTimer = new Timer();
            _updatePictureBoxSizeTimer.Interval = 250;
            _updatePictureBoxSizeTimer.Tick += UpdatePictureBoxSizeTimer_Tick;

            ContentRendered += LiveView_ContentRendered;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_viewModel.LiveMonitors.Any(monitor => monitor.IsRecording))
            {
                System.Windows.MessageBox.Show("Please stop all recordings before closing this window", Title);
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.LiveMonitors.Where(monitor => monitor.IsOnline).ToList().ForEach(monitor => monitor.StopLiveView());
            base.OnClosed(e);
        }

        private void LiveView_ContentRendered(object sender, EventArgs e)
        {
            var liveMonitorId = 0;
            foreach (var liveMonitor in _viewModel.LiveMonitors)
            {
                var instanceCount = liveMonitorId++;
                var control = FindFrameworkElement<WindowsFormsHost>(this, ref instanceCount);
                if (control != null)
                {
                    liveMonitor.Host = control;
                    var pictureBox = control.Child as PictureBox;
                    liveMonitor.DisplayOriginalSize = new Size(pictureBox.Width, pictureBox.Height);
                    if (liveMonitor.IsOnline)
                        liveMonitor.StartLiveView();

                    pictureBox.MouseDown += PictureBox_MouseDown;
                    pictureBox.MouseUp += PictureBox_MouseUp;
                    pictureBox.MouseMove += PictureBox_MouseMove;
                    pictureBox.MouseWheel += PictureBox_MouseWheel;
                }

                liveMonitor.ChannelChanging += LiveMonitor_ChannelChanging;
                liveMonitor.RecordButtonPressed += LiveMonitor_RecordButtonPressed;
            }
        }

        public static T FindFrameworkElement<T>(DependencyObject dependencyObj, ref int instanceCount) where T : FrameworkElement
        {
            if (dependencyObj != null)
            {
                for (var childId = 0; childId < VisualTreeHelper.GetChildrenCount(dependencyObj); childId++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObj, childId);
                    if (child != null && child is T && instanceCount-- == 0)
                        return child as T;

                    var control = FindFrameworkElement<T>(child, ref instanceCount);
                    if (control != null)
                        return control;
                }
            }
            return null;
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.C && System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
                _viewModel.AreControlsVisible = !_viewModel.AreControlsVisible;
            else if (e.Key == System.Windows.Input.Key.Escape)
                Close();

            base.OnKeyDown(e);
        }

        private void LiveMonitor_ChannelChanging(object sender, ChannelInfoModel newChannelInfo)
        {
            if (newChannelInfo.IsOnline)
            {
                var liveMonitor = _viewModel.LiveMonitors.FirstOrDefault(monitor => monitor.SelectedChannel.ChannelId == newChannelInfo.ChannelId);
                if (liveMonitor != null)
                    liveMonitor.SelectedChannel = liveMonitor.Channels.FirstOrDefault(monitor => !monitor.IsOnline);
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _lastMouseLocation = Control.MousePosition;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var liveMonitor = _viewModel.LiveMonitors.FirstOrDefault(monitor => monitor.Host.Child == sender);
                if (liveMonitor != null && liveMonitor.ZoomLevel > 0)
                {
                    var mouseLocationDelta = new Point(Control.MousePosition.X - _lastMouseLocation.X, Control.MousePosition.Y - _lastMouseLocation.Y);
                    liveMonitor.OffsetScroll(mouseLocationDelta);
                }

                _isDragging = true;
            }

            _lastMouseLocation = Control.MousePosition;
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                return;
            }

            if (_viewModel.ViewRowsCount == 1 && _viewModel.ViewColumnsCount == 1)
            {
                // Zoom out
                _viewModel.MaximizeRowsColumnsCount();

                _zoomedHost.Child = _viewModel.LiveMonitors.First().Host.Child;
                _viewModel.LiveMonitors.First().Host.Child = _originalZoomedPictureBox;
            }
            else
            {
                // Zoom in
                _zoomedHost = _viewModel.LiveMonitors.FirstOrDefault(monitor => monitor.Host.Child == sender)?.Host;
                _originalZoomedPictureBox = _viewModel.LiveMonitors.First()?.Host.Child as PictureBox;
                if (_zoomedHost != null && _originalZoomedPictureBox != null)
                {
                    _viewModel.ViewRowsCount = 1;
                    _viewModel.ViewColumnsCount = 1;

                    _viewModel.LiveMonitors.First().Host.Child = sender as PictureBox;
                }
            }

            _viewModel.LiveMonitors.First().ZoomLevel = 0;

            // Picture box size has changed and we need to update its original size in the live monitor model
            // but UI didn't have time to change it yet.  Do it after UI has refreshed the controls.
            // I'd love to use SizeChanged event instead of timer but it's also called when zooming in/out.
            _updatePictureBoxSizeTimer.Start();
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            var liveMonitor = _viewModel.LiveMonitors.FirstOrDefault(monitor => monitor.Host.Child == sender);
            if (liveMonitor.ZoomLevel + e.Delta < 0)
                liveMonitor.ZoomLevel = 0;
            else if (liveMonitor.ZoomLevel + e.Delta > liveMonitor.ZoomLevelMaximum)
                liveMonitor.ZoomLevel = liveMonitor.ZoomLevelMaximum;
            else
                liveMonitor.ZoomLevel += e.Delta;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _updatePictureBoxSizeTimer.Start();
        }

        private void UpdatePictureBoxSizeTimer_Tick(object sender, EventArgs e)
        {
            _updatePictureBoxSizeTimer.Stop();

            var liveMonitor = _viewModel.LiveMonitors.FirstOrDefault(monitor => monitor.Host == _zoomedHost);
            if (liveMonitor?.Host?.Child != null)
            {
                var pictureBox = liveMonitor.Host.Child as PictureBox;
                liveMonitor.DisplayOriginalSize = new Size(pictureBox.Width, pictureBox.Height);
            }
        }

        private void LiveMonitor_RecordButtonPressed(object sender, EventArgs e)
        {
            var liveMonitor = sender as LiveMonitorModel;
            if (liveMonitor.IsRecording)
            {
                _deviceManager.StopSaveRealData(liveMonitor.PlayHandle);
                liveMonitor.IsRecording = false;

                var converter = new VideoConverter();
                try
                {
                    converter.ConversionDone += (s, exitCode) => File.Delete(Path.Combine(App.Settings.DownloadFolder, liveMonitor.LocalRecordFileName + ".dav"));
                    converter.Convert(liveMonitor.LocalRecordFileName);
                }
                catch (Win32Exception exception)
                {
                    System.Windows.MessageBox.Show($"{liveMonitor.LocalRecordFileName}: Conversion process error - {exception.Message}", "Local record",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                liveMonitor.LocalRecordFileName = FileNameBuilder.Build(App.Settings.FileNamesPattern, DateTime.Now, liveMonitor.SelectedChannel.ChannelId);
                var fileName = Path.Combine(App.Settings.DownloadFolder, liveMonitor.LocalRecordFileName + ".dav");
                liveMonitor.IsRecording = _deviceManager.SaveRealData(liveMonitor.PlayHandle, fileName);
            }
        }
    }
}
