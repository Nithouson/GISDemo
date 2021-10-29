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
    public partial class frmGridDense : Form
    {
        public int cut;

        public frmGridDense()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tbxCut.Text == "")
            {
                MessageBox.Show("等分数不能为空！", "错误");
            }
            else
            {
                cut = int.Parse(tbxCut.Text);
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
