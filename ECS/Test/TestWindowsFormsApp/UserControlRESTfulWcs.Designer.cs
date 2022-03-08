
namespace TestWindowsFormsApp
{
    partial class UserControlRESTfulWcs
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
            this.buttonboxIDSampleData = new System.Windows.Forms.Button();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonAlive = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonWeight = new System.Windows.Forms.Button();
            this.buttonInvoice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonboxIDSampleData
            // 
            this.buttonboxIDSampleData.Location = new System.Drawing.Point(3, 30);
            this.buttonboxIDSampleData.Name = "buttonboxIDSampleData";
            this.buttonboxIDSampleData.Size = new System.Drawing.Size(171, 23);
            this.buttonboxIDSampleData.TabIndex = 7;
            this.buttonboxIDSampleData.Text = "boxID Sample Data";
            this.buttonboxIDSampleData.UseVisualStyleBackColor = true;
            this.buttonboxIDSampleData.Click += new System.EventHandler(this.buttonboxIDSampleData_Click);
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(5, 6);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(20, 12);
            this.labelUrl.TabIndex = 6;
            this.labelUrl.Text = "Url";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(31, 3);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(165, 21);
            this.textBoxUrl.TabIndex = 5;
            this.textBoxUrl.Text = "http://128.11.3.65";
            // 
            // buttonAlive
            // 
            this.buttonAlive.Location = new System.Drawing.Point(194, 30);
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
            this.richTextBoxLog.TabIndex = 21;
            this.richTextBoxLog.Text = "";
            // 
            // buttonWeight
            // 
            this.buttonWeight.Location = new System.Drawing.Point(3, 59);
            this.buttonWeight.Name = "buttonWeight";
            this.buttonWeight.Size = new System.Drawing.Size(171, 23);
            this.buttonWeight.TabIndex = 24;
            this.buttonWeight.Text = "Weight Sample Data";
            this.buttonWeight.UseVisualStyleBackColor = true;
            this.buttonWeight.Click += new System.EventHandler(this.buttonWeight_Click);
            // 
            // buttonInvoice
            // 
            this.buttonInvoice.Location = new System.Drawing.Point(3, 88);
            this.buttonInvoice.Name = "buttonInvoice";
            this.buttonInvoice.Size = new System.Drawing.Size(171, 23);
            this.buttonInvoice.TabIndex = 25;
            this.buttonInvoice.Text = "Invoice Sample Data";
            this.buttonInvoice.UseVisualStyleBackColor = true;
            this.buttonInvoice.Click += new System.EventHandler(this.buttonInvoice_Click);
            // 
            // UserControlRESTfulWcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonInvoice);
            this.Controls.Add(this.buttonWeight);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonAlive);
            this.Controls.Add(this.buttonboxIDSampleData);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxUrl);
            this.Name = "UserControlRESTfulWcs";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonboxIDSampleData;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonAlive;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonWeight;
        private System.Windows.Forms.Button buttonInvoice;
    }
}
