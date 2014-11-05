using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Device.Location;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Networking.Proximity; //蓝牙
using Windows.Phone.Devices.Power;
using Windows.System;

namespace HelloWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly Battery _battery;
        private SolidColorBrush red, green, defaultColor;  //颜色红、绿、默认
        private readonly string pingStr = "固定到开始屏幕";
        private readonly string delStr = "从开始屏幕删除";
        private PhotoChooserTask photoChooser = new PhotoChooserTask();

        private DispatcherTimer looptimer = new DispatcherTimer(); //定时器

        private bool IsBluetooth = false;  //蓝牙打开状态
        private bool IsWifi = false;  //wifi打开状态
        private bool IsCellular = false;  //数据打开状态
        private bool IsNetwork = false;  //网络打开状态
        private bool IsLocation = false;
        private int TileCnt = 0;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            _battery = Battery.GetDefault();
            _battery.RemainingChargePercentChanged += _battery_RemainingChargePercentChanged; //电池

            DeviceStatus.PowerSourceChanged += DeviceStatus_PowerSourceChanged; //判断是外接电源还是电池

            photoChooser.Completed += new EventHandler<PhotoResult>(OnPhotoChooserCompleted); //更新背景

            looptimer.Interval = TimeSpan.FromSeconds(1); //轮询显示网络状态
            looptimer.Tick += OnTimerLoop;
            looptimer.Start();

            DeviceDisconnectFromPower(); //是否接入外部电源
        }

        private void UpdateTileColor()
        {
            //更新磁贴颜色
            red = new SolidColorBrush(ToColor(Lython.ColorList[Config.openColor].ColorPath));
            green = new SolidColorBrush(ToColor(Lython.ColorList[Config.closeColor].ColorPath));
            defaultColor = new SolidColorBrush(ToColor(Lython.ColorList[Config.defaultColor].ColorPath));

            this.tile_airplane.Background = defaultColor;
            this.tile_bluetooth.Background = defaultColor;
            this.tile_location.Background = defaultColor;
            this.tile_stopmusic.Background = defaultColor;
            this.tile_battery.Background = defaultColor;
            this.tile_cellular.Background = defaultColor;
            this.tile_wifi.Background = defaultColor;
            this.tile_lock.Background = defaultColor;
            this.tile_screenrotation.Background = defaultColor;
            this.tile_restart.Background = defaultColor;
            this.tile_account.Background = defaultColor;
            this.tile_flashlight.Background = defaultColor;
            this.tile_wallet.Background = defaultColor;
            this.tile_stopwatch.Background = defaultColor;
            this.tile_data.Background = defaultColor;
            this.ButtonSkin.Background = defaultColor;
            this.ButtonSettings.Background = defaultColor;
            this.ButtonAbout.Background = defaultColor;
        }

        private Color ToColor(string colorName)
        {
            //16进制转换成颜色
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }

        //定时器，更新网络信息
        void OnTimerLoop(object sender, EventArgs e)
        {
            NetWorkChanged();
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

        //点击瓷贴跳转到相关页面
        private void TileLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string index = (sender as Tile).Name;
            GotoOtherPage(index);
        }

        #region 跳转到其他窗口
        private async void GotoOtherPage(string index)
        {
            switch (index)
            {
                case "tile_cellular": //手机网络
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-cellular:"));
                    break;
                case "tile_wifi": //WiFi
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
                    break;
                case "tile_airplane": //飞行模式
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-airplanemode:"));
                    break;
                case "tile_bluetooth": //蓝牙
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                    break;
                case "tile_location": //位置服务
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                    break;
                case "tile_lock": //锁屏
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
                    break;
                case "tile_stopmusic": //停止音乐
                    StopMusic();
                    break;
                case "tile_account": //电子邮件 + 账户
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-emailandaccounts:"));
                    break;
                case "tile_wallet": //电子钱包
                    await Launcher.LaunchUriAsync(new Uri("wallet:"));
                    break;
                case "tile_flashlight": //手电筒
                    await Launcher.LaunchUriAsync(new System.Uri("flashlight:"));
                    break;
                case "tile_stopwatch": //秒表
                    await Launcher.LaunchUriAsync(new System.Uri("stopwatch:"));
                    break;
                case "tile_restart": //重启
                    if (MessageBox.Show("", "重启手机？", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //await Launcher.LaunchUriAsync(new System.Uri("test:"));
                        NavigationService.Navigate(new Uri("/PageBye.xaml", UriKind.Relative));
                    }
                    break;
                case "tile_battery": //节电模式
                    if (OSVersionIsGDR3() == false)
                    {
                        MessageBox.Show("此功能需要GDR 3支持，请更新你的系统。", "不是GDR 3！", MessageBoxButton.OK);
                        return;
                    }
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-power:"));
                    break;
                case "tile_screenrotation": //屏幕旋转
                    if (OSVersionIsGDR3() == false)
                    {
                        MessageBox.Show("此功能需要GDR 3支持，请更新你的系统。", "不是GDR 3！", MessageBoxButton.OK);
                        return;
                    }
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-screenrotation:"));
                    break;
            }
        }
        #endregion

        #region 将磁贴ping到桌面或从桌面删除
        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            int on, off;
            string title_on, title_off, net_true, net_false;
            if (Config.IsTileNotie)
            {
                on = Config.openColor;
                off = Config.closeColor;
                title_on = "：开";
                title_off = "：关";
                net_true = "：有";
                net_false = "：无";
            }
            else
            {
                on = off = 0;
                title_on = title_off = net_true = net_false = "";
            }

            string index = (sender as MenuItem).Name;
            switch (index)
            {
                case "MenuItem_cellular": //手机网络
                    ShellTile TileToFind_cellular = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_cellular"));
                    if (TileToFind_cellular == null)
                    {
                        StandardTileData tile_cellular = new StandardTileData();
                        if (IsCellular)
                        {
                            tile_cellular.Title = String.Format("手机网络{0}", title_on);
                            tile_cellular.BackgroundImage = new Uri(String.Format("/tile/Cellular_{0}.png", on), UriKind.Relative);
                        }
                        else
                        {
                            tile_cellular.Title = String.Format("手机网络{0}", title_off);
                            tile_cellular.BackgroundImage = new Uri(String.Format("/tile/Cellular_{0}.png", off), UriKind.Relative);
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_cellular", UriKind.Relative), tile_cellular);
                    }
                    else
                    {
                        TileToFind_cellular.Delete();
                    }
                    break;
                case "MenuItem_wifi": //WIFI
                    ShellTile TileToFind_wifi = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_wifi"));
                    if (TileToFind_wifi == null)
                    {
                        StandardTileData tile_wifi = new StandardTileData();
                        if (IsWifi)
                        {
                            tile_wifi.Title = String.Format("Wi-Fi{0}", title_on);
                            tile_wifi.BackgroundImage = new Uri(String.Format("/tile/WiFi_{0}.png", on), UriKind.Relative);
                        }
                        else
                        {
                            tile_wifi.Title = String.Format("Wi-Fi{0}", title_off);
                            tile_wifi.BackgroundImage = new Uri(String.Format("/tile/WiFi_{0}.png", off), UriKind.Relative);
                        }
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_wifi", UriKind.Relative), tile_wifi);
                    }
                    else
                    {
                        TileToFind_wifi.Delete();
                    }
                    break;
                case "MenuItem_bluetooth": //蓝牙
                    ShellTile TileToFind_bluetooth = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_bluetooth"));
                    if (TileToFind_bluetooth == null)
                    {
                        StandardTileData tile_bluetooth = new StandardTileData();
                        if (IsBluetooth)
                        {
                            tile_bluetooth.Title = String.Format("蓝牙{0}", title_on);
                            tile_bluetooth.BackgroundImage = new Uri(String.Format("/tile/Bluetooth_{0}.png", on), UriKind.Relative);
                        }
                        else
                        {
                            tile_bluetooth.Title = String.Format("蓝牙{0}", title_off);
                            tile_bluetooth.BackgroundImage = new Uri(String.Format("/tile/Bluetooth_{0}.png", off), UriKind.Relative);
                        }
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_bluetooth", UriKind.Relative), tile_bluetooth);
                    }
                    else
                    {
                        TileToFind_bluetooth.Delete();
                    }
                    break;
                case "MenuItem_location": //定位
                    ShellTile TileToFind_location = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_location"));
                    if (TileToFind_location == null)
                    {
                        StandardTileData tile_location = new StandardTileData();
                        if (IsLocation)
                        {
                            tile_location.Title = String.Format("定位{0}", title_on);
                            tile_location.BackgroundImage = new Uri(String.Format("/tile/location_{0}.png", on), UriKind.Relative);
                        }
                        else
                        {
                            tile_location.Title = String.Format("定位{0}", title_off);
                            tile_location.BackgroundImage = new Uri(String.Format("/tile/location_{0}.png", off), UriKind.Relative);
                        }
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_location", UriKind.Relative), tile_location);
                    }
                    else
                    {
                        TileToFind_location.Delete();
                    }
                    break;
                case "MenuItem_data": //可用网络
                    ShellTile TileToFind_data = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_data"));
                    if (TileToFind_data == null)
                    {
                        StandardTileData tile_data = new StandardTileData();
                        if (IsNetwork)
                        {
                            tile_data.Title = String.Format("可用网络{0}", net_true);
                            tile_data.BackgroundImage = new Uri(String.Format("/tile/data_{0}.png", on), UriKind.Relative);
                        }
                        else
                        {
                            tile_data.Title = String.Format("可用网络{0}", net_false);
                            tile_data.BackgroundImage = new Uri(String.Format("/tile/data_{0}.png", off), UriKind.Relative);
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_data", UriKind.Relative), tile_data);
                    }
                    else
                    {
                        TileToFind_data.Delete();
                    }
                    break;
                case "MenuItem_airplane": //飞行模式
                    ShellTile TileToFind_airplane = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_airplane"));
                    if (TileToFind_airplane == null)
                    {
                        StandardTileData tile_airplane = new StandardTileData
                        {
                            Title = "飞行模式",
                            BackgroundImage = new Uri("/tile/AirplaneMode.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_airplane", UriKind.Relative), tile_airplane);
                    }
                    else
                    {
                        TileToFind_airplane.Delete();
                    }
                    break;
                case "MenuItem_stopmusic": //停止音乐
                    ShellTile TileToFind_stopmusic = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_stopmusic"));
                    if (TileToFind_stopmusic == null)
                    {
                        StandardTileData tile_stopmusic = new StandardTileData
                        {
                            Title = "停止音乐",
                            BackgroundImage = new Uri("/tile/stop.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/PageError.xaml?theTitle=tile_stopmusic", UriKind.Relative), tile_stopmusic);
                    }
                    else
                    {
                        TileToFind_stopmusic.Delete();
                    }
                    break;
                case "MenuItem_lock": //锁屏
                    ShellTile TileToFind_lock = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_lock"));
                    if (TileToFind_lock == null)
                    {
                        StandardTileData tile_lock = new StandardTileData
                        {
                            Title = "锁屏界面",
                            BackgroundImage = new Uri("/tile/link_win8.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_lock", UriKind.Relative), tile_lock);
                    }
                    else
                    {
                        TileToFind_lock.Delete();
                    }
                    break;
                case "MenuItem_account": //电子邮件+账户
                    ShellTile TileToFind_account = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_account"));
                    if (TileToFind_account == null)
                    {
                        StandardTileData tile_account = new StandardTileData
                        {
                            Title = "电子邮件+账户",
                            BackgroundImage = new Uri("/tile/microsoft.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_account", UriKind.Relative), tile_account);
                    }
                    else
                    {
                        TileToFind_account.Delete();
                    }
                    break;
                case "MenuItem_battery": //tile_battery
                    if (OSVersionIsGDR3() == false)
                    {
                        MessageBox.Show("此功能需要GDR 3支持，请更新你的系统。", "不是GDR 3！", MessageBoxButton.OK);
                        return;
                    }
                    ShellTile TileToFind_battery = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_battery"));
                    if (TileToFind_battery == null)
                    {
                        StandardTileData tile_battery = new StandardTileData
                        {
                            Title = "节电模式",
                            BackgroundImage = new Uri("/tile/battery.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_battery", UriKind.Relative), tile_battery);
                    }
                    else
                    {
                        TileToFind_battery.Delete();
                    }
                    break;
                case "MenuItem_screenrotation": //tile_screenrotation
                    if (OSVersionIsGDR3() == false)
                    {
                        MessageBox.Show("此功能需要GDR 3支持，请更新你的系统。", "不是GDR 3！", MessageBoxButton.OK);
                        return;
                    }
                    ShellTile TileToFind_screenrotation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_screenrotation"));
                    if (TileToFind_screenrotation == null)
                    {
                        StandardTileData tile_screenrotation = new StandardTileData
                        {
                            Title = "屏幕旋转",
                            BackgroundImage = new Uri("/tile/screenrotation.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_screenrotation", UriKind.Relative), tile_screenrotation);
                    }
                    else
                    {
                        TileToFind_screenrotation.Delete();
                    }
                    break;
                case "MenuItem_wallet": //tile_wallet
                    ShellTile TileToFind_wallet = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_wallet"));
                    if (TileToFind_wallet == null)
                    {
                        StandardTileData tile_wallet = new StandardTileData
                        {
                            Title = "电子钱包",
                            BackgroundImage = new Uri("/tile/wallet.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_wallet", UriKind.Relative), tile_wallet);
                    }
                    else
                    {
                        TileToFind_wallet.Delete();
                    }
                    break;
                case "MenuItem_flashlight": //tile_flashlight
                    ShellTile TileToFind_flashlight = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_flashlight"));
                    if (TileToFind_flashlight == null)
                    {
                        StandardTileData tile_flashlight = new StandardTileData
                        {
                            Title = "手电筒",
                            BackgroundImage = new Uri("/tile/flashlight.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_flashlight", UriKind.Relative), tile_flashlight);
                    }
                    else
                    {
                        TileToFind_flashlight.Delete();
                    }
                    break;
                case "MenuItem_stopwatch": //tile_stopwatch
                    ShellTile TileToFind_stopwatch = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_stopwatch"));
                    if (TileToFind_stopwatch == null)
                    {
                        StandardTileData tile_stopwatch = new StandardTileData
                        {
                            Title = "秒表",
                            BackgroundImage = new Uri("/tile/stopwatch.png", UriKind.Relative)
                        };
                        ShellTile.Create(new Uri("/MainPage.xaml?theTitle=tile_stopwatch", UriKind.Relative), tile_stopwatch);
                    }
                    else
                    {
                        TileToFind_stopwatch.Delete();
                    }
                    break;
                case "MenuItem_restart": //tile_restart
                    ShellTile TileToFind_restart = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_restart"));
                    if (TileToFind_restart == null)
                        {
                            StandardTileData tile_restart = new StandardTileData
                            {
                                Title = "重启",
                                BackgroundImage = new Uri("/tile/restart.png", UriKind.Relative)
                            };
                            ShellTile.Create(new Uri("/PageRestart.xaml?theTitle=tile_restart", UriKind.Relative), tile_restart);
                        }
                        else
                        {
                            TileToFind_restart.Delete();
                        }
                        break;
            }
        }
        #endregion

        #region 更新桌面磁贴
        private void UpdateDesktopTile()
        {
            int on, off;
            string title_on, title_off, net_true, net_false;
            if (Config.IsTileNotie)
            {
                on = Config.openColor;
                off = Config.closeColor;
                title_on = "：开";
                title_off = "：关";
                net_true = "：有";
                net_false = "：无";
            }
            else
            {
                on = off = 0;
                title_on = title_off = net_true = net_false = "";
            }
            ShellTile TileToFind_cellular = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_cellular"));
            ShellTile TileToFind_wifi = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_wifi"));
            ShellTile TileToFind_data = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_data"));
            ShellTile TileToFind_bluetooth = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_bluetooth"));
            ShellTile TileToFind_location = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_location"));
            if (TileToFind_cellular != null)
            {
                StandardTileData tile_cellular = new StandardTileData();
                if (IsCellular)
                {
                    tile_cellular.Title = String.Format("手机网络{0}", title_on);
                    tile_cellular.BackgroundImage = new Uri(String.Format("/tile/Cellular_{0}.png", on), UriKind.Relative);
                }
                else
                {
                    tile_cellular.Title = String.Format("手机网络{0}", title_off);
                    tile_cellular.BackgroundImage = new Uri(String.Format("/tile/Cellular_{0}.png", off), UriKind.Relative);
                };
                TileToFind_cellular.Update(tile_cellular);
            }
            if (TileToFind_wifi != null)
            {
                StandardTileData tile_wifi = new StandardTileData();
                if (IsWifi)
                {
                    tile_wifi.Title = String.Format("Wi-Fi{0}", title_on);
                    tile_wifi.BackgroundImage = new Uri(String.Format("/tile/WiFi_{0}.png", on), UriKind.Relative);
                }
                else
                {
                    tile_wifi.Title = String.Format("Wi-Fi{0}", title_off);
                    tile_wifi.BackgroundImage = new Uri(String.Format("/tile/WiFi_{0}.png", off), UriKind.Relative);
                }
                TileToFind_wifi.Update(tile_wifi);
            }
            if (TileToFind_data != null)
            {
                StandardTileData tile_data = new StandardTileData();
                if (IsNetwork)
                {
                    tile_data.Title = String.Format("可用网络{0}", net_true);
                    tile_data.BackgroundImage = new Uri(String.Format("/tile/data_{0}.png", on), UriKind.Relative);
                }
                else
                {
                    tile_data.Title = String.Format("可用网络{0}", net_false);
                    tile_data.BackgroundImage = new Uri(String.Format("/tile/data_{0}.png", off), UriKind.Relative);
                };
                TileToFind_data.Update(tile_data);
            }
            if (TileToFind_bluetooth != null)
            {
                StandardTileData tile_bluetooth = new StandardTileData();
                if (IsBluetooth)
                {
                    tile_bluetooth.Title = String.Format("蓝牙{0}", title_on);
                    tile_bluetooth.BackgroundImage = new Uri(String.Format("/tile/Bluetooth_{0}.png", on), UriKind.Relative);
                }
                else
                {
                    tile_bluetooth.Title = String.Format("蓝牙{0}", title_off);
                    tile_bluetooth.BackgroundImage = new Uri(String.Format("/tile/Bluetooth_{0}.png", off), UriKind.Relative);
                }
                TileToFind_bluetooth.Update(tile_bluetooth);
            }
            if (TileToFind_location != null)
            {
                StandardTileData tile_location = new StandardTileData();
                if (IsLocation)
                {
                    tile_location.Title = String.Format("定位{0}", title_on);
                    tile_location.BackgroundImage = new Uri(String.Format("/tile/location_{0}.png", on), UriKind.Relative);
                }
                else
                {
                    tile_location.Title = String.Format("定位{0}", title_off);
                    tile_location.BackgroundImage = new Uri(String.Format("/tile/location_{0}.png", off), UriKind.Relative);
                }
                TileToFind_location.Update(tile_location);
            }
        }
        #endregion

        #region 长按磁贴更新环境菜单
        private void tile_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string index = (sender as Tile).Name;
            switch (index)
            {
                case "tile_cellular": //手机网络
                    ShellTile TileToFind_cellular = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_cellular"));
                    MenuItem_cellular.Header = (TileToFind_cellular != null) ? delStr : pingStr;
                    break;
                case "tile_wifi": //WIFI
                    ShellTile TileToFind_wifi = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_wifi"));
                    MenuItem_wifi.Header = (TileToFind_wifi != null) ? delStr : pingStr;
                    break;
                case "tile_airplane": //飞行
                    ShellTile TileToFind_airplane = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_airplane"));
                    MenuItem_airplane.Header = (TileToFind_airplane != null) ? delStr : pingStr;
                    break;
                case "tile_bluetooth": //蓝牙
                    ShellTile TileToFind_bluetooth = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_bluetooth"));
                    MenuItem_bluetooth.Header = (TileToFind_bluetooth != null) ? delStr : pingStr;
                    break;
                case "tile_location": //定位
                    ShellTile TileToFind_location = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_location"));
                    MenuItem_location.Header = (TileToFind_location != null) ? delStr : pingStr;
                    break;
                case "tile_stopmusic": //停止音乐
                    ShellTile TileToFind_stopmusic = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_stopmusic"));
                    MenuItem_stopmusic.Header = (TileToFind_stopmusic != null) ? delStr : pingStr;
                    break;
                case "tile_lock": //锁屏界面
                    ShellTile TileToFind_lock = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_lock"));
                    MenuItem_lock.Header = (TileToFind_lock != null) ? delStr : pingStr;
                    break;
                case "tile_account": //电子邮件+账户
                    ShellTile TileToFind_account = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_account"));
                    MenuItem_account.Header = (TileToFind_account != null) ? delStr : pingStr;
                    break;
                case "tile_battery": //battery
                    ShellTile TileToFind_battery = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_battery"));
                    MenuItem_battery.Header = (TileToFind_battery != null) ? delStr : pingStr;
                    break;
                case "tile_screenrotation": //screenrotation
                    ShellTile TileToFind_screenrotation = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_screenrotation"));
                    MenuItem_screenrotation.Header = (TileToFind_screenrotation != null) ? delStr : pingStr;
                    break;
                case "tile_wallet": //电子钱包
                    ShellTile TileToFind_wallet = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_wallet"));
                    MenuItem_wallet.Header = (TileToFind_wallet != null) ? delStr : pingStr;
                    break;
                case "tile_flashlight": //手电筒
                    ShellTile TileToFind_flashlight = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_flashlight"));
                    MenuItem_flashlight.Header = (TileToFind_flashlight != null) ? delStr : pingStr;
                    break;
                case "tile_stopwatch": //秒表
                    ShellTile TileToFind_stopwatch = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_stopwatch"));
                    MenuItem_stopwatch.Header = (TileToFind_stopwatch != null) ? delStr : pingStr;
                    break;
                case "tile_restart": //重启
                    ShellTile TileToFind_restart = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_restart"));
                    MenuItem_restart.Header = (TileToFind_restart != null) ? delStr : pingStr;
                    break;
            }
        }

        private void tile_data_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShellTile TileToFind_data = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("theTitle=tile_data"));
            MenuItem_data.Header = (TileToFind_data != null) ? delStr : pingStr;
        }
        #endregion

        #region 电量更新,蓝牙状态，网络变化，位置信息，停止音乐
        //电量更新
        void _battery_RemainingChargePercentChanged(object sender, object e)
        {
            UpdateBattery();
        }

        void UpdateBattery()
        {
            int bat_Per = 100;
            bat_Per = _battery.RemainingChargePercent;
            notice_battery.Content = String.Format("{0} %", bat_Per);
            tile_battery.Title = String.Format("{0} 小时", Math.Round(_battery.RemainingDischargeTime.TotalHours,2));
            if (bat_Per <= 20)
            {
                tile_battery.Background = red;
            }
            else
            {
                tile_battery.Background = defaultColor;
            }
        }

        void DeviceStatus_PowerSourceChanged(object sender, object e)
        {
            this.Dispatcher.BeginInvoke(DeviceDisconnectFromPower);
        }

        void DeviceDisconnectFromPower()
        {
            if (DeviceStatus.PowerSource.ToString() == "External")
            {
                Uri uri = new Uri("img/battery_out.png", UriKind.Relative);
                BitmapImage bmp = new BitmapImage(uri);
                image_battery.Source = bmp;
            }
            else
            {
                Uri uri = new Uri("img/battery.png", UriKind.Relative);
                BitmapImage bmp = new BitmapImage(uri);
                image_battery.Source = bmp;
            }
        }

        //蓝牙开关状态
        private async void FindPaired()
        {
            // Search for all paired devices
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            try
            {
                var peers = await PeerFinder.FindAllPeersAsync();
                IsBluetooth = true;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x8007048F)
                {
                    IsBluetooth = false;
                }
            }
            if (IsBluetooth)
            {
                tile_bluetooth.Background = red;
                tile_bluetooth.Title = "蓝牙：开";
            }
            else
            {
                tile_bluetooth.Background = green;
                tile_bluetooth.Title = "蓝牙：关";
            }
        }

        //网络发生变化
        void NetWorkChanged()
        {
            IsWifi = DeviceNetworkInformation.IsWiFiEnabled;
            IsCellular = DeviceNetworkInformation.IsCellularDataEnabled;
            IsNetwork = DeviceNetworkInformation.IsNetworkAvailable;
            tile_data.Background = (IsNetwork) ? red : green;
            if (IsCellular)
            {
                tile_cellular.Background = red;
                tile_cellular.Title = "手机网络：开";
            }
            else
            {
                tile_cellular.Background = green;
                tile_cellular.Title = "手机网络：关";
            }
            if (IsWifi)
            {
                tile_wifi.Background = red;
                tile_wifi.Title = "Wi-Fi：开";
            }
            else
            {
                tile_wifi.Background = green;
                tile_wifi.Title = "Wi-Fi：关";
            }
            //RAM情况
            //notice_ram.Content = 
            //    String.Format("{0} M", Math.Round(DeviceStatus.ApplicationCurrentMemoryUsage/1048576.0, 3));
        }

        //位置权限
        private void WatherPermissionUpdates()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            if (watcher.Permission == GeoPositionPermission.Granted)
            {
                IsLocation = true;
                tile_location.Background = red;
                tile_location.Title = "定位：开";
                
            }
            else if (watcher.Permission == GeoPositionPermission.Denied)
            {
                IsLocation = false;
                tile_location.Background = green;
                tile_location.Title = "定位：关";
            }
            else
            {
                tile_location.Background = defaultColor;
                tile_location.Title = "定位：未知";
            }
        }

        //停止音乐
        private void StopMusic()
        {
            myMedia.Source = new Uri("res/Stop.mp3", UriKind.Relative);
            myMedia.Play();
            ToastPrompt toast = new ToastPrompt
            {
                Title = "音乐已停止",
            };
            toast.Show();
        }
        #endregion

        #region 工具栏、换皮肤
        private void ButtonAbout_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //关于
            NavigationService.Navigate(new Uri("/PivotPageAbout.xaml", UriKind.Relative));
        }

        private void ButtonSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //设置
            NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.Relative));
        }

        private void ButtonStar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //评价
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void ButtonSkin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //自定义背景
            photoChooser.PixelWidth = 1280;
            photoChooser.PixelHeight = 768;
            photoChooser.ShowCamera = true;
            photoChooser.Show();
        }

        void OnPhotoChooserCompleted(object sender, PhotoResult e)
        {
            if (e.Error == null && e.ChosenPhoto != null)
            {
                ImageBrush img = new ImageBrush();
                WriteableBitmap bmp = Microsoft.Phone.PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
                string filename = e.OriginalFileName.Substring(e.OriginalFileName.LastIndexOf('\\') + 1);
                if (isf.FileExists(filename)) //如果已经存在这个文件，则将这个文件删除
                {
                    isf.DeleteFile(filename);
                }
                IsolatedStorageFileStream PhotoStream = isf.CreateFile(filename);
                Extensions.SaveJpeg(bmp, PhotoStream, bmp.PixelWidth, bmp.PixelHeight, 0, 85); //这里设置保存后图片的大小
                PhotoStream.Close();    //写入完毕，关闭文件流

                img.ImageSource = bmp;
                this.myPage.Background = img;
                try
                {
                    if (isf.FileExists(Config.BackImg)) isf.DeleteFile(Config.BackImg); //删除上一个皮肤
                }
                catch { };
                Config.BackImg = filename;
                Config.IsBackground = true;
            }
        }
        #endregion

        private bool OSVersionIsGDR3()
        {
            string OSVersion = Environment.OSVersion.ToString();   //OS版本
            string[] aa = OSVersion.Split('.');
            Int64 ver_current = Convert.ToInt32(aa[1]) * 100000 + Convert.ToInt32(aa[2]);
            if (ver_current > 10400)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        #region 墓碑化
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            try
            {
                string index = this.NavigationContext.QueryString["theTitle"];
                if (index == "tile_stopmusic") //如果是停止音乐则跳转至停止音乐界面
                {
                    return;
                }
                else if (index == "tile_data") //如果是网络状态瓷帖进入则相当于主瓷帖进入
                {
                    mainTileUpdateUI();
                    base.OnNavigatedTo(args);
                    return;
                }
                else
                {
                    if (TileCnt == 0) //第一次进入不用销毁，第二次进入销毁
                    {
                        TileCnt++;
                        GotoOtherPage(index);
                        if (Config.IsTileNotie)
                        {
                            FindPaired(); //蓝牙比较慢，所以在进入的时候也判断一下
                        }
                        else
                        {
                            App.Current.Terminate(); //如果关闭了瓷帖更新则直接关掉
                        }
                    }
                    else if (TileCnt >= 1) //第二次进入相当于按下返回键返回界面
                    {
                        FindPaired(); //判断蓝牙状态
                        WatherPermissionUpdates(); //更新位置服务
                        NetWorkChanged(); //更新网络变化，包括数据连接、wifi
                        UpdateDesktopTile(); //更新桌面磁贴
                        App.Current.Terminate();
                    }
                }
            }
            catch
            { //主磁贴进入会触发异常
                mainTileUpdateUI();
                base.OnNavigatedTo(args);
            }
        }

        void mainTileUpdateUI()
        {
            //主瓷帖进入更新界面UI
            UpdateTileColor(); //更新颜色
            WatherPermissionUpdates(); //位置服务
            NetWorkChanged(); //网络变化，包括数据连接、飞行模式、wifi
            FindPaired(); //判断蓝牙状态
            UpdateBattery(); //更新电池
            UpdateDesktopTile(); //更新磁贴
        }
        #endregion

        private void myPage_Loaded(object sender, RoutedEventArgs e)
        {
            //加载完成后再换皮肤
            try
            {  //换皮肤
                ImageBrush img = new ImageBrush();
                BitmapImage bmp = new BitmapImage();
                if (Config.IsBackground)
                {
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(Config.BackImg, FileMode.Open, FileAccess.Read))
                        {
                            bmp.SetSource(fileStream);
                            img.ImageSource = bmp;
                        }
                    }
                }
                else
                {
                    Uri uri = new Uri("assets/bing.jpg", UriKind.Relative);
                    bmp.UriSource = uri;
                    img.ImageSource = bmp;
                }
                this.myPage.Background = img;
            }
            catch { }
            this.tile_flashlight.Visibility = (Config.IsThirdApp) ? Visibility.Visible : Visibility.Collapsed;
            this.tile_stopwatch.Visibility = (Config.IsThirdApp) ? Visibility.Visible : Visibility.Collapsed;
            this.Layout_1.Opacity = Config.OpacityValue / 10.0;
            this.Layout_2.Opacity = Config.OpacityValue / 10.0;
        }
    }
}