﻿using System;
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
using System.Globalization;

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
                    var rowCount = 0;
                    using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                    {
                        OConn.Open();
                        OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM employees", OConn);
                        int.TryParse(cmd.ExecuteScalar() + "", out rowCount);

                    }
                    if (rowCount > 0)
                    {
                        //try Login
                        string dbPassword = string.Empty;
                        DataSet ds = new DataSet();

                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                        {
                            OConn.Open();
                            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM employees", OConn);
                            da.Fill(ds);
                        }
                        //Get Data From DB
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            //Fetch Details 
                            if (dt.Rows.Count > 0)
                            {
                                for(int i=0;i< dt.Rows.Count;i++)
                                {
                                    App_Off_BAL.EmployeeCL emp = new App_Off_BAL.EmployeeCL();

                                    DataRow dtRow = dt.Rows[i];
                                    emp.id = dtRow["server_id"] + "";
                                    emp.username = dtRow["username"] + "";
                                    emp.password = dtRow["password"] + "";
                                    emp.position = dtRow["position"] + "";
                                    emp.restaurant_id = dtRow["restaurant_id"] + "";
                                    emp.tanent_id = dtRow["tanent_id"] + "";
                                    emp.access_type = dtRow["access_type"] + "";
                                    emp.acess_token = dtRow["access_token"] + "";
                                    emp.created = dtRow["created"] + "";
                                    emp.daily_cash_limit = dtRow["daily_cash_limit"] + "";
                                    emp.employee_name = dtRow["employee_name"] + "";
                                    //Add Into Global Employee List
                                    if (!Program.Employees.Where(p => p.id == emp.id).Any())
                                        Program.Employees.Add(emp);
                                }

                                var _EmpExist = Program.Employees.Where(p => p.username == txtUserName.Text.Trim()).FirstOrDefault();

                                //Exist or not
                                if (_EmpExist != null)
                                {
                                    if (_EmpExist.password != txtPassword.Text.Trim())
                                    {
                                        MessageBox.Show("Password do not match ?", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (_EmpExist.password == txtPassword.Text.Trim())
                                    {
                                        //Save the Current Employee Id
                                        Program.CurrentEmployeeID = _EmpExist.id;
                                        
                                        Program.IsLogined = true;

                                        //Create EmployeeSession
                                        Program.SessionId = _EmpExist.tanent_id + "-" + DateTime.UtcNow.Date.ToString("ddMMyyyy");
                                        //Check for Today Login Session
                                        bool IsSession = false;
                                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                                        {
                                            OConn.Open();
                                            OleDbCommand ComChkSesn = new OleDbCommand("Select employeesessions.id FROM employeesessions where (employeesessions.session_id = @sessionid)", OConn);
                                            ComChkSesn.Parameters.AddWithValue("@sessionid", Program.SessionId);
                                            var SessionResult = ComChkSesn.ExecuteScalar()+"";
                                            
                                            String StartTime = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss tt", new CultureInfo("en-SG"));
                                            if (String.IsNullOrEmpty(SessionResult))
                                            {
                                                //Create Session
                                                OleDbCommand com = new OleDbCommand("insert into employeesessions(restaurent_id,session_id,session_start_time,session_end_time,session_status) values (@restaurent_id,@session_id,@session_start_time,@session_end_time,@session_status);", OConn);
                                                com.Parameters.AddWithValue("@restaurent_id", _EmpExist.restaurant_id);
                                                com.Parameters.AddWithValue("@session_id", Program.SessionId);
                                                com.Parameters.AddWithValue("@session_start_time", StartTime);
                                                com.Parameters.AddWithValue("@session_end_time", "");
                                                com.Parameters.AddWithValue("@session_status", "Pending");
                                                var InsertResult = com.ExecuteNonQuery();
                                                com = new OleDbCommand("Select @@Identity;", OConn);
                                                string SessionId = com.ExecuteScalar() + "";
                                                Program.SessionId = SessionId;
                                            }
                                            else
                                            {
                                                Program.SessionId = SessionResult;
                                            }
                                            if(!String.IsNullOrEmpty(Program.SessionId))
                                            {
                                                OleDbCommand ComChkEmp = new OleDbCommand("Select sessionusers.id FROM sessionusers where ((sessionusers.session_id = "+Program.SessionId+") AND (sessionusers.employee_id = "+Program.CurrentEmployeeID+")) ", OConn);
                                                var EmpSessionChk = ComChkEmp.ExecuteScalar() + "";

                                                if (String.IsNullOrEmpty(EmpSessionChk))
                                                {
                                                    OleDbCommand sCom = new OleDbCommand("insert into sessionusers(session_id,employee_id,tanent_id,restaurent_id,emp_loggedin_time,emp_status) values(@session_id,@employee_id,@tanent_id,@restaurent_id,@emp_loggedin_time,'LoggedIn')", OConn);
                                                    sCom.Parameters.AddWithValue("@session_id", Program.SessionId);
                                                    sCom.Parameters.AddWithValue("@employee_id", _EmpExist.id);
                                                    sCom.Parameters.AddWithValue("@tanent_id", _EmpExist.tanent_id);
                                                    sCom.Parameters.AddWithValue("@restaurent_id", _EmpExist.restaurant_id);
                                                    sCom.Parameters.AddWithValue("@emp_loggedin_time", StartTime);
                                                    var result = sCom.ExecuteNonQuery();
                                                }
                                            }
                                            
                                        }

                                        //Fetch the Outlet Type
                                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                                        {
                                            OConn.Open();
                                            string SelectCommand = "Select * from restaurants where Server_id=@restaurent_id";
                                            OleDbCommand com = new OleDbCommand(SelectCommand, OConn);
                                            com.Parameters.AddWithValue("@restaurent_id", _EmpExist.restaurant_id);
                                            OleDbDataAdapter da = new OleDbDataAdapter(com);
                                            DataSet dsRestaurent = new DataSet("Restaurent");
                                            da.Fill(dsRestaurent);
                                            if(dsRestaurent.Tables.Count > 0)
                                            {
                                                DataTable dtRestaurent = dsRestaurent.Tables[0];
                                                //Fetch Details 
                                                if (dtRestaurent.Rows.Count > 0)
                                                {
                                                    DataRow CurrentRow = dtRestaurent.Rows[0];
                                                    var IsRestaurentExist = Program.Restaurents.Where(p => p.id == _EmpExist.restaurant_id).Any();
                                                    if(!IsRestaurentExist)
                                                    {
                                                        App_Off_BAL.Restaurant restaurent = new App_Off_BAL.Restaurant();

                                                        restaurent.id = CurrentRow["Server_id"] + "";
                                                        restaurent.tanent_id = CurrentRow["tanent_id"] + "";
                                                        restaurent.restaurant_name = CurrentRow["restaurant_name"] + "";
                                                        restaurent.main_category = CurrentRow["main_category"] + "";
                                                        restaurent.restaurant_type = CurrentRow["restaurant_type"] + "";
                                                        restaurent.location = CurrentRow["location"] + "";
                                                        restaurent.gst = CurrentRow["gst"] + "";
                                                        restaurent.contact_no = CurrentRow["contact_no"] + "";
                                                        restaurent.latitude = CurrentRow["latitude"] + "";
                                                        restaurent.longitude = CurrentRow["longitude"] + "";
                                                        restaurent.min_item_price = CurrentRow["min_item_price"] + "";
                                                        restaurent.min_order_price = CurrentRow["min_order_price"] + "";
                                                        restaurent.order_type = CurrentRow["order_type"] + "";
                                                        restaurent.shipping_charge = CurrentRow["shipping_charge"] + "";
                                                        restaurent.delivery_time = CurrentRow["delivery_time"] + "";
                                                        restaurent.delivery_format = CurrentRow["delivery_format"] + "";
                                                        restaurent.payment_types = CurrentRow["payment_types"] + "";
                                                        restaurent.restaurant_image = CurrentRow["restaurant_image"] + "";
                                                        restaurent.created = CurrentRow["created"] + "";
                                                        restaurent.delivery_hours = CurrentRow["delivery_hours"] + "";
                                                        restaurent.reserve_table = CurrentRow["reserve_table"] + "";
                                                        restaurent.closing_day = CurrentRow["closing_day"] + "";
                                                        restaurent.supported_payment_types = CurrentRow["supported_payment_types"] + "";
                                                        restaurent.ref_url = CurrentRow["ref_url"] + "";
                                                        restaurent.gst_no = CurrentRow["gst_no"] + "";
                                                        restaurent.gst_value = CurrentRow["gst_value"] + "";
                                                        Program.Restaurents.Add(restaurent);
                                                    }
                                                }
                                            }
                                        }

                                        Program.RestaurentID = _EmpExist.restaurant_id;
                                        Program.TenantID = Program.Restaurents.Where(p => p.id == _EmpExist.restaurant_id).Select(p => p.tanent_id).FirstOrDefault() + "";
                                        Program.OutletType = Program.Restaurents.Where(p => p.id == _EmpExist.restaurant_id).Select(p => p.restaurant_type).FirstOrDefault() + "";
                                        Program.StoreLogo = Program.Restaurents.Where(p => p.id == _EmpExist.restaurant_id).Select(p => p.restaurant_image).FirstOrDefault() + "";
                                        //Program.OutletType = result.outlet_Type;//Restraunt Table

                                        //Token is not required
                                        //Program.Token = result.data;
                                        //Form Opened Directly
                                        this.Hide();
                                        if (!IsMain)
                                        {
                                            frmMain _main = new frmMain();
                                            _main.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Login Failed For User ?", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("User not found ?", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else
                        {
                            MessageBox.Show("User Not Found ?", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    //Show Message for Sync.
                    //Sync all api altogether.
                    if (rowCount == 0)
                    {
                        Program.IsInitialSetup = true;
                        bool CheckConnection = Program.CheckForInternetConnection();

                        if (CheckConnection)
                        {
                            DialogResult msgRes = MessageBox.Show("Do you want to Sync Data from Server ?", "Sync System", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (DialogResult.Yes == msgRes)
                            {
                                Processing frmProcess = new Processing();
                                frmProcess.UserName = txtUserName.Text.Trim();
                                frmProcess.Password = txtPassword.Text.Trim();
                                frmProcess.ShowDialog();

                                MessageBox.Show(frmProcess.Message, "Sync Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Check Internet Connection ?", "Connection Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
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
            if (!IsMain)
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
