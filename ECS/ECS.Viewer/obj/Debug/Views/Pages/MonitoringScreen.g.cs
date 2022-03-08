﻿#pragma checksum "..\..\..\..\Views\Pages\MonitoringScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CF4C8939BF5B9C6750D86195D39349C0F31F9AA1157E12F06C6BC621A2191B59"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using ECS.Core.ViewModels.Viewer;
using ECS.Viewer.Views.Controls;
using ECS.Viewer.Views.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace ECS.Viewer.Views.Pages {
    
    
    /// <summary>
    /// MonitoringScreen
    /// </summary>
    public partial class MonitoringScreen : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayDate;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost dailyOrderChart;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Viewer.Views.Controls.SimpleChart OrderChart;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayOrderCount;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayCaseErectRejectCount;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayWeightRejectCount;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodaySmartPackingRejectCount;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayTopRejectCount;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock HourlyOutCount;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost yesterdayNotOutChart;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NonOutCount;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost todayOutChart;
        
        #line default
        #line hidden
        
        
        #line 185 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TodayOutCount;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost totalOutChart;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalOutCount;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Viewer;component/views/pages/monitoringscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
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
            
            #line 10 "..\..\..\..\Views\Pages\MonitoringScreen.xaml"
            ((ECS.Viewer.Views.Pages.MonitoringScreen)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.Page_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TodayDate = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.dailyOrderChart = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 4:
            this.OrderChart = ((ECS.Viewer.Views.Controls.SimpleChart)(target));
            return;
            case 5:
            this.TodayOrderCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.TodayCaseErectRejectCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.TodayWeightRejectCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.TodaySmartPackingRejectCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.TodayTopRejectCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.HourlyOutCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.yesterdayNotOutChart = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 12:
            this.NonOutCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 13:
            this.todayOutChart = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 14:
            this.TodayOutCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 15:
            this.totalOutChart = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 16:
            this.TotalOutCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
