using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;
using System;
using Windows.System;
using System.Windows;
using Coding4Fun.Phone.Controls;

namespace HelloWorld
{
    public partial class PivotPageAbout : PhoneApplicationPage
    {
        public PivotPageAbout()
        {
            InitializeComponent();
            storyboard_1.Begin();
        }

        private void version_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //登录商店
            MarketplaceDetailTask task = new MarketplaceDetailTask { };
            task.Show();
        }

        private void email_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string OSVersion = Environment.OSVersion.ToString();   //OS版本
            string manufacturer = DeviceStatus.DeviceManufacturer.ToString();       //硬件厂商
            string name = DeviceStatus.DeviceName.ToString();   //设备型号
            //发送电子邮件
            EmailComposeTask task = new EmailComposeTask
            {
                To = "lython@live.cn",
                Subject = String.Format("[Lython]设置²{0}", Application.Current.Resources["appVersion"]),
                Body = String.Format("\n\n\n生产厂商：{0}\n手机型号：{1}\nOS版本：{2}", manufacturer, name, OSVersion),
            };
            task.Show();
        }

        private void star_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //评价
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private async void update_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //登录商店
            await Launcher.LaunchUriAsync(new System.Uri("zune:search?publisher=Lython"));
        }

        private void shareapp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string content = "我在微软Windows Phone应用商店发现这款 设置² App相当不错，点击 http://www.windowsphone.com/s?appid=11bd0602-9fa9-4903-a6c0-f755887f8139 可以下载哦！";
            //发送短信
            SmsComposeTask task = new SmsComposeTask
            {
                Body = content,
            };
            task.Show();
        }

        private async void download_flashlight_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            await Launcher.LaunchUriAsync(new System.Uri("zune:navigate?appid=34d3f2ba-044f-4f9e-94ba-127bf8dd3cd2"));
        }

        private async void download_stopwatch_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            await Launcher.LaunchUriAsync(new System.Uri("zune:navigate?appid=063f4d60-5aea-4c7f-8cda-ad32ed0e6803"));
        }

        //触摸开始与结束
        private void tile_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            (sender as Tile).Margin = new Thickness(2, 2, 2, 2);
        }

        private void tile_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            (sender as Tile).Margin = new Thickness(0, 0, 0, 0);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            storyboard_2.Begin();
        }
    }
}
