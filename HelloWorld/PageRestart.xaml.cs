using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.System;
using Coding4Fun.Phone.Controls;
using System.Windows.Media;

namespace HelloWorld
{
    public partial class PageRestart : PhoneApplicationPage
    {
        public PageRestart()
        {
            InitializeComponent();
        }

        private void btn_restart_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //await Launcher.LaunchUriAsync(new System.Uri("test:"));
            NavigationService.Navigate(new Uri("/PageBye.xaml", UriKind.Relative));
        }

        private void btn_cancel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.Current.Terminate();
        }

        private void round_btn_Click(object sender, RoutedEventArgs e)
        {
            //await Launcher.LaunchUriAsync(new System.Uri("test:"));
            NavigationService.Navigate(new Uri("/PageBye.xaml", UriKind.Relative));
        }
    }
}