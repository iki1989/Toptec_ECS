#pragma checksum "..\..\..\Views\InvoiceRejectScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "430E5101BFFD78DE9FCFF651F3CB27D9D96325160413953C721F2B23CFCF9AB3"
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
    /// InvoiceRejectScreen
    /// </summary>
    public partial class InvoiceRejectScreen : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Views\InvoiceRejectScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Touch.Views.InvoiceRejectScreen control;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Views\InvoiceRejectScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ECS.Core.ViewModels.Touch.InvoiceRejectViewModel ViewModel;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\Views\InvoiceRejectScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BoxIdTextBox;
        
        #line default
        #line hidden
        
        
        #line 310 "..\..\..\Views\InvoiceRejectScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ManualVerificationBoxId;
        
        #line default
        #line hidden
        
        
        #line 323 "..\..\..\Views\InvoiceRejectScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ManualVerificationInvoiceId;
        
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
            System.Uri resourceLocater = new System.Uri("/ECS.Touch;component/views/invoicerejectscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\InvoiceRejectScreen.xaml"
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
            this.control = ((ECS.Touch.Views.InvoiceRejectScreen)(target));
            return;
            case 2:
            this.ViewModel = ((ECS.Core.ViewModels.Touch.InvoiceRejectViewModel)(target));
            return;
            case 3:
            this.BoxIdTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 175 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.BoxIdTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.BoxIdTextBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 175 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.BoxIdTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.BoxId_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 181 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 186 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ReprintClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 193 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.InvoiceReprintUpClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 194 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.InvoiceReprintDownClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 230 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SameOrderInvoiceUpClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 231 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SameOrderInvoiceDownClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 291 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ManualVerificationCloseClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.ManualVerificationBoxId = ((System.Windows.Controls.TextBox)(target));
            
            #line 310 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.ManualVerificationBoxId.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.ManualVerificationBoxId_IsVisibleChanged);
            
            #line default
            #line hidden
            
            #line 310 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.ManualVerificationBoxId.KeyDown += new System.Windows.Input.KeyEventHandler(this.ManualVerificationBoxId_KeyDown);
            
            #line default
            #line hidden
            
            #line 310 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.ManualVerificationBoxId.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ManualVerificationBoxId_TextChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.ManualVerificationInvoiceId = ((System.Windows.Controls.TextBox)(target));
            
            #line 323 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.ManualVerificationInvoiceId.KeyDown += new System.Windows.Input.KeyEventHandler(this.ManualVerificationKeyDown);
            
            #line default
            #line hidden
            
            #line 323 "..\..\..\Views\InvoiceRejectScreen.xaml"
            this.ManualVerificationInvoiceId.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ManualVerificationInvoiceId_TextChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 325 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ManualVerificationClick);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 336 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Grid)(target)).IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Grid_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 342 "..\..\..\Views\InvoiceRejectScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ErrorMessageCloseClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

