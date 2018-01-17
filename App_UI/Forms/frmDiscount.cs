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
    public partial class frmDiscount : Form
    {
        public double OriginalAmt = 0;
        public double DiscountAmt = 0;
        public EmDiscountType DiscountType;
        public frmDiscount()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Double _discountAmt = 0;
            Double.TryParse(txtAmt.Text.Trim(), out _discountAmt);
            Double PercentAmt = 0;
            Double.TryParse(txtPercent.Text.Trim(), out PercentAmt);
            //Both Are Null
            if (String.IsNullOrEmpty(txtAmt.Text.Trim()) && String.IsNullOrEmpty(txtPercent.Text.Trim()))
            {
                MessageBox.Show("Both Amount/Percent are Empty!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmt.Focus();
            }
            //Both are not Null
            else if (!String.IsNullOrEmpty(txtAmt.Text.Trim()) && !String.IsNullOrEmpty(txtPercent.Text.Trim()))
            {
                MessageBox.Show("Apply Either Amount or Percent!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmt.Text = txtPercent.Text = String.Empty;
            }
            else if(!String.IsNullOrEmpty(txtAmt.Text.Trim()))
            {
                
                if(_discountAmt < 0)
                {
                    //Show Message for Minus values
                    MessageBox.Show("Amount cannot be Less than or equal to 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAmt.Text = "";
                    txtAmt.Focus();
                }
                else if(_discountAmt > OriginalAmt)
                {
                    //Show Message for Minus values
                    MessageBox.Show("Discount Amount cannot be greater than or equal to original Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAmt.Text = "";
                    txtAmt.Focus();
                }
                else 
                {
                    DiscountType = EmDiscountType.Amount;
                    DiscountAmt = _discountAmt;
                    this.Close();
                }
            }
            else if(!String.IsNullOrEmpty(txtPercent.Text.Trim()))
            {

                
                if (PercentAmt < 0)
                {
                    //Show Message for Minus Percentage
                    MessageBox.Show("Percent cannot be Less than or equal to 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPercent.Text = "0.00";
                    txtPercent.Focus();
                }
                else if(PercentAmt >= 100)
                {
                    MessageBox.Show("100% Discount cannot be applied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPercent.Text = "0.00";
                    txtPercent.Focus();
                }
                else
                {
                    DiscountType = EmDiscountType.Percent;
                    DiscountAmt = PercentAmt;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDiscount_Load(object sender, EventArgs e)
        {
            if((DiscountType == EmDiscountType.Amount || DiscountType == EmDiscountType.Percent) && DiscountAmt == 0)
            {
                txtAmt.Text = txtPercent.Text = String.Empty;
                txtAmt.Focus();
            }
            else if(DiscountType == EmDiscountType.Amount)
            {
                txtAmt.Text = DiscountAmt.ToString();
                txtAmt.Focus();
            }
            else if(DiscountType == EmDiscountType.Percent)
            {
                txtPercent.Text = DiscountAmt.ToString();
                txtPercent.Focus();
            }
            else
            {
                txtAmt.Focus();
            }
        }
    }
}
