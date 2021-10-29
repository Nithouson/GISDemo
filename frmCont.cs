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
    public partial class frmCont : Form
    {
        public bool UseGrid;
        public int interval;
        
        public frmCont()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbxInt.Text == "")
            {
                MessageBox.Show("等值线间距不能为空！", "错误");
            }
            else
            {
                interval = int.Parse(tbxInt.Text);
                if (rbtGrid.Checked) UseGrid = true;
                else if (rbtTIN.Checked) UseGrid = false;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
