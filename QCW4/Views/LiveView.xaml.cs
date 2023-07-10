using NetSDKCS;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;

namespace QSeeView.Views
{
    public partial class LiveView : Window
    {
        private LiveViewModel _viewModel;

        private IDeviceManager _deviceManager;
        private IDictionary<int, IntPtr> _realPlayIds;

        public LiveView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _realPlayIds = new Dictionary<int, IntPtr>();

            _viewModel = new LiveViewModel(deviceManager.ChannelsCount);
            DataContext = _viewModel;

            ContentRendered += LiveView_ContentRendered;
        }

        protected override void OnClosed(EventArgs e)
        {
            _realPlayIds.ToList().ForEach(realPlayId => _deviceManager.StopLiveView(realPlayId.Value));
            base.OnClosed(e);
        }

        private void LiveView_ContentRendered(object sender, EventArgs e)
        {
            _viewModel.LiveMonitors.ToList().ForEach(monitor => StartChannel(monitor.ChannelId));
        }

        private void StartChannel(int channelId)
        {
            var instanceCount = channelId;
            var pictureBox = FindPictureBox(this, ref instanceCount);
            if (pictureBox != null)
                _realPlayIds[channelId] = _deviceManager.StartLiveView(channelId, pictureBox.Handle);
        }

        public static PictureBox FindPictureBox(DependencyObject dependencyObj, ref int instanceCount)
        {
            if (dependencyObj != null)
            {
                for (var childId = 0; childId < VisualTreeHelper.GetChildrenCount(dependencyObj); childId++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObj, childId);
                    if (child != null && child is WindowsFormsHost && instanceCount-- == 0)
                        return ((WindowsFormsHost)child).Child as PictureBox;

                    var pictureBox = FindPictureBox(child, ref instanceCount);
                    if (pictureBox != null)
                        return pictureBox;
                }
            }
            return null;
        }
    }
}
