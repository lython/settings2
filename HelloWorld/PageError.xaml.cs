using System;
using Microsoft.Phone.Controls;
using System.Windows.Threading;

namespace HelloWorld
{
    public partial class PageError : PhoneApplicationPage
    {

        private DispatcherTimer stopmusic_timer = new DispatcherTimer(); //定时器

        public PageError()
        {
            InitializeComponent();

            stopmusic_timer.Interval = TimeSpan.FromSeconds(0.5); //停止音乐延时0.5毫秒
            stopmusic_timer.Tick += OnStopMusicTimer;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            myMedia.Source = new Uri("res/Stop.mp3", UriKind.Relative);
            myMedia.Play();
            stopmusic_timer.Start();
            base.OnNavigatedTo(e);
        }

        void OnStopMusicTimer(object sender, EventArgs e)
        {
            stopmusic_timer.Stop();
            App.Current.Terminate();
        }
    }
}