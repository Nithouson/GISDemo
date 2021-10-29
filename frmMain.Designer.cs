namespace GISDemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tSStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mSInterpolation = new System.Windows.Forms.ToolStripMenuItem();
            this.mSIntGen = new System.Windows.Forms.ToolStripMenuItem();
            this.mSIntDens = new System.Windows.Forms.ToolStripMenuItem();
            this.mSIntShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mSIntClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mSDelaunay = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTINGen = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTINShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mSContour = new System.Windows.Forms.ToolStripMenuItem();
            this.mSConGen = new System.Windows.Forms.ToolStripMenuItem();
            this.mSConShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mSConClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mSConSmooth = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTopo = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTopoGen = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTopoShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTopoClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mSFillPoly = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTopoTab = new System.Windows.Forms.ToolStripMenuItem();
            this.mSAuthor = new System.Windows.Forms.ToolStripMenuItem();
            this.pbxMain = new System.Windows.Forms.PictureBox();
            this.sfdTopo = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSStatus,
            this.tSPos});
            this.statusStrip1.Location = new System.Drawing.Point(0, 680);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tSStatus
            // 
            this.tSStatus.AutoSize = false;
            this.tSStatus.Name = "tSStatus";
            this.tSStatus.Size = new System.Drawing.Size(500, 17);
            // 
            // tSPos
            // 
            this.tSPos.Name = "tSPos";
            this.tSPos.Size = new System.Drawing.Size(469, 17);
            this.tSPos.Spring = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSInterpolation,
            this.mSDelaunay,
            this.mSContour,
            this.mSTopo,
            this.mSAuthor});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mSInterpolation
            // 
            this.mSInterpolation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSIntGen,
            this.mSIntDens,
            this.mSIntShow,
            this.mSIntClear});
            this.mSInterpolation.Name = "mSInterpolation";
            this.mSInterpolation.Size = new System.Drawing.Size(68, 21);
            this.mSInterpolation.Text = "空间插值";
            // 
            // mSIntGen
            // 
            this.mSIntGen.Name = "mSIntGen";
            this.mSIntGen.Size = new System.Drawing.Size(124, 22);
            this.mSIntGen.Text = "生成格网";
            this.mSIntGen.Click += new System.EventHandler(this.mSIntGen_Click);
            // 
            // mSIntDens
            // 
            this.mSIntDens.Name = "mSIntDens";
            this.mSIntDens.Size = new System.Drawing.Size(124, 22);
            this.mSIntDens.Text = "加密格网";
            this.mSIntDens.Click += new System.EventHandler(this.mSIntDens_Click);
            // 
            // mSIntShow
            // 
            this.mSIntShow.CheckOnClick = true;
            this.mSIntShow.Name = "mSIntShow";
            this.mSIntShow.Size = new System.Drawing.Size(124, 22);
            this.mSIntShow.Text = "显示格网";
            this.mSIntShow.Click += new System.EventHandler(this.mSIntShow_Click);
            // 
            // mSIntClear
            // 
            this.mSIntClear.Name = "mSIntClear";
            this.mSIntClear.Size = new System.Drawing.Size(124, 22);
            this.mSIntClear.Text = "清除格网";
            this.mSIntClear.Click += new System.EventHandler(this.mSIntClear_Click);
            // 
            // mSDelaunay
            // 
            this.mSDelaunay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSTINGen,
            this.mSTINShow});
            this.mSDelaunay.Name = "mSDelaunay";
            this.mSDelaunay.Size = new System.Drawing.Size(109, 21);
            this.mSDelaunay.Text = "Delaunay三角网";
            // 
            // mSTINGen
            // 
            this.mSTINGen.Name = "mSTINGen";
            this.mSTINGen.Size = new System.Drawing.Size(136, 22);
            this.mSTINGen.Text = "生成三角网";
            this.mSTINGen.Click += new System.EventHandler(this.mSTINGen_Click);
            // 
            // mSTINShow
            // 
            this.mSTINShow.CheckOnClick = true;
            this.mSTINShow.Name = "mSTINShow";
            this.mSTINShow.Size = new System.Drawing.Size(136, 22);
            this.mSTINShow.Text = "显示三角网";
            this.mSTINShow.Click += new System.EventHandler(this.mSTINShow_Click);
            // 
            // mSContour
            // 
            this.mSContour.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSConGen,
            this.mSConShow,
            this.mSConClear,
            this.mSConSmooth});
            this.mSContour.Name = "mSContour";
            this.mSContour.Size = new System.Drawing.Size(56, 21);
            this.mSContour.Text = "等值线";
            // 
            // mSConGen
            // 
            this.mSConGen.Name = "mSConGen";
            this.mSConGen.Size = new System.Drawing.Size(136, 22);
            this.mSConGen.Text = "生成等值线";
            this.mSConGen.Click += new System.EventHandler(this.mSConGen_Click);
            // 
            // mSConShow
            // 
            this.mSConShow.CheckOnClick = true;
            this.mSConShow.Name = "mSConShow";
            this.mSConShow.Size = new System.Drawing.Size(136, 22);
            this.mSConShow.Text = "显示等值线";
            this.mSConShow.Click += new System.EventHandler(this.mSConShow_Click);
            // 
            // mSConClear
            // 
            this.mSConClear.Name = "mSConClear";
            this.mSConClear.Size = new System.Drawing.Size(136, 22);
            this.mSConClear.Text = "清除等值线";
            this.mSConClear.Click += new System.EventHandler(this.mSConClear_Click);
            // 
            // mSConSmooth
            // 
            this.mSConSmooth.CheckOnClick = true;
            this.mSConSmooth.Name = "mSConSmooth";
            this.mSConSmooth.Size = new System.Drawing.Size(136, 22);
            this.mSConSmooth.Text = "等值线光滑";
            this.mSConSmooth.Click += new System.EventHandler(this.mSConSmooth_Click);
            // 
            // mSTopo
            // 
            this.mSTopo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSTopoGen,
            this.mSTopoShow,
            this.mSTopoClear,
            this.mSFillPoly,
            this.mSTopoTab});
            this.mSTopo.Name = "mSTopo";
            this.mSTopo.Size = new System.Drawing.Size(68, 21);
            this.mSTopo.Text = "拓扑生成";
            // 
            // mSTopoGen
            // 
            this.mSTopoGen.Name = "mSTopoGen";
            this.mSTopoGen.Size = new System.Drawing.Size(180, 22);
            this.mSTopoGen.Text = "拓扑自动生成";
            this.mSTopoGen.Click += new System.EventHandler(this.mSTopoGen_Click);
            // 
            // mSTopoShow
            // 
            this.mSTopoShow.Name = "mSTopoShow";
            this.mSTopoShow.Size = new System.Drawing.Size(180, 22);
            this.mSTopoShow.Text = "显示多边形";
            this.mSTopoShow.Click += new System.EventHandler(this.mSTopoShow_Click);
            // 
            // mSTopoClear
            // 
            this.mSTopoClear.Name = "mSTopoClear";
            this.mSTopoClear.Size = new System.Drawing.Size(180, 22);
            this.mSTopoClear.Text = "清除多边形";
            this.mSTopoClear.Click += new System.EventHandler(this.mSTopoClear_Click);
            // 
            // mSFillPoly
            // 
            this.mSFillPoly.Name = "mSFillPoly";
            this.mSFillPoly.Size = new System.Drawing.Size(180, 22);
            this.mSFillPoly.Text = "多边形填充";
            this.mSFillPoly.Click += new System.EventHandler(this.mSFillPoly_Click);
            // 
            // mSTopoTab
            // 
            this.mSTopoTab.Name = "mSTopoTab";
            this.mSTopoTab.Size = new System.Drawing.Size(180, 22);
            this.mSTopoTab.Text = "拓扑关系表";
            this.mSTopoTab.Click += new System.EventHandler(this.mSTopoTab_Click);
            // 
            // mSAuthor
            // 
            this.mSAuthor.Name = "mSAuthor";
            this.mSAuthor.Size = new System.Drawing.Size(44, 21);
            this.mSAuthor.Text = "关于";
            this.mSAuthor.Click += new System.EventHandler(this.mSAuthor_Click);
            // 
            // pbxMain
            // 
            this.pbxMain.BackColor = System.Drawing.Color.White;
            this.pbxMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxMain.Location = new System.Drawing.Point(0, 25);
            this.pbxMain.Name = "pbxMain";
            this.pbxMain.Size = new System.Drawing.Size(984, 655);
            this.pbxMain.TabIndex = 2;
            this.pbxMain.TabStop = false;
            this.pbxMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxMain_Paint);
            this.pbxMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbxMain_MouseClick);
            this.pbxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbxMain_MouseMove);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 702);
            this.Controls.Add(this.pbxMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "GISDemo";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tSStatus;
        private System.Windows.Forms.ToolStripStatusLabel tSPos;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mSInterpolation;
        private System.Windows.Forms.ToolStripMenuItem mSIntGen;
        private System.Windows.Forms.ToolStripMenuItem mSIntDens;
        private System.Windows.Forms.ToolStripMenuItem mSIntShow;
        private System.Windows.Forms.ToolStripMenuItem mSDelaunay;
        private System.Windows.Forms.ToolStripMenuItem mSContour;
        private System.Windows.Forms.ToolStripMenuItem mSConGen;
        private System.Windows.Forms.ToolStripMenuItem mSConShow;
        private System.Windows.Forms.ToolStripMenuItem mSTopo;
        private System.Windows.Forms.ToolStripMenuItem mSTopoGen;
        private System.Windows.Forms.ToolStripMenuItem mSFillPoly;
        private System.Windows.Forms.ToolStripMenuItem mSTopoTab;
        private System.Windows.Forms.ToolStripMenuItem mSAuthor;
        private System.Windows.Forms.PictureBox pbxMain;
        private System.Windows.Forms.ToolStripMenuItem mSIntClear;
        private System.Windows.Forms.ToolStripMenuItem mSTINGen;
        private System.Windows.Forms.ToolStripMenuItem mSTINShow;
        private System.Windows.Forms.ToolStripMenuItem mSConClear;
        private System.Windows.Forms.ToolStripMenuItem mSConSmooth;
        private System.Windows.Forms.ToolStripMenuItem mSTopoShow;
        private System.Windows.Forms.ToolStripMenuItem mSTopoClear;
        private System.Windows.Forms.SaveFileDialog sfdTopo;
    }
}

