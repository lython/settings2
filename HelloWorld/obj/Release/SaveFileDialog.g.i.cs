﻿#pragma checksum "C:\Users\Administrator\Desktop\HelloWorld\HelloWorld\SaveFileDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38FE2CEF7E6EE923E1F4F025FB4CA1A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.296
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
    
    
    public partial class SaveFileDialog : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.PlaneProjection planeProjection;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox CodeText;
        
        internal System.Windows.Controls.Button okButton;
        
        internal System.Windows.Controls.Button cancelButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HelloWorld;component/SaveFileDialog.xaml", System.UriKind.Relative));
            this.planeProjection = ((System.Windows.Media.PlaneProjection)(this.FindName("planeProjection")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.CodeText = ((System.Windows.Controls.TextBox)(this.FindName("CodeText")));
            this.okButton = ((System.Windows.Controls.Button)(this.FindName("okButton")));
            this.cancelButton = ((System.Windows.Controls.Button)(this.FindName("cancelButton")));
        }
    }
}

