#pragma checksum "..\..\..\..\Views\WeightInspectorScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F6CC06E74934682510B27E3F43367D32DD9A1EA17BE3BB8CEA1CB46B2063C994"
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
    /// WeightInspectorScreen
    /// </summary>
    public partial class WeightInspectorScreen : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Views\WeightInspectorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Core.ViewModels.Touch.WeightInspectorViewModel ViewModel;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\..\Views\WeightInspectorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BoxId;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\..\Views\WeightInspectorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CstOrdNo;
        
        #line default
        #line hidden
        
        
        #line 246 "..\..\..\..\Views\WeightInspectorScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock1;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Touch;component/views/weightinspectorscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\WeightInspectorScreen.xaml"
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
            this.ViewModel = ((ECS.Core.ViewModels.Touch.WeightInspectorViewModel)(target));
            return;
            case 2:
            
            #line 59 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 76 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightUpClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 77 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightDownClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BoxId = ((System.Windows.Controls.TextBox)(target));
            
            #line 147 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            this.BoxId.KeyDown += new System.Windows.Input.KeyEventHandler(this.BoxId_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 149 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CstOrdNo = ((System.Windows.Controls.TextBox)(target));
            
            #line 156 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            this.CstOrdNo.KeyDown += new System.Windows.Input.KeyEventHandler(this.CstOrdNo_KeyDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            
            #line 247 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PopupCloseClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 257 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.DataGrid)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DataGrid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 283 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightSearchUpClick);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 284 "..\..\..\..\Views\WeightInspectorScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.WeightSearchDownClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

