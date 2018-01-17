using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App_BAL;
using App_Wrapper;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using BestariTerrace.Forms;

namespace BestariTerrace.UserControls
{
    public partial class ucPayment : UserControl
    {
        public double TotalAmount = 0;
        public double RemAmount = 0;
        public EmPaymentType PayementType = EmPaymentType.Cash;
        public string LastTransactionID = string.Empty;
        public bool IsAmount = false;
        public bool IsDiscount = false;
        public double DiscountAmt = 0;
        public EmDiscountType DiscountType;
        public ucPayment()
        {
            InitializeComponent();
            this.txtAmount.Focus();
        }

        public void BindData(double _Amount)
        {
            TotalAmount = _Amount;
            lblTotal.Text = _Amount.ToString("N");
            RemAmount = 0 - _Amount;
            lblRem.Text = "Balance : " + RemAmount.ToString("N");
            ChangeColorSelectedButton(btnCash);
            txtAmount.Focus();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (PayementType == EmPaymentType.Cash)
            {
                double PaidAmount = 0;
                double.TryParse(txtAmount.Text, out PaidAmount);
                RemAmount = PaidAmount - TotalAmount;
                lblRem.Text = "Balance : " + RemAmount.ToString("N");
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            bool IsDone = false;
            if (PayementType == EmPaymentType.Cash)
            {
                if (RemAmount < 0)
                {
                    MessageBox.Show("Payment not complete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    IsDone = true;
                }
            }
            else if (PayementType == EmPaymentType.Wallet)
            {
                if (txtAmount.Text == "" || String.IsNullOrEmpty(txtAmount.Text) == true)
                {
                    MessageBox.Show("Please enter the Transaction Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Verify the transaction Id here
                    string URL = Program.BaseUrl;
                    string _transID = txtAmount.Text.Trim();
                    string TransactionStatusURL = URL + "/orderdetailusingtransaction?tn_id=" + _transID + "&acess_token=" + Program.Token;

                    var GetStatus = DataProviderWrapper.Instance.GetData(TransactionStatusURL, Verbs.GET, "");
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var result = serializer.Deserialize<TransactionAPICL>(GetStatus);
                    if (result.status)
                    {
                        IsDone = true;
                        LastTransactionID = _transID;
                    }
                    else
                    {
                        IsDone = false;
                        MessageBox.Show("The transaction not successful or enter a valid transaction id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (PayementType == EmPaymentType.Card)
            {
                if (txtAmount.Text == "" || String.IsNullOrEmpty(txtAmount.Text) == true)
                {
                    MessageBox.Show("Please enter the Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Verify the code here
                    IsDone = true;
                }
            }

            if (IsDone)
            {
                if (this.ParentForm.Modal)
                {
                    this.ParentForm.DialogResult = DialogResult.OK;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.ParentForm.Modal)
            {
                this.ParentForm.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnExactAmount_Click(object sender, EventArgs e)
        {

        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            ChangeColorSelectedButton(btnCash);
            txtAmount.Text = "";
            txtAmount.PlaceholderText = "Cash Amount";
            txtAmount.Focus();
            PayementType = EmPaymentType.Cash;
            pnlCash.BringToFront();
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            ChangeColorSelectedButton(btnCard);
            txtAmount.Text = "";
            txtAmount.PlaceholderText = "Code";
            txtAmount.Focus();
            //txtAmount.Text = TotalAmount.ToString("N");
            PayementType = EmPaymentType.Card;
            pnlCash.BringToFront();
        }

        public void ChangeColorSelectedButton(Button btn)
        {
            if (btn != null)
            {
                String Name = btn.Name;
                btnCash.BackColor = btnCard.BackColor = btnWallet.BackColor = Color.White;
                btnCash.FlatAppearance.MouseOverBackColor = btnCard.FlatAppearance.MouseOverBackColor = btnWallet.FlatAppearance.MouseOverBackColor = Color.White;
                rectBtnCard.BackColor = Color.White;
                rectbtnCash.BackColor = Color.White;
                rectBtnWallet.BackColor = Color.White;
                switch (Name)
                {
                    case "btnCard":
                        rectBtnCard.BackColor = Color.FromArgb(93, 213, 93);
                        btn.BackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(93, 213, 93);
                        break;
                    case "btnCash":
                        rectbtnCash.BackColor = Color.FromArgb(93, 213, 93);
                        btn.BackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(93, 213, 93);
                        break;
                    case "btnWallet":
                        rectBtnWallet.BackColor = Color.FromArgb(93, 213, 93);
                        btn.BackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(93, 213, 93);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(93, 213, 93);
                        break;
                    default:
                        btnCash.BackColor = Color.FromArgb(93, 213, 93);

                        rectBtnCard.BackColor = Color.FromArgb(93, 213, 93);
                        break;
                }

            }
            txtAmount.Focus();
        }

        private void btnWallet_Click(object sender, EventArgs e)
        {
            ChangeColorSelectedButton(btnWallet); txtAmount.Text = "";
            txtAmount.PlaceholderText = "Transaction Number";
            txtAmount.Focus();
            PayementType = EmPaymentType.Wallet;
            pnlCash.BringToFront();
        }

        private void ucPayment_Load(object sender, EventArgs e)
        {
            this.txtAmount.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            decimal Amount = 0;
            Button btn = sender as Button;
            if (btn.Text == "clear")
            {
                txtAmount.Clear();
            }
            else if (btn.Text == ".")
            {
                if (txtAmount.Text.Length > 0)
                {
                    if (!txtAmount.Text.Contains("."))
                    {
                        txtAmount.Text += btn.Text;
                    }
                }
                else
                {
                    txtAmount.Text = "0" + btn.Text;
                }
                txtAmount_TextChanged(txtAmount, null);

            }
            else if (btn.Text == "<-")
            {
                if (!String.IsNullOrEmpty(txtAmount.Text))
                {
                    string val = txtAmount.Text.TrimEnd().Substring(0, txtAmount.Text.Length - 1);
                    txtAmount.Text = val;
                    txtAmount_TextChanged(txtAmount, null);
                }
            }
            else
            {
                decimal.TryParse(txtAmount.Text, out Amount);

                if (txtAmount.Text == "0" && btn.Text == "0" && Amount == 0)
                {
                    txtAmount.Text = btn.Text;
                }
                else
                {
                    if (txtAmount.Text.Contains("."))
                    {
                        decimal.TryParse(txtAmount.Text, out Amount);
                        var newAmount = Amount.ToString("N2");

                        if (newAmount.Split('.')[1] == "00")
                            txtAmount.Text = newAmount.Substring(0, newAmount.Length - 2);
                        else
                            txtAmount.Text = newAmount.Substring(0, newAmount.Length - 1);
                    }
                    txtAmount.Text += btn.Text;
                }
                txtAmount_TextChanged(txtAmount, null);
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount();
            frm.OriginalAmt = Program.cartItems.Sum(p => p.Price); 
            if (IsDiscount)
            {
                frm.DiscountType = DiscountType;
                frm.DiscountAmt = DiscountAmt;
            }
            frm.ShowDialog();
            DiscountType = frm.DiscountType;
            DiscountAmt = frm.DiscountAmt;
            if(DiscountAmt > 0)
            {
                IsDiscount = true;
                if (DiscountType == EmDiscountType.Amount)
                {
                    var _FinalAmt = frm.OriginalAmt - DiscountAmt;
                    BindData(_FinalAmt);
                    //RemAmount = RemAmount - DiscountAmt;

                }
                else if(DiscountType == EmDiscountType.Percent)
                {
                    var _FinalAmt =frm.OriginalAmt - ((frm.OriginalAmt * DiscountAmt) / 100);
                    BindData(_FinalAmt);
                }
                //BindData(RemAmount);
                txtAmount_TextChanged(txtAmount, null);
            }

        }
    }
}
