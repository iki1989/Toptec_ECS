using ECS.Core.Equipments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace TestWindowsFormsApp
{
    public partial class Form1 : Form
    {
        UserControlWebService userControlWebService = new UserControlWebService("127.0.0.1", 8080);
        UserControlRESTfulWcs userControlRESTful = new UserControlRESTfulWcs();
        UserControlRESTfulRicp userControlRESTfulRicp = new UserControlRESTfulRicp();
        UserControlInkjet userControlInkject = new UserControlInkjet();
        UserControlDynamicScale userControlDynamicScale = new UserControlDynamicScale();
        UserControlBcr userControlBcr = new UserControlBcr();
        UserControlZebraZt411 userControlZebraZt411 = new UserControlZebraZt411();
        UserControlZebraZE500 userControlZebraZE500 = new UserControlZebraZE500();
        UserControlBixolon userControlBixolon = new UserControlBixolon();
        //UserControlTouchPc userControlTouchPc = new UserControlTouchPc();

        public Form1()
        {
            //SplashScreen.SplashScreenRunAsync(new System.Threading.CancellationTokenSource(), this);

            //AutoClosingMessageBox.Show("알림말", "메세지박스타이틀", 1000);
            InitializeComponent();

            this.tabPage1.Controls.Add(userControlWebService);
            this.tabPage2.Controls.Add(userControlRESTful);
            this.tabPage3.Controls.Add(userControlRESTfulRicp);
            this.tabPage4.Controls.Add(userControlInkject);
            this.tabPage5.Controls.Add(userControlDynamicScale);
            this.tabPage6.Controls.Add(userControlBcr);
            this.tabPage7.Controls.Add(userControlZebraZt411);
            this.tabPage8.Controls.Add(userControlZebraZE500);
            this.tabPage9.Controls.Add(userControlBixolon);
           

            this.tabPage1.Text = "WebService";
            this.tabPage2.Text = "RESTful";
            this.tabPage3.Text = "RESTfulRicp";
            this.tabPage4.Text = "Inkject";
            this.tabPage5.Text = "DynamicScale";
            this.tabPage6.Text = "BCR";
            this.tabPage7.Text = "ZebraZT411";
            this.tabPage8.Text = "ZebraZE500";
            this.tabPage9.Text = "Bixolon";
            
            this.tabControl1.SelectedTab = this.tabPage8;

            userControlBcr.SetPirnter(userControlZebraZE500.eq);
        }
    }
}
