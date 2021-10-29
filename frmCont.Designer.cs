namespace GISDemo
{
    partial class frmCont
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rbtGrid = new System.Windows.Forms.RadioButton();
            this.rbtTIN = new System.Windows.Forms.RadioButton();
            this.tbxInt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbtGrid
            // 
            this.rbtGrid.AutoSize = true;
            this.rbtGrid.Checked = true;
            this.rbtGrid.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtGrid.Location = new System.Drawing.Point(33, 16);
            this.rbtGrid.Name = "rbtGrid";
            this.rbtGrid.Size = new System.Drawing.Size(103, 23);
            this.rbtGrid.TabIndex = 0;
            this.rbtGrid.TabStop = true;
            this.rbtGrid.Text = "基于格网";
            this.rbtGrid.UseVisualStyleBackColor = true;
            // 
            // rbtTIN
            // 
            this.rbtTIN.AutoSize = true;
            this.rbtTIN.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtTIN.Location = new System.Drawing.Point(189, 16);
            this.rbtTIN.Name = "rbtTIN";
            this.rbtTIN.Size = new System.Drawing.Size(95, 23);
            this.rbtTIN.TabIndex = 1;
            this.rbtTIN.TabStop = true;
            this.rbtTIN.Text = "基于TIN";
            this.rbtTIN.UseVisualStyleBackColor = true;
            // 
            // tbxInt
            // 
            this.tbxInt.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxInt.Location = new System.Drawing.Point(189, 51);
            this.tbxInt.Name = "tbxInt";
            this.tbxInt.Size = new System.Drawing.Size(101, 29);
            this.tbxInt.TabIndex = 2;
            this.tbxInt.Text = "50";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "等值线间距（m）：";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(114, 99);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 31);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmCont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 149);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxInt);
            this.Controls.Add(this.rbtTIN);
            this.Controls.Add(this.rbtGrid);
            this.Name = "frmCont";
            this.Text = "生成等值线";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtGrid;
        private System.Windows.Forms.RadioButton rbtTIN;
        private System.Windows.Forms.TextBox tbxInt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
    }
}