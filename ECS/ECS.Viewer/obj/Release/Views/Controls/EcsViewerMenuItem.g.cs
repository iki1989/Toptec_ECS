﻿#pragma checksum "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A356A1C75A0C36CDEC7E8F214669F740A24459B912C85D30FD61ABC136A49F13"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using ECS.Viewer.Views.Controls;
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


namespace ECS.Viewer.Views.Controls {
    
    
    /// <summary>
    /// EcsViewerMenuItem
    /// </summary>
    public partial class EcsViewerMenuItem : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Viewer.Views.Controls.EcsViewerMenuItem control;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander ExpanderMenu;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock _Header;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewMenu;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Viewer;component/views/controls/ecsviewermenuitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
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
            this.control = ((ECS.Viewer.Views.Controls.EcsViewerMenuItem)(target));
            return;
            case 2:
            this.ExpanderMenu = ((System.Windows.Controls.Expander)(target));
            return;
            case 3:
            this._Header = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.ListViewMenu = ((System.Windows.Controls.ListView)(target));
            
            #line 21 "..\..\..\..\Views\Controls\EcsViewerMenuItem.xaml"
            this.ListViewMenu.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

