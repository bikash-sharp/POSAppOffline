using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public partial class frmAssign : Form
    {
        public static int GuestCount = 0;
        public static string TableNo = string.Empty;
        public frmAssign()
        {
            InitializeComponent();
        }

        

        private void btnDineIn_Click(object sender, EventArgs e)
        {
            TableNo = txtBox1.Text.Trim();
            this.Close();
        }

        private void frmAssign_Load(object sender, EventArgs e)
        {
            txtBox2.Text = GuestCount.ToString();
            txtBox2.ReadOnly = true;
        }
    }
}
