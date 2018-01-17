using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App_BAL;
using App_Wrapper;
using System.Web.Script.Serialization;
using CustomServerControls;
using System.Data.OleDb;

namespace BestariTerrace.Forms
{
    public partial class frmLogin : Form
    {
        public bool IsMain = false;
        public static bool IsClosed = false;
        public frmLogin()
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

                    //if(txtUserName.Text == "Admin" && txtPassword.Text == "12345")
                    if (result.message == "success")
                    {
                        this.Hide();
                        Program.IsLogined = true;
                        Program.SessionId = result.sessionid;
                        Program.OutletType = result.outlet_Type;
                        Program.Token = result.data;
                        //Form Opened Directly
                        if(!IsMain)
                        {
                            frmMain _main = new frmMain();
                            _main.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login Failed For User ?", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
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
                this.rectangleShape3.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.txtPassword.UseSystemPasswordChar = true;
                this.rectangleShape3.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //frmForgotPassword forgotpswd = new frmForgotPassword();
            //forgotpswd.ShowDialog();
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
                this.rectangleShape2.BorderColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                this.rectangleShape2.BorderColor = Color.FromArgb(225, 225, 225);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //var datetime = DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm:ss tt", new System.Globalization.CultureInfo("en-US"));
            //var datetime2 = DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm:ss tt", new System.Globalization.CultureInfo("en-CA"));
            //txtUserName.Text = "tanent_admin_1";
            //txtPassword.Text = "welcome2sw";
            //txtUserName.se
            
        }

        private void txtUserName_Click(object sender, EventArgs e)
        {
            
        }

        private void txtUserName_MouseMove(object sender, MouseEventArgs e)
        {
            txtUserName.SelectionLength = 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (IsClosed)
            {
                Application.ExitThread();
            }

            //base.OnFormClosed(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(!IsMain)
            {
                DialogResult msgResult = MessageBox.Show("Do you want to Exit Application ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (msgResult == DialogResult.No)
                {
                    e.Cancel = true;
                    IsClosed = false;
                }
                else
                {
                    Program.ClearData();
                    Application.ExitThread();
                    //if (Program.IsLogined)
                    //{
                    //    frmManagerExit mgr = new frmManagerExit();
                    //    mgr.ShowDialog();
                    //    if (mgr.IsOK)
                    //    {
                    //        Program.ClearData();
                    //        Application.ExitThread();
                    //    }
                    //}
                    //else
                    //{

                    //}
                    IsClosed = true;
                }
            }
            
            //base.OnFormClosing(e);
        }
    }
}
