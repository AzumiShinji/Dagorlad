﻿#pragma checksum "..\..\..\Windows\SmartMenuWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4DC86F8421E04E1DA81AF7427EDC8CA0D36E871E0E56F257A48124852604C6AB"
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
    /// SmartMenuWindow
    /// </summary>
    public partial class SmartMenuWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 24 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid DragablePanel;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label HeaderLabel;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid SmartAnswersGrid;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewItemTextBox;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewItemAddButton;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox SmartAnswersListBox;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewItemSubTextBox;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewItemSubAddButton;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox SmartAnswers_Items_ListBox;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\..\Windows\SmartMenuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WindowHideButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Dagorlad;component/windows/smartmenuwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\SmartMenuWindow.xaml"
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
            
            #line 12 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((Dagorlad_7.Windows.SmartMenuWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((Dagorlad_7.Windows.SmartMenuWindow)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.Window_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DragablePanel = ((System.Windows.Controls.Grid)(target));
            
            #line 23 "..\..\..\Windows\SmartMenuWindow.xaml"
            this.DragablePanel.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.DragablePanel_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.HeaderLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.SmartAnswersGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.NewItemTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.NewItemAddButton = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\Windows\SmartMenuWindow.xaml"
            this.NewItemAddButton.Click += new System.Windows.RoutedEventHandler(this.SmartAnswers_NewItemAddButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SmartAnswersListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 14:
            this.NewItemSubTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.NewItemSubAddButton = ((System.Windows.Controls.Button)(target));
            
            #line 130 "..\..\..\Windows\SmartMenuWindow.xaml"
            this.NewItemSubAddButton.Click += new System.Windows.RoutedEventHandler(this.SmartAnswers_NewItemSubAddButton_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.SmartAnswers_Items_ListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 25:
            this.WindowHideButton = ((System.Windows.Controls.Button)(target));
            
            #line 245 "..\..\..\Windows\SmartMenuWindow.xaml"
            this.WindowHideButton.Click += new System.Windows.RoutedEventHandler(this.WindowHideButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 8:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.ListBoxItem.SelectedEvent;
            
            #line 61 "..\..\..\Windows\SmartMenuWindow.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.SmartAnswersListBox_SelectedEvent);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 9:
            
            #line 86 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveItem);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 90 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveItem);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 96 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ShowControlMenu_Click);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 105 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveRootItem_Click);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 109 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelRemove_Click);
            
            #line default
            #line hidden
            break;
            case 17:
            
            #line 159 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ControlContent_Click);
            
            #line default
            #line hidden
            break;
            case 18:
            
            #line 163 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ControlContent_Click);
            
            #line default
            #line hidden
            break;
            case 19:
            
            #line 170 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ControlContent_Click);
            
            #line default
            #line hidden
            break;
            case 20:
            
            #line 175 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ControlContent_Click);
            
            #line default
            #line hidden
            break;
            case 21:
            
            #line 179 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ShowControlMenu_Click);
            
            #line default
            #line hidden
            break;
            case 22:
            
            #line 189 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ControlContent_Click);
            
            #line default
            #line hidden
            break;
            case 23:
            
            #line 193 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelRemove_Click);
            
            #line default
            #line hidden
            break;
            case 24:
            
            #line 231 "..\..\..\Windows\SmartMenuWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ClosePopup_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

