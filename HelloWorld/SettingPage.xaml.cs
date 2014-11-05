using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using System;

namespace HelloWorld
{
    public partial class SettingPage : PhoneApplicationPage
    {
        private DispatcherTimer timer_out = new DispatcherTimer(); //定时器

        public SettingPage( )
        {
            InitializeComponent( );
            storyboard_1.Begin();
            //InitListPacker();
            this.toggleBackground.IsChecked = Config.IsBackground;
            this.toggleTileNotie.IsChecked = Config.IsTileNotie;
            this.toggleTileThird.IsChecked = Config.IsThirdApp;
            this.sliderLimit.Value = Config.OpacityValue;
            this.textBlockOpacity.Text = Config.OpacityValue.ToString();

            timer_out.Interval = TimeSpan.FromMilliseconds(20); //xx毫秒之后再加载
            timer_out.Tick += OnTimerOut;
            timer_out.Start();
        }

        void OnTimerOut(object sender, EventArgs e)
        {
            timer_out.Stop();
            this.listPickerOpen.ItemsSource = Lython.ColorList;
            this.listPickerClose.ItemsSource = Lython.ColorList;
            this.listPickerDefault.ItemsSource = Lython.ColorList;

            this.listPickerOpen.SelectedIndex = Config.openColor;
            this.listPickerClose.SelectedIndex = Config.closeColor;
            this.listPickerDefault.SelectedIndex = Config.defaultColor;

            this.listPickerOpen.SelectionChanged += listPickerOpen_SelectionChanged;
            this.listPickerClose.SelectionChanged += listPickerClose_SelectionChanged;
            this.listPickerDefault.SelectionChanged += listPickerDefault_SelectionChanged;
            this.sliderLimit.ValueChanged += sliderLimit_ValueChanged;
        }
        
        #region 存储设置状态
        private void toggleBackground_Checked(object sender, RoutedEventArgs e)
        {
            Config.IsBackground = (bool)this.toggleBackground.IsChecked;
            this.toggleBackground.Content = ((bool)this.toggleBackground.IsChecked) ? "自定义": "默认";
        }
        #endregion

        private void listPickerOpen_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Config.openColor = (int)this.listPickerOpen.SelectedIndex;
            Config.openColorPath = Lython.ColorList[Config.openColor].ColorPath;
        }

        private void listPickerClose_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Config.closeColor = (int)this.listPickerClose.SelectedIndex;
            Config.closeColorPath = Lython.ColorList[Config.closeColor].ColorPath;
        }

        private void listPickerDefault_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Config.defaultColor = (int)this.listPickerDefault.SelectedIndex;
            Config.defaultColorPath = Lython.ColorList[Config.defaultColor].ColorPath;
        }

        private void toggleTileNotie_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)this.toggleTileNotie.IsChecked)
            {
                Config.IsTileNotie = true;
                this.toggleTileNotie.Content = "开";
            }
            else
            {
                Config.IsTileNotie = false;
                this.toggleTileNotie.Content = "关";
            }
        }

        private void toggleTileThird_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)this.toggleTileThird.IsChecked)
            {
                Config.IsThirdApp = true;
                this.toggleTileThird.Content = "开";
            }
            else
            {
                Config.IsThirdApp = false;
                this.toggleTileThird.Content = "关";
            }
        }

        private void linkButtonReset_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //重置
            listPickerOpen.SelectedIndex = 13;
            listPickerClose.SelectedIndex = 1;
            listPickerDefault.SelectedIndex = 0;
            toggleBackground.IsChecked = false;
            toggleTileNotie.IsChecked = true;
            toggleTileThird.IsChecked = true;
            this.sliderLimit.Value = 8;
        }

        private void sliderLimit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int lv = (int)this.sliderLimit.Value;
            this.textBlockOpacity.Text = lv.ToString();
            Config.OpacityValue = lv;
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            storyboard_2.Begin();
        }
    }
}
