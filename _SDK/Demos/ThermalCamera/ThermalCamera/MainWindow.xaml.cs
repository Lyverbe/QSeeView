using NetSDKCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThermalCamera.ViewModel;

namespace ThermalCamera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ThermalViewModel tvm = new ThermalViewModel();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = tvm;
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                this.playwnd.Refresh();
                this.IsEnabled = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(((TextBox)sender).Text + e.Text);
                e.Handled = false;
            }
            catch
            {
                e.Handled = true;
            } 
        }

        private void PlayChannel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int channel = cmb_channel.SelectedIndex;
            if (tvm.playHandle != IntPtr.Zero && channel != tvm.currentChannel)
            {
                NETClient.StopRealPlay(tvm.playHandle);
                tvm.playHandle = NETClient.RealPlay(ThermalViewModel.lLoginID, channel, tvm.hWnd);
                if (tvm.playHandle != IntPtr.Zero)
                {
                    tvm.currentChannel = channel;
                    tvm.PlayBtnText = "Stop(停止)";
                }
                else
                {
                    MessageBox.Show("Real-Play failed(实时监视失败)!");
                }
            }
        }
    }
}
