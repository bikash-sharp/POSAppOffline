using App_BAL;
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
    public partial class frmOrderType : Form
    {
        public static bool isClosed = true;
        public frmOrderType()
        {
            InitializeComponent();
        }

        private void frmOrderType_Load(object sender, EventArgs e)
        {
            //frmMain.CurrentOrderType = EmOrderType.DineIn;
        }

        private void btnDineIn_Click(object sender, EventArgs e)
        {
            isClosed = false;
            frmMain.CurrentOrderType = EmOrderType.DineIn;
            this.Close();
        }

        private void btnTakeAway_Click(object sender, EventArgs e)
        {
            isClosed = false;
            frmMain.CurrentOrderType = EmOrderType.TakeOut;
            this.Close();
        }
    }
}
