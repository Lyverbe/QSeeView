using QSeeView.Models;
using QSeeView.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Close;
        public event EventHandler Clear;

        private IDeviceManager _deviceManager;
        private string _eventDetails;

        public LogsViewModel(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;

            Logs = new ObservableCollection<LogModel>();

            CloseCommand = new RelayCommand(() => Close?.Invoke(this, EventArgs.Empty));
            QueryCommand = new RelayCommand(() => Query(), () => CanQuery());
            ClearCommand = new RelayCommand(() => Clear?.Invoke(this, EventArgs.Empty));

            StartDateTime = DateTime.Now.Date;
            EndDateTime = DateTime.Now.Date.AddDays(1);
        }

        public ICommand CloseCommand { get; }
        public ICommand QueryCommand { get; }
        public ICommand ClearCommand { get; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        
        public LogModel SelectedLog
        {
            set
            {
                EventDetails = value?.Detail;
                OnPropertyChanged(nameof(SelectedLog));
            }
        }

        public string EventDetails
        {
            get => _eventDetails;
            set
            {
                _eventDetails = value;
                OnPropertyChanged(nameof(EventDetails));
            }
        }

        public ObservableCollection<LogModel> Logs { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool CanQuery()
        {
            if (EndDateTime <= StartDateTime)
                return false;

            return true;
        }

        public void Query()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var logs = _deviceManager.GetLogs(StartDateTime, EndDateTime);
            Mouse.OverrideCursor = null;
            if (logs == null)
            {
                MessageBox.Show("Operation failed:\n\n" + _deviceManager.GetLastError(), "Get logs", MessageBoxButton.OK);
                Logs.Clear();
                return;
            }

            Logs = new ObservableCollection<LogModel>(logs);
            OnPropertyChanged(nameof(Logs));
        }
    }
}
