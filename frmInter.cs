using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDemo
{
    public partial class frmInter : Form
    {
        public bool UseIDW;
        public double res;
        
        public frmInter()
        {
            InitializeComponent();
        }

        private void btnInter_Click(object sender, EventArgs e)
        {
            if (tbxRes.Text == "")
            {
                MessageBox.Show("分辨率不能为空！", "错误");
            }
            else
            {
                res = double.Parse(tbxRes.Text);
                if (rdBIDW.Checked) UseIDW = true;
                else if (rdBDirec.Checked) UseIDW = false;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
