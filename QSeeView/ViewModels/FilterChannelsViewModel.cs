﻿using QSeeView.Tools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class FilterChannelsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Close;

        public FilterChannelsViewModel()
        {
            CloseCommand = new RelayCommand(() => Close?.Invoke(this, EventArgs.Empty));
        }

        public ICommand CloseCommand { get; }

        public IEnumerable<ChannelInfoModel> Channels => App.Settings.ChannelsInfo.Where(channelInfo => channelInfo.IsOnline);

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
