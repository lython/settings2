﻿#pragma checksum "E:\Windows Phone\提交App\设置lite\1.4.0.5\设置_1.4.0.5\HelloWorld\SettingPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0FA0F1FCF0ABFADBDB1E6CBCCFD66C6E"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34003
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace HelloWorld {
    
    
    public partial class SettingPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.PlaneProjection translate;
        
        internal System.Windows.Media.Animation.Storyboard storyboard_1;
        
        internal System.Windows.Media.Animation.Storyboard storyboard_2;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.ListPicker listPickerOpen;
        
        internal Microsoft.Phone.Controls.ListPicker listPickerClose;
        
        internal Microsoft.Phone.Controls.ListPicker listPickerDefault;
        
        internal Microsoft.Phone.Controls.ToggleSwitch toggleBackground;
        
        internal Microsoft.Phone.Controls.ToggleSwitch toggleTileNotie;
        
        internal Microsoft.Phone.Controls.ToggleSwitch toggleTileThird;
        
        internal System.Windows.Controls.TextBlock textBlockOpacity;
        
        internal System.Windows.Controls.Slider sliderLimit;
        
        internal System.Windows.Controls.HyperlinkButton linkButtonReset;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/HelloWorld;component/SettingPage.xaml", System.UriKind.Relative));
            this.translate = ((System.Windows.Media.PlaneProjection)(this.FindName("translate")));
            this.storyboard_1 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("storyboard_1")));
            this.storyboard_2 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("storyboard_2")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.listPickerOpen = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("listPickerOpen")));
            this.listPickerClose = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("listPickerClose")));
            this.listPickerDefault = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("listPickerDefault")));
            this.toggleBackground = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("toggleBackground")));
            this.toggleTileNotie = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("toggleTileNotie")));
            this.toggleTileThird = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("toggleTileThird")));
            this.textBlockOpacity = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockOpacity")));
            this.sliderLimit = ((System.Windows.Controls.Slider)(this.FindName("sliderLimit")));
            this.linkButtonReset = ((System.Windows.Controls.HyperlinkButton)(this.FindName("linkButtonReset")));
        }
    }
}

