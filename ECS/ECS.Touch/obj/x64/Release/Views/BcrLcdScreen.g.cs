﻿#pragma checksum "..\..\..\..\Views\BcrLcdScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "788D6E89B44C9EC0877705AF9295F61EA08AA22A79548A29FEDE63DABA915929"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using ECS.Core.ViewModels.Touch;
using ECS.Touch.Views;
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


namespace ECS.Touch.Views {
    
    
    /// <summary>
    /// BcrLcdScreen
    /// </summary>
    public partial class BcrLcdScreen : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\Views\BcrLcdScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Core.ViewModels.Touch.BcrLcdViewModel ViewModel;
        
        #line default
        #line hidden
        
        
        #line 1040 "..\..\..\..\Views\BcrLcdScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker OutBcrBegin;
        
        #line default
        #line hidden
        
        
        #line 1056 "..\..\..\..\Views\BcrLcdScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker OutBcrEnd;
        
        #line default
        #line hidden
        
        
        #line 1079 "..\..\..\..\Views\BcrLcdScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutBcrId;
        
        #line default
        #line hidden
        
        
        #line 1185 "..\..\..\..\Views\BcrLcdScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Touch;component/views/bcrlcdscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\BcrLcdScreen.xaml"
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
            this.ViewModel = ((ECS.Core.ViewModels.Touch.BcrLcdViewModel)(target));
            return;
            case 2:
            
            #line 39 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.TabControl)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TabControl_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 112 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartSummaryPrintBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 113 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartSummaryPrintBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 151 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartTopBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 152 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartTopBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 200 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalSummaryPrintBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 201 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalSummaryPrintBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 239 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalTopBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 240 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalTopBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 292 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartRouteBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 293 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartRouteBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 313 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalRouteBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 314 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalRouteBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 363 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartPrintBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 364 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SmartPrintBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 665 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalPrintBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 666 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalPrintBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 963 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TopBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 964 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TopBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 21:
            this.OutBcrBegin = ((System.Windows.Controls.DatePicker)(target));
            
            #line 1040 "..\..\..\..\Views\BcrLcdScreen.xaml"
            this.OutBcrBegin.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.OutBcrDateChanged);
            
            #line default
            #line hidden
            return;
            case 22:
            this.OutBcrEnd = ((System.Windows.Controls.DatePicker)(target));
            
            #line 1056 "..\..\..\..\Views\BcrLcdScreen.xaml"
            this.OutBcrEnd.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.OutBcrDateChanged);
            
            #line default
            #line hidden
            return;
            case 23:
            this.OutBcrId = ((System.Windows.Controls.TextBox)(target));
            
            #line 1079 "..\..\..\..\Views\BcrLcdScreen.xaml"
            this.OutBcrId.KeyDown += new System.Windows.Input.KeyEventHandler(this.OutBcrId_KeyDown);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 1085 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OutBcrSearchClick);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 1086 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OutBcrRefreshClick);
            
            #line default
            #line hidden
            return;
            case 26:
            
            #line 1094 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OutBcrUpClick);
            
            #line default
            #line hidden
            return;
            case 27:
            
            #line 1095 "..\..\..\..\Views\BcrLcdScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OutBcrDownClick);
            
            #line default
            #line hidden
            return;
            case 28:
            this.textBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

