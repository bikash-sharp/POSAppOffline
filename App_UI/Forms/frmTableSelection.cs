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
    public partial class frmTableSelection : Form
    {
        public string tableSelection;
        public string tableRemarks;
        public EmOrderType orderType;
        public frmTableSelection()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(orderType == EmOrderType.DineIn)
            {
                if(String.IsNullOrEmpty(txtTableNo.Text))
                {
                    MessageBox.Show("Please enter the table number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    tableRemarks = txtDescription.Text.Trim();
                    tableSelection = txtTableNo.Text.Trim();
                    this.Close();
                }
                //Further Process
            }
            else
            {
                tableRemarks = txtDescription.Text.Trim();
                tableSelection = txtTableNo.Text.Trim();
                this.Close();
            }
        }

        private void frmTableSelection_Load(object sender, EventArgs e)
        {
            if(orderType == EmOrderType.TakeOut)
            {
                //txtTableNo.Text = "TakeAway";
                //txtTableNo.Enabled = false;
                label4.Visible = false;
                txtTableNo.Visible = false;
            }
            else
            {
                txtTableNo.Text = string.Empty;
                txtTableNo.Enabled = true;
            }
        }
    }
}
