using App_BAL;
using App_Wrapper;
using CustomServerControls;
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
    public partial class frmLogout : Form
    {
        public frmLogout()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                this.rectangleShape1.BorderColor = Color.FromArgb(251, 51, 51);
                this.txtUserName.Focus();
            }
            else if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                this.rectangleShape2.BorderColor = Color.FromArgb(251, 51, 51);
                this.txtPassword.Focus();
            }
            else
            {
                this.rectangleShape1.BorderColor = Color.FromArgb(225, 225, 225);
                this.rectangleShape2.BorderColor = Color.FromArgb(225, 225, 225);
                try
                {
                    //var rowCount = 0;
                    //using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                    //{
                    //    OConn.Open();
                    //    OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM employees", OConn);
                    //    int.TryParse(cmd.ExecuteScalar()+"", out rowCount);
                    //    //OleDbCommand cmd = new OleDbCommand("SELECT password FROM employees WHERE user = '" + txtUserName.Text.Trim() + "'", OConn);
                    //}
                    //if(rowCount > 0)
                    //{
                    //    //try Login
                    //}
                    //else
                    //{
                    //    //Show Message for Sync.
                    //    //Sync all api altogether.
                    //}

                    string URL = Program.BaseUrl;
                    string LoginUrl = URL + "/login?username=" + txtUserName.Text.Trim() + "&password=" + txtPassword.Text.Trim();

                    var GetLoginDetails = DataProviderWrapper.Instance.GetData(LoginUrl, Verbs.GET, "");
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    LoginCL result = serializer.Deserialize<LoginCL>(GetLoginDetails);

                    if (result.message == "success")
                    {
                        this.Hide();
                        Program.IsLogined = true;
                        Program.SessionId = result.sessionid;
                        Program.OutletType = result.outlet_Type;
                        Program.Token = result.data;                        
                    }
                    else
                    {
                        Program.IsLogined = false;
                        MessageBox.Show("Login Failed For User ?", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Program.IsLogined = false;
                    var err = ex.Message;
                    MessageBox.Show(ex.Message, "Error Occurred", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (this.txtPassword.Text == "" && this.txtPassword.TextLength > 0)
            {
                this.txtPassword.UseSystemPasswordChar = false;
                this.txtPassword.Text = "Password";
                this.rectangleShape2.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.txtPassword.UseSystemPasswordChar = true;
                this.rectangleShape2.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            var txt = ((TxtBox)sender).Text;

            if (this.txtUserName.SelectionLength > 1)
            {
                this.txtUserName.Text = "";
            }

            this.txtUserName.Text = txt;

            if (this.txtUserName.Text == "" && this.txtUserName.TextLength > 0)
            {
                this.txtUserName.Text = "User Name";
                this.rectangleShape1.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.rectangleShape1.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }
    }
}
