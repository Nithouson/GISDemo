namespace GISDemo
{
    partial class frmInter
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
            this.rdBIDW = new System.Windows.Forms.RadioButton();
            this.rdBDirec = new System.Windows.Forms.RadioButton();
            this.tbxRes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdBIDW
            // 
            this.rdBIDW.AutoSize = true;
            this.rdBIDW.Checked = true;
            this.rdBIDW.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdBIDW.Location = new System.Drawing.Point(17, 26);
            this.rdBIDW.Name = "rdBIDW";
            this.rdBIDW.Size = new System.Drawing.Size(141, 23);
            this.rdBIDW.TabIndex = 0;
            this.rdBIDW.TabStop = true;
            this.rdBIDW.Text = "距离倒数权重";
            this.rdBIDW.UseVisualStyleBackColor = true;
            // 
            // rdBDirec
            // 
            this.rdBDirec.AutoSize = true;
            this.rdBDirec.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdBDirec.Location = new System.Drawing.Point(164, 26);
            this.rdBDirec.Name = "rdBDirec";
            this.rdBDirec.Size = new System.Drawing.Size(141, 23);
            this.rdBDirec.TabIndex = 1;
            this.rdBDirec.Text = "方位取点加权";
            this.rdBDirec.UseVisualStyleBackColor = true;
            // 
            // tbxRes
            // 
            this.tbxRes.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxRes.Location = new System.Drawing.Point(164, 66);
            this.tbxRes.Name = "tbxRes";
            this.tbxRes.Size = new System.Drawing.Size(141, 29);
            this.tbxRes.TabIndex = 2;
            this.tbxRes.Text = "350";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(27, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "分辨率（m)：";
            // 
            // btnInter
            // 
            this.btnInter.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInter.Location = new System.Drawing.Point(121, 120);
            this.btnInter.Name = "btnInter";
            this.btnInter.Size = new System.Drawing.Size(94, 30);
            this.btnInter.TabIndex = 4;
            this.btnInter.Text = "确定";
            this.btnInter.UseVisualStyleBackColor = true;
            this.btnInter.Click += new System.EventHandler(this.btnInter_Click);
            // 
            // frmInter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 162);
            this.Controls.Add(this.btnInter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxRes);
            this.Controls.Add(this.rdBDirec);
            this.Controls.Add(this.rdBIDW);
            this.Name = "frmInter";
            this.Text = "frmInter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdBIDW;
        private System.Windows.Forms.RadioButton rdBDirec;
        private System.Windows.Forms.TextBox tbxRes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInter;
    }
}