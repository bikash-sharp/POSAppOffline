using App_BAL;
using App_Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public partial class frmManagerExit : Form
    {
        public bool IsOK = false;
        public frmManagerExit()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtUserName.Text.Trim()))
                {
                    this.rectangleShape2.BorderColor = Color.FromArgb(251, 51, 51);
                    this.txtUserName.Focus();
                }
                else if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    this.rectangleShape3.BorderColor = Color.FromArgb(251, 51, 51);
                    this.txtPassword.Focus();
                }
                else
                {
                    this.rectangleShape2.BorderColor = Color.FromArgb(225, 225, 225);
                    this.rectangleShape3.BorderColor = Color.FromArgb(225, 225, 225);

                    string LogoutSessionUrl = Program.BaseUrl + "/logoutSession?username=" + txtUserName.Text.Trim() + "&password=" + txtPassword.Text.Trim() + "&session_id=" + Program.SessionId + "&acess_token=" + Program.Token;
                    var GetDetails = DataProviderWrapper.Instance.GetData(LogoutSessionUrl, Verbs.GET, "");
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    MessageCL result = serializer.Deserialize<MessageCL>(GetDetails);
                    if(result.status)
                    {
                        IsOK = true;
                        this.Close();
                    }
                    else
                    {
                        IsOK = false;
                        MessageBox.Show("Wrong Username/Password ?", "Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch(Exception ex)
            {
                IsOK = false;
                MessageBox.Show("Wrong Username/Password ?", "Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }

        private void frmManagerExit_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (this.txtPassword.Text == "" && this.txtPassword.TextLength > 0)
            {
                this.txtPassword.UseSystemPasswordChar = false;
                this.txtPassword.Text = "Password";
                this.rectangleShape3.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.txtPassword.UseSystemPasswordChar = true;
                this.rectangleShape3.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (this.txtUserName.Text == "" && this.txtUserName.TextLength > 0)
            {
                this.txtUserName.Text = "User Name";
                this.rectangleShape2.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.rectangleShape2.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }
    }
}
