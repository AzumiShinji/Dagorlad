﻿#pragma checksum "..\..\..\Windows\MySettingsWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "180F66A99B6A4BEC67DF33CF6F14B321949B8AAD5E1A0FC0C94470E0DCC4F516"
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
    /// MySettingsWindow
    /// </summary>
    public partial class MySettingsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label AppNameLabel;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label AppVersionLabel;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox IsAutorunCheckBox;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox IsEnabledSmartMenuCheckBox;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton DarkColorSchemeRadioButton;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton LightColorSchemeRadioButton;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EmailTextBox;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\Windows\MySettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenLogsFolderButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Dagorlad;component/windows/mysettingswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\MySettingsWindow.xaml"
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
            
            #line 12 "..\..\..\Windows\MySettingsWindow.xaml"
            ((Dagorlad_7.Windows.MySettingsWindow)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.Window_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AppNameLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.AppVersionLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.IsAutorunCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.IsEnabledSmartMenuCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.DarkColorSchemeRadioButton = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.LightColorSchemeRadioButton = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 8:
            this.EmailTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.OpenLogsFolderButton = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\..\Windows\MySettingsWindow.xaml"
            this.OpenLogsFolderButton.Click += new System.Windows.RoutedEventHandler(this.OpenLogsFolder_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 112 "..\..\..\Windows\MySettingsWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

