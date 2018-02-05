using App_BAL;
using App_Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public partial class Processing : Form
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsLoginSuccesful { get; set; }
        public string Message { get; set; }
        public static bool IsDone { get; set; }
        public Processing()
        {
            InitializeComponent();
           
            
        }

        private void Processing_Load(object sender, EventArgs e)
        {
            
            
        }

        private void Processing_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncData();
        }

        public void SyncData()
        {
            try
            {
                //Application.DoEvents();

                //Initial Process
                if (Program.IsInitialSetup)
                {
                    string URL = Program.BaseUrl;
                    string LoginUrl = URL + "/login?username=" + UserName + "&password=" + Password;

                    var GetLoginDetails = DataProviderWrapper.Instance.GetData(LoginUrl, Verbs.GET, "");
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    LoginCL result = serializer.Deserialize<LoginCL>(GetLoginDetails);
                    if (result.message == "success")
                    {
                        Message = "Sync Successfull";
                        IsLoginSuccesful = true;
                        //Program.Token = result.data;
                        //Fetch Data From Server
                        var ServerDataUrl = URL + "/getData?acess_token=" + result.data;
                        var GetDataDetails = DataProviderWrapper.Instance.GetData(ServerDataUrl, Verbs.GET, "");
                        App_Off_BAL.GetData Dataresult = serializer.Deserialize<App_Off_BAL.GetData>(GetDataDetails);

                        if (Dataresult != null)
                        {
                            if (Dataresult.status)
                            {
                                var mainData = Dataresult.databaseData;
                                if (mainData != null)
                                {
                                    #region Employee
                                    foreach (var item in mainData.employees)
                                    {
                                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                                        {
                                            OConn.Open();
                                            //String InsertCommand = @"insert into employees([restaurant_id],[tanent_id],[employee_name],[username],[password],[access_type],[access_token],[position],[created],[daily_cash_limit],[IsSync]) VALUES ('" + item.Employee.restaurant_id+"','"+ item.Employee.tanent_id+"','"+ item.Employee.employee_name+"','"+ item.Employee.username+"','"+ item.Employee.password+"','"+ item.Employee.access_type+"','"+item.Employee.acess_token+"','"+ item.Employee.position+"','"+ item.Employee.created+"','"+ item.Employee.daily_cash_limit+"','0')";
                                            String InsertCommand = @"insert into employees([server_id],[restaurant_id],[tanent_id],[employee_name],[username],[password],[access_type],[access_token],[position],[created],[daily_cash_limit],[IsSync]) VALUES (@server_id,@restaurant_id,@tanent_id,@employee_name,@username,@password,@access_type,@acess_token,@position,@created,@daily_cash_limit,'-1')";
                                            OleDbCommand com = new OleDbCommand(InsertCommand, OConn);
                                            com.Parameters.AddWithValue("@server_id", item.Employee.id);
                                            com.Parameters.AddWithValue("@restaurant_id", item.Employee.restaurant_id);
                                            com.Parameters.AddWithValue("@tanent_id", item.Employee.tanent_id);
                                            com.Parameters.AddWithValue("@employee_name", item.Employee.employee_name);
                                            com.Parameters.AddWithValue("@username", item.Employee.username);
                                            com.Parameters.AddWithValue("@password", item.Employee.password);
                                            com.Parameters.AddWithValue("@access_type", item.Employee.access_type);
                                            com.Parameters.AddWithValue("@acess_token", item.Employee.acess_token);
                                            com.Parameters.AddWithValue("@position", item.Employee.position);
                                            com.Parameters.AddWithValue("@created", item.Employee.created);
                                            com.Parameters.AddWithValue("@daily_cash_limit", item.Employee.daily_cash_limit);
                                            var insertResult = com.ExecuteNonQuery();
                                        }
                                    }
                                    #endregion

                                    #region Products
                                    foreach (var item in mainData.products)
                                    {
                                        string ProductsPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Products\" + item.Product.product_code + @"\";
                                        DirectoryInfo dirInfo = new DirectoryInfo(ProductsPath);
                                        if (!dirInfo.Exists)
                                        {
                                            dirInfo.Create();
                                        }
                                        String ProductImage = ProductsPath + item.Product.product_code + ".bmp";
                                        try
                                        {
                                            String WebProductURL = Program.ProductImagesLoc + item.Product.product_image;
                                            FileInfo _imgInfo = new FileInfo(ProductImage);
                                            if (_imgInfo.Exists)
                                                _imgInfo.Delete();

                                            using (WebClient client = new WebClient())
                                            {
                                                byte[] pic = client.DownloadData(WebProductURL);
                                                //string checkPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\1.png";
                                                //File.WriteAllBytes(checkPath, pic);
                                                File.WriteAllBytes(ProductImage, pic);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            var err = ex.Message;
                                        }

                                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                                        {
                                            OConn.Open();
                                            String InsertCommand = @"insert into Products(Server_Id,restaurent_id,tanent_id,product_name,product_price,manage_stock,total_stock,remaining_stock,main_category,outlet_categories,food_type,description,product_image,[created],order_type,product_code,IsSync) values 
                                                                                         (@server_id,@restaurent_id,@tanent_id,@product_name,@product_price,@manage_stock,@total_stock,@remaining_stock,@main_category,@outlet_categories,@food_type,@description,@product_image,@created,@order_type,@product_code,'-1')";
                                            OleDbCommand com = new OleDbCommand(InsertCommand, OConn);
                                            com.Parameters.AddWithValue("@server_id", item.Product.id);
                                            com.Parameters.AddWithValue("@restaurent_id", item.Product.restaurent_id);
                                            com.Parameters.AddWithValue("@tanent_id", item.Product.tanent_id);
                                            com.Parameters.AddWithValue("@product_name", item.Product.product_name);
                                            com.Parameters.AddWithValue("@product_price", item.Product.product_price);
                                            com.Parameters.AddWithValue("@manage_stock", item.Product.manage_stock);
                                            com.Parameters.AddWithValue("@total_stock", item.Product.total_stock);
                                            com.Parameters.AddWithValue("@remaining_stock", item.Product.remaining_stock);
                                            com.Parameters.AddWithValue("@main_category", item.Product.main_category);
                                            com.Parameters.AddWithValue("@outlet_categories", item.Product.outlet_categories);
                                            com.Parameters.AddWithValue("@food_type", item.Product.food_type);
                                            com.Parameters.AddWithValue("@description", item.Product.description);
                                            com.Parameters.AddWithValue("@product_image", ProductImage);
                                            com.Parameters.AddWithValue("@created", item.Product.created);
                                            com.Parameters.AddWithValue("@order_type", item.Product.order_type);
                                            com.Parameters.AddWithValue("@product_code", item.Product.product_code);
                                            var insertResult = com.ExecuteNonQuery();
                                        }
                                    }
                                    #endregion

                                    #region Restraurent Details
                                    foreach (var item in mainData.restaurentDetails)
                                    {
                                        string RestaurantPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Restaurant\" + item.Restaurant.id + @"\";
                                        DirectoryInfo dirInfo = new DirectoryInfo(RestaurantPath);
                                        if (!dirInfo.Exists)
                                        {
                                            dirInfo.Create();
                                        }
                                        String RestaurantImage = RestaurantPath + item.Restaurant.id + ".bmp";
                                        try
                                        {
                                            String WebRestaurentURL = Program.StoreImagesLoc + item.Restaurant.restaurant_image;
                                            FileInfo _imgInfo = new FileInfo(RestaurantImage);
                                            if (_imgInfo.Exists)
                                                _imgInfo.Delete();

                                            using (WebClient client = new WebClient())
                                            {
                                                byte[] pic = client.DownloadData(WebRestaurentURL);
                                                //string checkPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\1.png";
                                                //File.WriteAllBytes(checkPath, pic);
                                                File.WriteAllBytes(RestaurantImage, pic);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            var err = ex.Message;
                                        }


                                        using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                                        {
                                            OConn.Open();
                                            String InsertCommand = @"insert into restaurants(Server_id,tanent_id,restaurant_name,main_category,restaurant_type,[location],[gst],contact_no,[latitude],[longitude],min_item_price,min_order_price,order_type,shipping_charge,delivery_time,delivery_format,payment_types,restaurant_image,[created],delivery_hours,reserve_table,closing_day,supported_payment_types,ref_url,gst_no,[IsSync]) 
                                                                                      values(@server_id,@tanent_id,@restaurant_name,@main_category,@restaurant_type,@location,@gst,@contact_no,@latitude,@longitude,@min_item_price,@min_order_price,@order_type,@shipping_charge,@delivery_time,@delivery_format,@payment_types,@restaurant_image,@created,@delivery_hours,@reserve_table,@closing_day,@supported_payment_types,@ref_url,@gst_no,'-1')";
                                            OleDbCommand com = new OleDbCommand(InsertCommand, OConn);
                                            com.Parameters.AddWithValue("@server_id", item.Restaurant.id);
                                            com.Parameters.AddWithValue("@tanent_id", item.Restaurant.tanent_id);
                                            com.Parameters.AddWithValue("@restaurant_name", item.Restaurant.restaurant_name);
                                            com.Parameters.AddWithValue("@main_category", item.Restaurant.main_category);
                                            com.Parameters.AddWithValue("@restaurant_type", item.Restaurant.restaurant_type);
                                            com.Parameters.AddWithValue("@location", item.Restaurant.location);
                                            com.Parameters.AddWithValue("@gst", item.Restaurant.gst);
                                            com.Parameters.AddWithValue("@contact_no", item.Restaurant.contact_no);
                                            com.Parameters.AddWithValue("@latitude", item.Restaurant.latitude);
                                            com.Parameters.AddWithValue("@longitude", item.Restaurant.longitude);
                                            com.Parameters.AddWithValue("@min_item_price", item.Restaurant.min_item_price);
                                            com.Parameters.AddWithValue("@min_order_price", item.Restaurant.min_order_price);
                                            com.Parameters.AddWithValue("@order_type", item.Restaurant.order_type);
                                            com.Parameters.AddWithValue("@shipping_charge", item.Restaurant.shipping_charge);
                                            com.Parameters.AddWithValue("@delivery_time", item.Restaurant.delivery_time);
                                            com.Parameters.AddWithValue("@delivery_format", item.Restaurant.delivery_format);
                                            com.Parameters.AddWithValue("@payment_types", item.Restaurant.payment_types);
                                            com.Parameters.AddWithValue("@restaurant_image", RestaurantImage);
                                            com.Parameters.AddWithValue("@created", item.Restaurant.created);
                                            com.Parameters.AddWithValue("@delivery_hours", item.Restaurant.delivery_hours);
                                            com.Parameters.AddWithValue("@reserve_table", item.Restaurant.reserve_table);
                                            com.Parameters.AddWithValue("@closing_day", item.Restaurant.closing_day);
                                            com.Parameters.AddWithValue("@supported_payment_types", item.Restaurant.supported_payment_types);
                                            com.Parameters.AddWithValue("@ref_url", item.Restaurant.ref_url ?? "http://www.google.com");
                                            com.Parameters.AddWithValue("@gst_no", item.Restaurant.gst_no);
                                            var insertResult = com.ExecuteNonQuery();
                                        }
                                    }
                                    #endregion

                                    #region Users

                                    #endregion
                                }
                                else
                                {
                                    Message = "Unable to get Data from server";
                                    IsLoginSuccesful = false;
                                }
                            }
                        }
                        else
                        {
                            Message = "Unable to get Data from server!!!";
                            IsLoginSuccesful = false;
                        }
                      
                    }
                    else
                    {
                        Message = "Login Failed For User on Server!!!";
                        IsLoginSuccesful = false;
                    }
                }
                else
                {
                    //Perform Full Sync
                }
                
            }
            catch (Exception ex)
            {
                Message = "Sync Failed : " + Environment.NewLine + ex.Message;
                IsLoginSuccesful = false;
            }
            finally
            {
                
            }
            IsDone = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Up:
            if (IsDone)
            {
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
                backgroundWorker1 = null;
                GC.Collect();
                this.Close();
            }
            else
            {
                goto Up;
            }

        }
    }
}
