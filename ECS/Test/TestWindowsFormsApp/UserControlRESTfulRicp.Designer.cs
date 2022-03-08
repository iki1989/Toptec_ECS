
namespace TestWindowsFormsApp
{
    partial class UserControlRESTfulRicp
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonDeviceStatus = new System.Windows.Forms.Button();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonAlive = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonContainerScan = new System.Windows.Forms.Button();
            this.buttonInvoiceScan = new System.Windows.Forms.Button();
            this.buttonOutInvoiceScan = new System.Windows.Forms.Button();
            this.buttonLocationPointPush = new System.Windows.Forms.Button();
            this.buttonLocationPointStatus = new System.Windows.Forms.Button();
            this.buttonRolltainerSensorStatus = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonRmsStatusSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDeviceStatus
            // 
            this.buttonDeviceStatus.Location = new System.Drawing.Point(3, 27);
            this.buttonDeviceStatus.Name = "buttonDeviceStatus";
            this.buttonDeviceStatus.Size = new System.Drawing.Size(171, 23);
            this.buttonDeviceStatus.TabIndex = 9;
            this.buttonDeviceStatus.Text = "DeviceStatus Sample Data";
            this.buttonDeviceStatus.UseVisualStyleBackColor = true;
            this.buttonDeviceStatus.Click += new System.EventHandler(this.buttonDeviceStatus_Click);
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(8, 3);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(20, 12);
            this.labelUrl.TabIndex = 6;
            this.labelUrl.Text = "Url";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(34, 0);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(165, 21);
            this.textBoxUrl.TabIndex = 5;
            this.textBoxUrl.Text = "http://127.0.0.1:8081";
            // 
            // buttonAlive
            // 
            this.buttonAlive.Location = new System.Drawing.Point(181, 27);
            this.buttonAlive.Name = "buttonAlive";
            this.buttonAlive.Size = new System.Drawing.Size(113, 23);
            this.buttonAlive.TabIndex = 12;
            this.buttonAlive.Text = "Alive";
            this.buttonAlive.UseVisualStyleBackColor = true;
            this.buttonAlive.Click += new System.EventHandler(this.buttonAlive_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxLog.Location = new System.Drawing.Point(317, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(406, 585);
            this.richTextBoxLog.TabIndex = 22;
            this.richTextBoxLog.Text = "";
            // 
            // buttonContainerScan
            // 
            this.buttonContainerScan.Location = new System.Drawing.Point(3, 56);
            this.buttonContainerScan.Name = "buttonContainerScan";
            this.buttonContainerScan.Size = new System.Drawing.Size(171, 23);
            this.buttonContainerScan.TabIndex = 25;
            this.buttonContainerScan.Text = "ContainerScan Sample Data";
            this.buttonContainerScan.UseVisualStyleBackColor = true;
            this.buttonContainerScan.Click += new System.EventHandler(this.buttonContainerScan_Click);
            // 
            // buttonInvoiceScan
            // 
            this.buttonInvoiceScan.Location = new System.Drawing.Point(3, 85);
            this.buttonInvoiceScan.Name = "buttonInvoiceScan";
            this.buttonInvoiceScan.Size = new System.Drawing.Size(171, 23);
            this.buttonInvoiceScan.TabIndex = 26;
            this.buttonInvoiceScan.Text = "InvoiceScan Sample Data";
            this.buttonInvoiceScan.UseVisualStyleBackColor = true;
            this.buttonInvoiceScan.Click += new System.EventHandler(this.buttonInvoiceScan_Click);
            // 
            // buttonOutInvoiceScan
            // 
            this.buttonOutInvoiceScan.Location = new System.Drawing.Point(3, 114);
            this.buttonOutInvoiceScan.Name = "buttonOutInvoiceScan";
            this.buttonOutInvoiceScan.Size = new System.Drawing.Size(171, 23);
            this.buttonOutInvoiceScan.TabIndex = 27;
            this.buttonOutInvoiceScan.Text = "OutInvoiceScan Sample Data";
            this.buttonOutInvoiceScan.UseVisualStyleBackColor = true;
            this.buttonOutInvoiceScan.Click += new System.EventHandler(this.buttonOutInvoiceScan_Click);
            // 
            // buttonLocationPointPush
            // 
            this.buttonLocationPointPush.Location = new System.Drawing.Point(3, 143);
            this.buttonLocationPointPush.Name = "buttonLocationPointPush";
            this.buttonLocationPointPush.Size = new System.Drawing.Size(171, 23);
            this.buttonLocationPointPush.TabIndex = 28;
            this.buttonLocationPointPush.Text = "LocationPointPush Sample Data";
            this.buttonLocationPointPush.UseVisualStyleBackColor = true;
            this.buttonLocationPointPush.Click += new System.EventHandler(this.buttonLocationPointPush_Click);
            // 
            // buttonLocationPointStatus
            // 
            this.buttonLocationPointStatus.Location = new System.Drawing.Point(3, 171);
            this.buttonLocationPointStatus.Name = "buttonLocationPointStatus";
            this.buttonLocationPointStatus.Size = new System.Drawing.Size(171, 23);
            this.buttonLocationPointStatus.TabIndex = 29;
            this.buttonLocationPointStatus.Text = "LocationPointStatus Sample Data";
            this.buttonLocationPointStatus.UseVisualStyleBackColor = true;
            this.buttonLocationPointStatus.Click += new System.EventHandler(this.buttonLocationPointStatus_Click);
            // 
            // buttonRolltainerSensorStatus
            // 
            this.buttonRolltainerSensorStatus.Location = new System.Drawing.Point(3, 200);
            this.buttonRolltainerSensorStatus.Name = "buttonRolltainerSensorStatus";
            this.buttonRolltainerSensorStatus.Size = new System.Drawing.Size(171, 23);
            this.buttonRolltainerSensorStatus.TabIndex = 30;
            this.buttonRolltainerSensorStatus.Text = "RolltainerSensorStatus Sample Data";
            this.buttonRolltainerSensorStatus.UseVisualStyleBackColor = true;
            this.buttonRolltainerSensorStatus.Click += new System.EventHandler(this.buttonRolltainerSensorStatus_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(180, 56);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(113, 23);
            this.buttonClear.TabIndex = 31;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonRmsStatusSetting
            // 
            this.buttonRmsStatusSetting.Location = new System.Drawing.Point(3, 229);
            this.buttonRmsStatusSetting.Name = "buttonRmsStatusSetting";
            this.buttonRmsStatusSetting.Size = new System.Drawing.Size(171, 23);
            this.buttonRmsStatusSetting.TabIndex = 32;
            this.buttonRmsStatusSetting.Text = "RmsStatusSetting";
            this.buttonRmsStatusSetting.UseVisualStyleBackColor = true;
            this.buttonRmsStatusSetting.Click += new System.EventHandler(this.buttonRmsStatusSetting_Click);
            // 
            // UserControlRESTfulRicp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonRmsStatusSetting);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonRolltainerSensorStatus);
            this.Controls.Add(this.buttonLocationPointStatus);
            this.Controls.Add(this.buttonLocationPointPush);
            this.Controls.Add(this.buttonOutInvoiceScan);
            this.Controls.Add(this.buttonInvoiceScan);
            this.Controls.Add(this.buttonContainerScan);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonAlive);
            this.Controls.Add(this.buttonDeviceStatus);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxUrl);
            this.Name = "UserControlRESTfulRicp";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDeviceStatus;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonAlive;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonContainerScan;
        private System.Windows.Forms.Button buttonInvoiceScan;
        private System.Windows.Forms.Button buttonOutInvoiceScan;
        private System.Windows.Forms.Button buttonLocationPointPush;
        private System.Windows.Forms.Button buttonLocationPointStatus;
        private System.Windows.Forms.Button buttonRolltainerSensorStatus;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonRmsStatusSetting;
    }
}
