
namespace Simulator
{
    partial class UserControlInkjet
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelInk = new System.Windows.Forms.Label();
            this.numericUpDownInk = new System.Windows.Forms.NumericUpDown();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxCurrentId = new System.Windows.Forms.TextBox();
            this.buttonPrintComplete = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInk)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelInk);
            this.groupBox1.Controls.Add(this.numericUpDownInk);
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.textBoxCurrentId);
            this.groupBox1.Controls.Add(this.buttonPrintComplete);
            this.groupBox1.Controls.Add(this.richTextBoxLog);
            this.groupBox1.Controls.Add(this.propertyGrid1);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(731, 327);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox";
            // 
            // labelInk
            // 
            this.labelInk.AutoSize = true;
            this.labelInk.Location = new System.Drawing.Point(6, 288);
            this.labelInk.Name = "labelInk";
            this.labelInk.Size = new System.Drawing.Size(41, 12);
            this.labelInk.TabIndex = 28;
            this.labelInk.Text = "Ink(%)";
            // 
            // numericUpDownInk
            // 
            this.numericUpDownInk.Location = new System.Drawing.Point(56, 286);
            this.numericUpDownInk.Name = "numericUpDownInk";
            this.numericUpDownInk.Size = new System.Drawing.Size(106, 21);
            this.numericUpDownInk.TabIndex = 27;
            this.numericUpDownInk.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownInk.ValueChanged += new System.EventHandler(this.numericUpDownInk_ValueChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(222, 201);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 26;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBoxCurrentId
            // 
            this.textBoxCurrentId.Location = new System.Drawing.Point(6, 259);
            this.textBoxCurrentId.Name = "textBoxCurrentId";
            this.textBoxCurrentId.Size = new System.Drawing.Size(141, 21);
            this.textBoxCurrentId.TabIndex = 25;
            this.textBoxCurrentId.Text = "0";
            this.textBoxCurrentId.TextChanged += new System.EventHandler(this.textBoxCurrentId_TextChanged);
            // 
            // buttonPrintComplete
            // 
            this.buttonPrintComplete.Location = new System.Drawing.Point(6, 230);
            this.buttonPrintComplete.Name = "buttonPrintComplete";
            this.buttonPrintComplete.Size = new System.Drawing.Size(116, 23);
            this.buttonPrintComplete.TabIndex = 24;
            this.buttonPrintComplete.Text = "PrintComplete";
            this.buttonPrintComplete.UseVisualStyleBackColor = true;
            this.buttonPrintComplete.Click += new System.EventHandler(this.buttonPrintComplete_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxLog.Location = new System.Drawing.Point(303, 17);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(425, 307);
            this.richTextBoxLog.TabIndex = 23;
            this.richTextBoxLog.Text = "";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(6, 20);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(291, 175);
            this.propertyGrid1.TabIndex = 22;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(87, 201);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 20;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(6, 201);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 19;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // UserControlInkjet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "UserControlInkjet";
            this.Size = new System.Drawing.Size(731, 327);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonPrintComplete;
        private System.Windows.Forms.TextBox textBoxCurrentId;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label labelInk;
        private System.Windows.Forms.NumericUpDown numericUpDownInk;
    }
}
