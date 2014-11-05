using System;
using Microsoft.Phone.Controls;
using Windows.System;

namespace HelloWorld
{
    public partial class PageBye : PhoneApplicationPage
    {
        public PageBye()
        {
            InitializeComponent();
            PageRestart();
        }

        private async void PageRestart()
        {
            await Launcher.LaunchUriAsync(new System.Uri("test:"));
        }
    }
}