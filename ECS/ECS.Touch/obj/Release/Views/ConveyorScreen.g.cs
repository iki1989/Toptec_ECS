﻿#pragma checksum "..\..\..\Views\ConveyorScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "64B4D1D9E9D7E0A397E61A7C53EAE52D01A688C899B6EE500D8EBF03AAB559DA"
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
    /// ConveyorScreen
    /// </summary>
    public partial class ConveyorScreen : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Views\ConveyorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Touch.Views.ConveyorScreen screen;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Views\ConveyorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Core.ViewModels.Touch.ConveyorViewModel ViewModel;
        
        #line default
        #line hidden
        
        
        #line 202 "..\..\..\Views\ConveyorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button auto;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\..\Views\ConveyorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ratio;
        
        #line default
        #line hidden
        
        
        #line 229 "..\..\..\Views\ConveyorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox ratioGroup;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Touch;component/views/conveyorscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ConveyorScreen.xaml"
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
            this.screen = ((ECS.Touch.Views.ConveyorScreen)(target));
            return;
            case 2:
            this.ViewModel = ((ECS.Core.ViewModels.Touch.ConveyorViewModel)(target));
            return;
            case 3:
            
            #line 69 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErectorSpeed1_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 76 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErectorSpeed2_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 83 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErectorSpeed3_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 89 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErectorSpeed4_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 96 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErectorSpeed5_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 125 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice1_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 131 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice2_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 137 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice3_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 143 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice5_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 149 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice6_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 156 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice10_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 163 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice14_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 170 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightInvoice18_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 181 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.TabItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TabItem_MouseDown);
            
            #line default
            #line hidden
            return;
            case 17:
            this.auto = ((System.Windows.Controls.Button)(target));
            
            #line 202 "..\..\..\Views\ConveyorScreen.xaml"
            this.auto.Click += new System.Windows.RoutedEventHandler(this.ToggleClick);
            
            #line default
            #line hidden
            return;
            case 18:
            this.ratio = ((System.Windows.Controls.Button)(target));
            
            #line 214 "..\..\..\Views\ConveyorScreen.xaml"
            this.ratio.Click += new System.Windows.RoutedEventHandler(this.ToggleClick);
            
            #line default
            #line hidden
            return;
            case 19:
            this.ratioGroup = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 20:
            
            #line 238 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 247 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 261 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RouteModeSubmitClick);
            
            #line default
            #line hidden
            return;
            case 23:
            
            #line 320 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Grid)(target)).IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Grid_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 326 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ConveyorSpeedClose_Click);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 340 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ConveyorSpeedPopopSubmitClick);
            
            #line default
            #line hidden
            return;
            case 26:
            
            #line 348 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 27:
            
            #line 375 "..\..\..\Views\ConveyorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErrorMessageCloseClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

