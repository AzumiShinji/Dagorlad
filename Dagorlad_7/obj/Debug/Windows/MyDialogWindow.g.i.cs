﻿#pragma checksum "..\..\..\Windows\MyDialogWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1B372742EA491E5A023974C9869D74A33DC9A823AE3F59B892BB9CBFEC8CD642"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Dagorlad_7.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Dagorlad_7.Windows {
    
    
    /// <summary>
    /// MyDialogWindow
    /// </summary>
    public partial class MyDialogWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\Windows\MyDialogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextTextBlock;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Windows\MyDialogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridButtons;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Windows\MyDialogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OkButton;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Windows\MyDialogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button YesButton;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Windows\MyDialogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NoButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Dagorlad;component/windows/mydialogwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\MyDialogWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\Windows\MyDialogWindow.xaml"
            ((Dagorlad_7.Windows.MyDialogWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.GridButtons = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.OkButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\Windows\MyDialogWindow.xaml"
            this.OkButton.Click += new System.Windows.RoutedEventHandler(this.EventClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.YesButton = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\Windows\MyDialogWindow.xaml"
            this.YesButton.Click += new System.Windows.RoutedEventHandler(this.EventClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.NoButton = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\Windows\MyDialogWindow.xaml"
            this.NoButton.Click += new System.Windows.RoutedEventHandler(this.EventClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

