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
    public partial class frmProductQty : Form
    {
        public int ProductId = 0;
        public frmProductQty()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void frmProductQty_Load(object sender, EventArgs e)
        {
            int ProductId = Program.SelectedProductId;
            var SelectedProduct = Program.cartItems.Where(p => p.ProductID == ProductId).FirstOrDefault();
            if(SelectedProduct != null)
            {
                lblProductName.Text = SelectedProduct.ProductName;
                txtQty.Text = SelectedProduct.Quantity.ToString();
                txtPrice.Text = SelectedProduct.Price + "";
                if(SelectedProduct.IsCounterSale)
                {
                    label2.Visible = txtPrice.Visible = true;
                }
                else
                {
                    label2.Visible = txtPrice.Visible = false;
                }
            }
        }

        private void btnUpdateQty_Click(object sender, EventArgs e)
        {
            int ProductId = Program.SelectedProductId;
            int Quantity = 0;
            double Price = 0;
            var SelectedProduct = Program.cartItems.Where(p => p.ProductID == ProductId).FirstOrDefault();
            if (SelectedProduct != null)
            {
                bool IsCounterSale = SelectedProduct.IsCounterSale;
                int.TryParse(txtQty.Text.ToString(), out Quantity);
                double.TryParse(txtPrice.Text.Trim(), out Price);
                if(Quantity <= 0)
                {
                    MessageBox.Show("Quantity cannot be less than 1 ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
                else if(IsCounterSale && Price <= 0)
                {
                    MessageBox.Show("Price cannot be less than or equal to 0 ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
                else
                {
                    SelectedProduct.Quantity = Quantity;
                    if (IsCounterSale)
                        SelectedProduct.Price = Price;
                    else
                        SelectedProduct.Price = Convert.ToDouble(SelectedProduct.OriginalPrice) * Quantity;
                    //Reset the SelectProductId
                    Program.SelectedProductId = 0;
                    Program.TotalCart();
                    this.Close();
                }
            }
            
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            int ProductId = Program.SelectedProductId;
            var SelectedProduct = Program.cartItems.Where(p => p.ProductID == ProductId).FirstOrDefault();
            if (SelectedProduct != null)
            {
                Program.cartItems.Remove(SelectedProduct);                
                //Reset the SelectProductId
                Program.SelectedProductId = 0;
            }
            Program.TotalCart();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
