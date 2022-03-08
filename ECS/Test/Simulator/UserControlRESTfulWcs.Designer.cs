
namespace Simulator
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
            this.buttonOrder = new System.Windows.Forms.Button();
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonAlive = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonOrderCancel = new System.Windows.Forms.Button();
            this.buttonOrderDelete = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonOperatorWeightResult = new System.Windows.Forms.Button();
            this.buttonSkuMaster = new System.Windows.Forms.Button();
            this.buttonPickingResultsImport = new System.Windows.Forms.Button();
            this.buttonTestOrder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOrder
            // 
            this.buttonOrder.Location = new System.Drawing.Point(10, 36);
            this.buttonOrder.Name = "buttonOrder";
            this.buttonOrder.Size = new System.Drawing.Size(171, 23);
            this.buttonOrder.TabIndex = 9;
            this.buttonOrder.Text = "Order Sample Data";
            this.buttonOrder.UseVisualStyleBackColor = true;
            this.buttonOrder.Click += new System.EventHandler(this.buttonOrder_Click);
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
            this.textBoxUrl.Text = "http://127.0.0.1:8080";
            // 
            // buttonAlive
            // 
            this.buttonAlive.Location = new System.Drawing.Point(188, 36);
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
            // buttonOrderCancel
            // 
            this.buttonOrderCancel.Location = new System.Drawing.Point(10, 65);
            this.buttonOrderCancel.Name = "buttonOrderCancel";
            this.buttonOrderCancel.Size = new System.Drawing.Size(171, 23);
            this.buttonOrderCancel.TabIndex = 25;
            this.buttonOrderCancel.Text = "OrderCancel Sample Data";
            this.buttonOrderCancel.UseVisualStyleBackColor = true;
            this.buttonOrderCancel.Click += new System.EventHandler(this.buttonOrderCancel_Click);
            // 
            // buttonOrderDelete
            // 
            this.buttonOrderDelete.Location = new System.Drawing.Point(10, 94);
            this.buttonOrderDelete.Name = "buttonOrderDelete";
            this.buttonOrderDelete.Size = new System.Drawing.Size(171, 23);
            this.buttonOrderDelete.TabIndex = 26;
            this.buttonOrderDelete.Text = "OrderDelete Sample Data";
            this.buttonOrderDelete.UseVisualStyleBackColor = true;
            this.buttonOrderDelete.Click += new System.EventHandler(this.buttonOrderDelete_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(187, 65);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(113, 23);
            this.buttonClear.TabIndex = 27;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonOperatorWeightResult
            // 
            this.buttonOperatorWeightResult.Location = new System.Drawing.Point(10, 123);
            this.buttonOperatorWeightResult.Name = "buttonOperatorWeightResult";
            this.buttonOperatorWeightResult.Size = new System.Drawing.Size(171, 23);
            this.buttonOperatorWeightResult.TabIndex = 28;
            this.buttonOperatorWeightResult.Text = "OperatorWeightResult Sample Data";
            this.buttonOperatorWeightResult.UseVisualStyleBackColor = true;
            this.buttonOperatorWeightResult.Click += new System.EventHandler(this.buttonOperatorWeightResult_Click);
            // 
            // buttonSkuMaster
            // 
            this.buttonSkuMaster.Location = new System.Drawing.Point(10, 152);
            this.buttonSkuMaster.Name = "buttonSkuMaster";
            this.buttonSkuMaster.Size = new System.Drawing.Size(171, 23);
            this.buttonSkuMaster.TabIndex = 29;
            this.buttonSkuMaster.Text = "SkuMaster Sample Data";
            this.buttonSkuMaster.UseVisualStyleBackColor = true;
            this.buttonSkuMaster.Click += new System.EventHandler(this.buttonSkuMaster_Click);
            // 
            // buttonPickingResultsImport
            // 
            this.buttonPickingResultsImport.Location = new System.Drawing.Point(10, 177);
            this.buttonPickingResultsImport.Name = "buttonPickingResultsImport";
            this.buttonPickingResultsImport.Size = new System.Drawing.Size(171, 23);
            this.buttonPickingResultsImport.TabIndex = 30;
            this.buttonPickingResultsImport.Text = "PickingResultsImport";
            this.buttonPickingResultsImport.UseVisualStyleBackColor = true;
            this.buttonPickingResultsImport.Click += new System.EventHandler(this.buttonPickingResultsImport_Click);
            // 
            // buttonTestOrder
            // 
            this.buttonTestOrder.Location = new System.Drawing.Point(10, 265);
            this.buttonTestOrder.Name = "buttonTestOrder";
            this.buttonTestOrder.Size = new System.Drawing.Size(171, 23);
            this.buttonTestOrder.TabIndex = 31;
            this.buttonTestOrder.Text = "Test Order";
            this.buttonTestOrder.UseVisualStyleBackColor = true;
            this.buttonTestOrder.Click += new System.EventHandler(this.buttonTestOrder_Click);
            // 
            // UserControlRESTfulWcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonTestOrder);
            this.Controls.Add(this.buttonPickingResultsImport);
            this.Controls.Add(this.buttonSkuMaster);
            this.Controls.Add(this.buttonOperatorWeightResult);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonOrderDelete);
            this.Controls.Add(this.buttonOrderCancel);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonAlive);
            this.Controls.Add(this.buttonOrder);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxUrl);
            this.Name = "UserControlRESTfulWcs";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOrder;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonAlive;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonOrderCancel;
        private System.Windows.Forms.Button buttonOrderDelete;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonOperatorWeightResult;
        private System.Windows.Forms.Button buttonSkuMaster;
        private System.Windows.Forms.Button buttonPickingResultsImport;
        private System.Windows.Forms.Button buttonTestOrder;
    }
}
