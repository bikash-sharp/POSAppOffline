using App_BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public partial class frmCounterSale : Form
    {

        public frmCounterSale()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public int GetUniqueNumber()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[4];//4 for int32
                rng.GetBytes(randomNumber);
                return BitConverter.ToInt32(randomNumber, 0);
            }
        }
        private void btnSale_Click(object sender, EventArgs e)
        {
            int Qty = 0;
            int.TryParse(txtQuantity.Text.Trim(), out Qty);
            double Price = 0;
            double.TryParse(txtPrice.Text.Trim(), out Price);

            if(Qty < 1)
            {
                MessageBox.Show("Quantity cannot be less than or equal to 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(Price <= 0)
            {
                MessageBox.Show("Price cannot be less than or equal to 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CartItemsCL cl = new CartItemsCL();
                cl.IsCounterSale = true;
                cl.ProductID = GetUniqueNumber();
                cl.Quantity = Qty;
                cl.Description = txtDescription.Text.Trim();
                cl.ProductName = txtProductName.Text.Trim();
                cl.Price = Price;                
                Program.cartItems.Add(cl);
                this.Close();
            }
        }
    }
}
