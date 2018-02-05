using App_BAL;
using BestariTerrace.UserControls;
using App_Wrapper;
using CustomServerControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BestariTerrace;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;
using System.Data.OleDb;

namespace BestariTerrace.Forms
{
    public partial class frmMain : Form
    {
        //Background Worker For the Hit API at Every 15s
        private System.ComponentModel.BackgroundWorker syncWorker;

        public static bool IsClosed = false;
        public int SelectedCategoryID = 0;
        public static bool IsOrder = false;
        public static EmOrderType CurrentOrderType;
        public frmMain()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.syncWorker = new System.ComponentModel.BackgroundWorker();
            syncWorker.DoWork += SyncWorker_DoWork;
            syncWorker.RunWorkerCompleted += SyncWorker_RunWorkerCompleted;
            syncWorker.ProgressChanged += SyncWorker_ProgressChanged;
            syncWorker.WorkerReportsProgress = true;
            syncWorker.WorkerSupportsCancellation = true;
        }

        public void ActivateRestraunt()
        {

        }

        private void SyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void SyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void SyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void GetStoreInfo()
        {
            //Load with StoreInfo
            try
            {
                App_BAL.Restaurant _resInfo = new Restaurant();
                var Info = Program.Restaurents.Where(p => p.id == Program.RestaurentID).FirstOrDefault();
                if (Info != null)
                {
                    _resInfo.address = Info.location;
                    _resInfo.contact_no = Info.contact_no;
                    _resInfo.gst_no = Info.gst_no;
                    _resInfo.gst_status = Info.gst;
                    _resInfo.restaurant_image = Info.restaurant_image;
                    _resInfo.restaurant_name = Info.restaurant_name;
                    _resInfo.restaurant_type = Info.restaurant_type;
                    Program.GSTValue = double.Parse(String.IsNullOrEmpty(Info.gst_value) ? "0":Info.gst_value);
                    Program.StoreInfo.message.Restaurant = _resInfo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StoreInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Program.OutletType.Contains("RESTAURANT"))
            {
                rdbDelivery.Visible = rdbOrder.Visible = rdbTakeWay.Visible = rdbReservation.Visible = true;
            }
            else
            {
                rdbTakeWay.Text = "SALES";
                rdbOrder.Visible = rdbReservation.Visible = false;
            }
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, lblOrderCount.Width, lblOrderCount.Height);
            this.lblOrderCount.Region = new Region(path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            uc_CategoryMenu1.BindData();
            uc_CategoryMenu1.EventCategoryClicked += uc_CategoryMenu1_EventCategoryClicked;
            flyLayout.AutoScroll = true;
            flyLayout.VerticalScroll.Visible = true;
            flyLayout.VerticalScroll.Enabled = true;
            GetStoreInfo();
            GetPendingOrders();
            BindProducts();
        }


        void uc_CategoryMenu1_EventCategoryClicked(object sender, EventArgs e)
        {
            rdbOrder.Checked = rdbDelivery.Checked = rdbReservation.Checked = rdbTakeWay.Checked = false;
            rdbProducts.Checked = true;
            string FoodType = sender.ToString().Trim();
            BindProducts(FoodType.ToLower());
        }

        private void BindProducts(String foodtype = "all")
        {
            try
            {

                if (rdbDelivery.Checked != true && rdbOrder.Checked != true && rdbTakeWay.Checked != true && rdbReservation.Checked != true)
                {
                    //string URL = Program.BaseUrl;
                    string ProductCommand = "Select * from products where restaurent_id=@restrauntId";

                    using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
                    {
                        OConn.Open();
                        string SelectCommand = ProductCommand;
                        OleDbCommand com = new OleDbCommand(SelectCommand, OConn);
                        //com.Parameters.AddWithValue("@tanentId", Program.TenantID);
                        com.Parameters.AddWithValue("@restrauntId", Program.RestaurentID);                       
                        OleDbDataAdapter da = new OleDbDataAdapter(com);
                        DataSet dsStore = new DataSet("Products");
                        da.Fill(dsStore);
                        if (dsStore.Tables.Count > 0)
                        {
                            DataTable dtStore = dsStore.Tables[0];
                            //Fetch Details 
                            if (dtStore.Rows.Count > 0)
                            {
                                for(int i=0;i<dtStore.Rows.Count;i++)
                                {
                                    DataRow CurrentRow = dtStore.Rows[i];
                                    App_BAL.ProductListCL _productInfo = new ProductListCL();
                                    _productInfo.ProductType = CurrentRow["food_type"] + "";
                                    _productInfo.ProductID = int.Parse(CurrentRow["Server_Id"] + "");
                                    _productInfo.ProductName = CurrentRow["product_name"] + "";
                                    _productInfo.ProductNumber = CurrentRow["product_code"] + "";
                                    _productInfo.Price = double.Parse(CurrentRow["product_price"] + "");
                                    _productInfo.ImageName = CurrentRow["product_image"] + "";

                                    var IsExists = Program.Products.Where(p => p.ProductID == _productInfo.ProductID).Any();
                                    if (!IsExists)
                                    {
                                        Program.Products.Add(_productInfo);
                                    }
                                }
                                
                            }
                        }
                    }
                    this.flyLayout.Controls.Clear();
                    this.flyLayout.VerticalScroll.Enabled = true;
                    this.flyLayout.VerticalScroll.Visible = true;
                    var ProductList = Program.Products;
                    if (foodtype !="all")
                    {
                        ProductList = Program.Products.Where(p => p.ProductType == foodtype).ToList();
                    }
                    AddCounterSaleLink();
                    foreach (var itm in ProductList)
                    {
                        CreateProdButtons(itm);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProductInfo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }

        private void AddCounterSaleLink()
        {
            Panel pnl = new Panel();

            pnl.Name = "pnl_0";
            pnl.Width = 200;
            pnl.Height = 220;
            pnl.Tag = Guid.NewGuid();
            pnl.BorderStyle = BorderStyle.FixedSingle;
            pnl.Cursor = Cursors.Hand;


            PictureBox pic = new PictureBox();
            pic.Name = "pic_0";
            pic.ImageLocation = Program.StoreLogo;
            pic.ErrorImage = global::BestariTerrace.Properties.Resources.IMG_NotFound;
            //pic.Image = global::BestariTerrace.Properties.Resources.IMG_NotFound;
            pic.Width = 200;
            pic.Height = 130;
            pic.BorderStyle = BorderStyle.None;
            pic.Margin = new Padding(0);
            pic.Location = new Point(0, 0);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Cursor = Cursors.Hand;
            pic.Click += pnl_Click;
            pic.Tag = Guid.NewGuid();
            pnl.Controls.Add(pic);

            Label lblName = new Label();
            lblName.Name = "lblProductName_0";
            lblName.Text = "Open Item";
            lblName.Location = new Point(5, pic.Height + 5);
            lblName.AutoSize = false;
            lblName.Width = pnl.Width - 10;
            lblName.Height = 80;
            lblName.Cursor = Cursors.Hand;
            lblName.Font = new Font("Segoe UI Semilight", 12, FontStyle.Regular);
            lblName.Click += pnl_Click;
            lblName.Tag = Guid.NewGuid();
            pnl.Controls.Add(lblName);

            pnl.Click += pnl_Click;
            pnl.Margin = new Padding(0, 0, 10, 20);
            this.flyLayout.Controls.Add(pnl);
        }

        private void CreateProdButtons(ProductListCL itm)
        {
            Panel pnl = new Panel();

            pnl.Name = "pnl_" + itm.ProductID;
            pnl.Width = 200;
            pnl.Height = 220;
            pnl.Tag = itm;
            pnl.BorderStyle = BorderStyle.FixedSingle;
            pnl.Cursor = Cursors.Hand;


            PictureBox pic = new PictureBox();
            pic.Name = "pic_" + itm.ProductID;
            pic.ImageLocation = itm.ImageName;
            pic.ErrorImage = global::BestariTerrace.Properties.Resources.IMG_NotFound;
            //pic.Image = global::BestariTerrace.Properties.Resources.IMG_NotFound;
            pic.Width = 200;
            pic.Height = 130;
            pic.BorderStyle = BorderStyle.None;
            pic.Margin = new Padding(0);
            pic.Location = new Point(0, 0);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Cursor = Cursors.Hand;
            pic.Click += pnl_Click;
            pic.Tag = itm;
            pnl.Controls.Add(pic);

            Label lblName = new Label();
            lblName.Name = "lblProductName_" + itm.ProductID;
            lblName.Text = itm.ProductName + Environment.NewLine + "MYR - " + itm.Price.ToString("N2");
            lblName.Location = new Point(5, pic.Height + 5);
            lblName.AutoSize = false;
            lblName.Width = pnl.Width - 10;
            lblName.Height = 80;
            lblName.Cursor = Cursors.Hand;
            lblName.Font = new Font("Segoe UI Semilight", 12, FontStyle.Regular);
            lblName.Click += pnl_Click;
            lblName.Tag = itm;
            pnl.Controls.Add(lblName);

            pnl.Click += pnl_Click;
            pnl.Margin = new Padding(0, 0, 10, 20);
            this.flyLayout.Controls.Add(pnl);
        }

        void pnl_Click(object sender, EventArgs e)
        {
            ProductListCL prod = new ProductListCL();
            try
            {
                if (sender is Panel)
                {
                    prod = (ProductListCL)(sender as Panel).Tag;
                }
                else if (sender is Label)
                {
                    prod = (ProductListCL)(sender as Label).Tag;
                }
                else if (sender is PictureBox)
                {
                    prod = (ProductListCL)(sender as PictureBox).Tag;
                }
            }
            catch (Exception ex)
            {
                var err = ex;
            }
            if (prod != null)
            {
                bool IsNew = false;
                CartItemsCL cl = new CartItemsCL();
                int ProductId = prod.ProductID;
                if (ProductId == 0)
                {
                    frmCounterSale _saleFrm = new frmCounterSale();
                    _saleFrm.ShowDialog();
                }
                else
                {
                    if (Program.cartItems.Count > 0 && ProductId > 0)
                    {
                        cl = Program.cartItems.Where(p => p.ProductID == ProductId).FirstOrDefault();
                        if (cl != null)
                        {
                            cl.Quantity += 1;
                            cl.Price = Convert.ToDouble(cl.OriginalPrice) * cl.Quantity;
                        }
                        else
                        {
                            IsNew = true;
                        }

                    }
                    else
                    {
                        IsNew = true;
                    }
                }

                if (IsNew)
                {
                    cl = new CartItemsCL();
                    cl.ProductID = ProductId;
                    cl.FoodType = prod.ProductType;
                    cl.Price = Convert.ToDouble(prod.Price);
                    cl.OriginalPrice = Convert.ToDouble(prod.Price);
                    cl.Quantity = 1;
                    cl.ProductName = prod.ProductName;
                    Program.cartItems.Add(cl);
                }


                //  IsConnected = true;
                //}
                // BindCart(Program.cartItems);
            }

            var source = new BindingSource(Program.cartItems, null);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = source;
            dataGridView1.ClearSelection();

            Program.TotalCart();
            lblCartTotal.DataBindings.Clear();
            lblGrandTotal.DataBindings.Clear();
            lblTax.DataBindings.Clear();
            //if(!IsConnected)
            //{
            var CartTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "CartTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
            lblCartTotal.DataBindings.Add(CartTotalBinding);
            var GrandTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "GrandTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
            lblGrandTotal.DataBindings.Add(GrandTotalBinding);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            DialogResult msgResult = MessageBox.Show("Do you want to Exit Application ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (msgResult == DialogResult.Yes)
            {
                frmManagerExit mgr = new frmManagerExit();
                mgr.ShowDialog();
                if (mgr.IsOK)
                {
                    Program.ClearData();
                    Application.ExitThread();
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindProductBySearchText();
        }

        private void BindProductBySearchText()
        {
            var ProductList = Program.Products.Where(p => ProductName.Contains(txtSearch.Text)).ToList();
            flyLayout.Controls.Clear();
            AddCounterSaleLink();
            foreach (var itm in ProductList)
            {
                CreateProdButtons(itm);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            TxtBox txtSearch = (TxtBox)sender;
            if (txtSearch != null)
            {
                if (e.KeyChar == (char)Keys.Return)
                {
                    BindProductBySearchText();
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (Program.cartItems.Count > 0)
            {
                string TableSelection = "";
                string OrderRemarks = "";

                if (CurrentOrderType == EmOrderType.CounterSale)
                {
                    //This is Counter Sale Part.
                }
                else
                {
                    if (Program.OutletType.Contains("RESTAURANT"))
                    {
                        frmOrderType _frmOrder = new frmOrderType();
                        _frmOrder.ShowDialog();
                        if (!frmOrderType.isClosed)
                        {
                            frmTableSelection frmTbl = new frmTableSelection();
                            frmTbl.orderType = CurrentOrderType;
                            frmTbl.ShowDialog();

                            OrderRemarks = frmTbl.tableRemarks;
                            TableSelection = frmTbl.tableSelection;

                            if (CurrentOrderType == EmOrderType.TakeOut)
                                TableSelection = "TakeAway";

                            if (String.IsNullOrEmpty(TableSelection))
                            {
                                MessageBox.Show("Please select table Number", " Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                    }
                    else
                    {
                        CurrentOrderType = EmOrderType.TakeOut;
                    }
                }

                var TotalAmount = 0.0;  // Total Price
                TotalAmount = Program.cartItems.Sum(p => p.Price);
                FrmContainer frm = new FrmContainer();
                ucPayment uc = new ucPayment();
                frm.Dock = DockStyle.Fill;
                uc.BindData(TotalAmount);
                frm.Width = uc.Width + 20;
                frm.Height = uc.Height + 40;
                frm.Controls.Add(uc);
                frm.StartPosition = FormStartPosition.CenterScreen;
                var dgRes = frm.ShowDialog();
                if (dgRes == System.Windows.Forms.DialogResult.OK)
                {
                    string OrderNumber = DateTime.UtcNow.Ticks.ToString();
                    CartCL cart = new CartCL();

                    cart.IsOrderConfirmed = true;
                    cart.OrderStatus = EmOrderStatus.Delivered;
                    cart.OrderID = 0;
                    cart.OrderNo = OrderNumber;
                    cart.DiscountAmt = uc.DiscountAmt;
                    cart.DiscountType = (uc.DiscountType == EmDiscountType.Amount ? "amount" : "percentage");
                    cart.OrderType = CurrentOrderType;
                    cart.PaymentType = uc.PayementType;
                    cart.Items = Program.cartItems;
                    cart.OrderTotal = Program.cartItems.First().GrandTotal.ToString("N");
                    cart.TableNo = TableSelection;
                    cart.OrderRemarks = OrderRemarks;
                    cart.IsCurrentOrder = true;
                    cart.TransactionID = uc.LastTransactionID;
                    MessageBox.Show("Order has been placed successfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Program.PlacedOrders.Add(cart);
                    //Program.OrderCount();
                    //Update Cart Order Number
                    foreach (var item in Program.cartItems)
                    {
                        item.orderNo = OrderNumber;
                        Program.PlacedCartItems.Add(item);
                    }
                    //Post Order
                    var NewOrderId = PostOrder(OrderNumber);
                    lblOrderCount.DataBindings.Clear();
                    var OrderCount = new Binding("Text", Program.OrderBindings, "OrderCount", true, DataSourceUpdateMode.Never, "0", "");
                    lblOrderCount.DataBindings.Add(OrderCount);
                    //Clear the Cart and Total Field
                    ClearList();
                    bool isCash = false;
                    bool ReprintMsg = true;

                    //Counter Sales
                    if (cart.OrderType == EmOrderType.CounterSale)
                    {
                        isCash = true;
                    }

                    //Printing
                    Up:
                    try
                    {
                        string filePath = Path.Combine(Environment.CurrentDirectory, "Printer.txt");
                        FileInfo _fileinfo = new FileInfo(filePath);
                        if (_fileinfo.Exists)
                        {
                            string[] lines = File.ReadAllLines(filePath);
                            if (lines.Length > 0)
                            {
                                string Kitchen = lines[0].Split('$')[1];
                                string Cashier = lines[1].Split('$')[1];
                                if (!String.IsNullOrEmpty(Cashier))
                                {
                                    Print(PrinterSetup.GetPrinterName(EmPrinterType.CashCounter), GetLogo("StoreLogo.bmp"));
                                    Print(PrinterSetup.GetPrinterName(EmPrinterType.CashCounter), GetDocument(NewOrderId, EmPrinterType.CashCounter));
                                }
                                if (!string.IsNullOrEmpty(Kitchen))
                                {
                                    if (!isCash)
                                        Print(PrinterSetup.GetPrinterName(EmPrinterType.Kitchen), GetDocument(NewOrderId, EmPrinterType.Kitchen));
                                }
                            }
                            else
                            {
                                ReprintMsg = false;
                                MessageBox.Show("No Printer installed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (ReprintMsg)
                    {
                        DialogResult reprintMsg = MessageBox.Show("Do you want to print again ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (reprintMsg == DialogResult.Yes)
                        {
                            isCash = true;
                            goto Up;
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Cart is Empty", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static void Print(string printerName, byte[] document)
        {

            TcpClient client = new TcpClient();
            client.Connect(printerName, 9100);
            NetworkStream stream = client.GetStream();
            stream.Write(document, 0, document.Length);
            System.Threading.Thread.Sleep(20);
            //stream.Close(5000); //Wait 5 seconds to ensure that the data is sent.
            client.Close();
        }

        public static byte[] GetDocument(string OrderNumber, EmPrinterType printerType)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Reset the printer bws (NV images are not cleared)
                PrintReceipt(bw, OrderNumber, printerType);

                //// Feed 3 vertical motion units and cut the paper with a 1 point cut
                //bw.Write(AsciiControlChars.GroupSeparator);
                //bw.Write('V');
                //bw.Write((byte)66);
                //bw.Write((byte)3);

                bw.Flush();

                return ms.ToArray();
            }
        }

        private void GetImage(string imgPath)
        {
            try
            {
                FileInfo _imgInfo = new FileInfo("StoreLogo.bmp");
                if (_imgInfo.Exists)
                    _imgInfo.Delete();

                using (WebClient client = new WebClient())
                {
                    byte[] pic = client.DownloadData(imgPath);
                    //string checkPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\1.png";
                    //File.WriteAllBytes(checkPath, pic);
                    File.WriteAllBytes("StoreLogo.bmp", pic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Store Logo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        /// <summary>
        /// This is the method we print the receipt the way we want. Note the spaces. 
        /// Wasted a lot of paper on this to get it right.
        /// </summary>
        /// <param name="bw"></param>
        public static void PrintReceipt(BinaryWriter bw, string OrderNumber, EmPrinterType printerType)
        {
            var Order = Program.PlacedOrders.FirstOrDefault(p => p.OrderNo == OrderNumber);
            var cartItems = Program.PlacedCartItems.Where(p => p.orderNo == OrderNumber && p.IsCounterSale == false).ToList();
            if (cartItems.Count > 0)
            {
                if (printerType == EmPrinterType.CashCounter)
                {
                    try
                    {
                        if (Program.StoreInfo.message.Restaurant.restaurant_name != null)
                            bw.Center(Program.StoreInfo.message.Restaurant.restaurant_name);
                        if (Program.StoreInfo.message.Restaurant.address != null)
                            bw.Center(Program.StoreInfo.message.Restaurant.address);
                        if (Program.StoreInfo.message.Restaurant.contact_no != null)
                            bw.Center(Program.StoreInfo.message.Restaurant.contact_no);
                        if (Program.StoreInfo.message.Restaurant.gst_no != null)
                            bw.Center("GST NO:" + Program.StoreInfo.message.Restaurant.gst_no);
                    }
                    catch
                    {
                        bw.Center("[POS]");
                        bw.Center("[Address]");
                        bw.Center("[ContactNumber]");
                        bw.Center("[GSTNumber]");
                    }

                    bw.LeftJustify("------------------------------------------------");
                    bw.NormalFont("Invoice #: " + OrderNumber);
                    bw.NormalFont("Table No#: " + Order.TableNo);
                    bw.NormalFont("Staff : test User");
                    bw.NormalFont("Date: " + DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss tt", new CultureInfo("en-SG")));
                    bw.NormalFont("------------------------------------------------");
                    bw.FeedLines(1);
                    bw.NormalFont("------------------------------------------------");

                    bw.NormalFont(("Description").PadRight(35) + ("Qty.").PadRight(5) + ("Amt.").PadLeft(8));
                    bw.NormalFont("------------------------------------------------");
                    var FormatCartItems = FormatLine(OrderNumber, cartItems, 33, printerType);
                    foreach (var item in FormatCartItems)
                    {
                        bw.NormalFont(item.LineText);
                        bw.FeedLines(1);
                    }
                    bw.NormalFont("------------------------------------------------");
                    bw.NormalFont("Number of Items : " + cartItems.Sum(p => p.Quantity));

                    try
                    {

                        //if (Program.StoreInfo.message.Restaurant.gst_status != null)
                        if (Program.OutletType.Contains("RESTAURANT"))
                        {
                            bw.NormalFont("GST : 6.00 %");
                            var Amount = decimal.Parse(cartItems.FirstOrDefault().CartTotal.ToString("N2"));
                            var GST = decimal.Parse("6");
                            var Tax = decimal.Parse(((Convert.ToDecimal(Amount) * GST) / 100).ToString("N2"));
                            bw.NormalFont("GST Price : " + Tax);
                            bw.NormalFont("Amount : " + (Amount - Tax).ToString());
                            var roundOffAmt = Math.Round(Amount, 2);
                            bw.NormalFont("Grand Total : " + roundOffAmt.ToString());
                            bw.NormalFont("Rounding Adjustment : " + (roundOffAmt - Amount).ToString());
                        }
                        else
                        {
                            var Amount = decimal.Parse(cartItems.FirstOrDefault().CartTotal.ToString("N2"));
                            var GST = decimal.Parse("6");
                            var Tax = decimal.Parse(((Convert.ToDecimal(Amount) * GST) / 100).ToString("N2"));
                            Amount += Tax;
                            bw.NormalFont("GST Price : " + Tax);
                            bw.NormalFont("Amount : " + Amount.ToString());
                            var roundOffAmt = Math.Round(Amount, 2);
                            bw.NormalFont("Grand Total :  " + roundOffAmt.ToString());
                            bw.NormalFont("Rounding Adjustment : " + (roundOffAmt - Amount).ToString());
                        }
                    }
                    catch
                    {
                        var Amount = decimal.Parse(cartItems.FirstOrDefault().CartTotal.ToString("N2"));
                        var GST = decimal.Parse("6");
                        var Tax = decimal.Parse(((Convert.ToDecimal(Amount) * GST) / 100).ToString("N2"));
                        Amount += Tax;
                        bw.NormalFont("GST Price : " + Tax);
                        bw.NormalFont("Amount : " + Amount.ToString());
                        var roundOffAmt = Math.Round(Amount, 2);
                        bw.NormalFont("Grand Total :  " + roundOffAmt.ToString());
                        bw.NormalFont("Rounding Adjustment : " + (roundOffAmt - Amount).ToString());
                    }

                    bw.NormalFont("------------------------------------------------");
                    bw.FinishLines();
                    bw.CutPaper();
                }
                else
                {
                    var FormatCartItems = FormatLine(OrderNumber, cartItems, 40, printerType);
                    foreach (var item in FormatCartItems)
                    {
                        bw.NormalFont(("Description").PadRight(43) + ("Qty.").PadRight(5));
                        bw.NormalFont("------------------------------------------------");
                        bw.NormalFont(item.LineText);
                        bw.FeedLines(3);
                        bw.CutPaper();
                    }
                }
            }
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

        public string PostOrder(string OrderNo)
        {
            string fresult = OrderNo;
            try
            {
                //POST Cart Details
                List<NewOrder> Orders = new List<NewOrder>();
                var CurrentOrder = Program.PlacedOrders.FirstOrDefault(p => p.IsCurrentOrder == true && p.OrderNo == OrderNo);
                var CartItems = Program.cartItems.Where(p => p.orderNo == OrderNo).ToList();
                foreach (var item in CartItems)
                {
                    NewOrder _order = new NewOrder();
                    _order.product_id = (item.IsCounterSale) ? "0" : item.ProductID.ToString();
                    _order.product_name = item.ProductName.ToString();
                    _order.quantity = item.Quantity.ToString();
                    _order.price = Convert.ToInt32(Math.Ceiling(decimal.Parse(item.Price.ToString())));

                    if (item.IsCounterSale)
                    {
                        _order.delivery_type = "counter_sale";
                    }
                    else
                    {
                        if (CurrentOrder.OrderType == EmOrderType.TakeOut)
                        {
                            _order.delivery_type = "take_away";
                        }
                        else if (CurrentOrder.OrderType == EmOrderType.DineIn)
                        {
                            _order.delivery_type = "dine_in";
                        }
                    }

                    Orders.Add(_order);
                }

                string URL = Program.BaseUrl;
                string PostOrderURL = URL + "/makedineinorder?acess_token=" + Program.Token;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var postData = serializer.Serialize(Orders);
                var PostResult = DataProviderWrapper.Instance.PostData(PostOrderURL, postData);
                var result = serializer.Deserialize<TransactionAPICL>(PostResult);
                if (result != null)
                {
                    fresult = result.message;
                }
                if (fresult == "Success.")
                {
                    CurrentOrder.OrderNo = result.orderid;
                    foreach (var item in CartItems)
                    {
                        item.orderNo = result.orderid;
                    }
                    fresult = result.orderid;
                    //Update the Discount Details
                    if (CurrentOrder.DiscountAmt > 0)
                    {
                        string DiscountURL = "/applyDiscount?order_id=" + result.orderid + "&discount_value=" + CurrentOrder.DiscountAmt + "&discount_type=" + CurrentOrder.DiscountType + "&acess_token=" + Program.Token;
                        var DiscountResult = DataProviderWrapper.Instance.GetData(DiscountURL, Verbs.GET, "");
                    }

                    //Update the Table Details
                    if (CurrentOrderType == EmOrderType.DineIn)
                    {
                        string TableUrl = "/addTableDetails?reservation_id=" + result.orderid + "&remarks=" + CurrentOrder.OrderRemarks + "&table_no=" + CurrentOrder.TableNo + "&acess_token=" + Program.Token;
                        var TableResult = DataProviderWrapper.Instance.GetData(TableUrl, Verbs.GET, "");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return fresult;
        }

        public static List<PrintLine> FormatLine(String OrderNumber, List<CartItemsCL> cartItems, int splitInt, EmPrinterType printerType)
        {
            int globalLen = 48;
            var Order = Program.PlacedOrders.FirstOrDefault(p => p.OrderNo == OrderNumber);
            List<PrintLine> result = new List<PrintLine>();
            foreach (var item in cartItems)
            {
                string ProductCode = Program.Products.FirstOrDefault(p => p.ProductID == item.ProductID)?.ProductNumber + "";
                string DescText = ProductCode + " " + item.ProductName;
                string Quantity = item.Quantity.ToString();
                string Price = item.Price.ToString("N2");
                var DescSplit = SplitInParts(DescText, splitInt).ToList();
                for (int i = 0; i < DescSplit.Count(); i++)
                {
                    PrintLine lineitem = new PrintLine();
                    lineitem.LineNo = i;
                    if (i == 0)
                    {
                        if (printerType == EmPrinterType.CashCounter)
                            lineitem.LineText = DescSplit[i].PadRight(35) + Quantity.PadRight(5) + String.Format("{0:n2}", Price.PadLeft(8));
                        else
                        {
                            lineitem.LineText = DescSplit[i].PadRight(43) + Quantity.PadRight(5);
                        }

                    }
                    else
                    {
                        lineitem.LineText = DescSplit[i].PadRight(35);
                    }

                    if ((i + 1 >= DescSplit.Count()) && printerType == EmPrinterType.Kitchen)
                    {
                        string lineText = lineitem.LineText;
                        string remarkstr = "Remarks" + Environment.NewLine;
                        remarkstr += Order.OrderRemarks;
                        lineitem.LineText += Environment.NewLine + remarkstr;
                    }
                    result.Add(lineitem);
                }
            }
            return result;
        }
        public static IEnumerable<String> SplitInParts(string txt, Int32 partLength)
        {
            for (var i = 0; i < txt.Length; i += partLength)
                yield return txt.Substring(i, Math.Min(partLength, txt.Length - i));
        }

        private void ClearList()
        {
            //Program.cartItems.Clear();
            //Program.TotalCart();

            dataGridView1.Rows.Clear();
            dataGridView1.DataBindings.Clear();
            lblTax.Text = "0.00";
            lblCartTotal.Text = "0.00";
            lblGrandTotal.Text = "0.00";
            lblCartTotal.DataBindings.Clear();
            lblGrandTotal.DataBindings.Clear();
            lblTax.DataBindings.Clear();

        }

        private void lblOrderCount_Click(object sender, EventArgs e)
        {
            if (!rdbDelivery.Checked)
            {
                rdbDelivery.Checked = true;
            }

            //if (!rdbReservation.Checked)
            //{
            //    rdbReservation.Checked = true;
            //}
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool IsSelected = dataGridView1.CurrentCell.Selected;
            int ProductId = 0;
            if (IsSelected)
            {
                string value = dataGridView1.Rows[e.RowIndex].Cells["Column1"].FormattedValue.ToString();
                int.TryParse(value, out ProductId);
            }

            if (ProductId != 0)
            {
                Program.SelectedProductId = ProductId;

                frmProductQty objFrmQty = new frmProductQty();
                objFrmQty.ShowDialog();
            }
            if (Program.cartItems.Count < 1)
            {
                lblCartTotal.DataBindings.Clear();
                lblGrandTotal.DataBindings.Clear();
                lblTax.DataBindings.Clear();
                lblTax.Text = "0.00";
                lblCartTotal.Text = "0.00";
                lblGrandTotal.Text = "0.00";
            }
            else
            {
                Program.TotalCart();
                lblCartTotal.DataBindings.Clear();
                lblGrandTotal.DataBindings.Clear();
                lblTax.DataBindings.Clear();

                var CartTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "CartTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
                lblCartTotal.DataBindings.Add(CartTotalBinding);
                var GrandTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "GrandTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
                lblGrandTotal.DataBindings.Add(GrandTotalBinding);
            }
        }

        private void rdbOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOrder.Checked)
            {
                uc_CategoryMenu1.SetCurrentControlBtnName("");
                uc_CategoryMenu1.SelectedControls();
                BindOrders(false, EmOrderType.DineIn);
                IsOrder = true;
            }
        }

        private void rdbDelivery_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDelivery.Checked)
            {
                uc_CategoryMenu1.SetCurrentControlBtnName("");
                uc_CategoryMenu1.SelectedControls();
                BindOrders(false, EmOrderType.Delivery);
                IsOrder = true;
            }
        }

        private void ovalShape2_Click(object sender, EventArgs e)
        {
            ClearList();
        }

        public void BindOrders(bool? IsOrderConfirmed = null, EmOrderType OrderType = EmOrderType.DineIn)
        {
            this.flyLayout.Controls.Clear();
            //this.flyLayout.AutoScroll = false;
            this.flyLayout.VerticalScroll.Visible = false;
            this.flyLayout.VerticalScroll.Enabled = false;
            this.flyLayout.Refresh();
            int Count = Program.PlacedOrders.Where(p => p.OrderType == OrderType).Count();
            if (Count > 0)
            {
                uc_ConfirmOrder uc = new uc_ConfirmOrder();
                uc.Width = flyLayout.Width - 15;
                uc.Height = flyLayout.Height - 15;
                uc.AutoScroll = true;
                uc.VerticalScroll.Visible = true;
                uc.VerticalScroll.Enabled = true;
                if (OrderType == EmOrderType.Delivery)
                {
                    uc.UpdateGridColumns(EmGridType.Delivery);
                }
                else if (OrderType == EmOrderType.DineIn)
                {
                    uc.UpdateGridColumns(EmGridType.OrderIn);
                }
                else if (OrderType == EmOrderType.TakeOut)
                {
                    uc.UpdateGridColumns(EmGridType.TakeAway);
                }
                uc.BindData(IsOrderConfirmed, OrderType);
                this.flyLayout.Controls.Add(uc);
            }
        }

        public void BindReservations()
        {
            this.flyLayout.Controls.Clear();
            //this.flyLayout.AutoScroll = false;
            this.flyLayout.VerticalScroll.Enabled = false;
            this.flyLayout.VerticalScroll.Visible = false;
            this.flyLayout.Refresh();
            uc_ConfirmOrder uc = new uc_ConfirmOrder();
            uc.Width = flyLayout.Width - 15;
            uc.Height = flyLayout.Height - 15;
            uc.AutoScroll = true;
            uc.VerticalScroll.Visible = true;
            uc.VerticalScroll.Enabled = true;
            uc.UpdateGridColumns(EmGridType.Reservation);
            uc.BindReservations();
            flyLayout.Controls.Add(uc);
        }
        private void rdbProducts_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbProducts.Checked)
                BindProducts();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetPendingOrders();
            if (!IsOrder)
            {
                uc_CategoryMenu1.SetCurrentControlBtnName("All");
                uc_CategoryMenu1.SelectedControls();
                BindProducts();
            }
            else
            {
                if (rdbReservation.Checked)
                {
                    uc_CategoryMenu1.SetCurrentControlBtnName("");
                    uc_CategoryMenu1.SelectedControls();
                    BindReservations();
                }
                else if (rdbOrder.Checked)
                {
                    uc_CategoryMenu1.SetCurrentControlBtnName("");
                    uc_CategoryMenu1.SelectedControls();
                    BindOrders(false, EmOrderType.DineIn);
                }
                else if (rdbDelivery.Checked)
                {
                    uc_CategoryMenu1.SetCurrentControlBtnName("");
                    uc_CategoryMenu1.SelectedControls();
                    BindOrders(false, EmOrderType.Delivery);
                }
                else if (rdbTakeWay.Checked)
                {
                    uc_CategoryMenu1.SetCurrentControlBtnName("");
                    uc_CategoryMenu1.SelectedControls();
                    BindOrders(false, EmOrderType.TakeOut);
                }
            }
        }

        private void rdbReservation_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbReservation.Checked)
            {
                uc_CategoryMenu1.SetCurrentControlBtnName("");
                uc_CategoryMenu1.SelectedControls();
                BindReservations();
                IsOrder = true;
            }
        }

        public void GetPendingOrders()
        {
            String CurrentDate = DateTime.UtcNow.ToString("MM/dd/yyyy", new CultureInfo("en-SG"));
            using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
            {
                OConn.Open();
                string SelectCommand = "Select * from orderdetails where created=@currentDate AND restaurant_id=@restaurentId AND tanent_id=@tanentId";
                OleDbCommand com = new OleDbCommand(SelectCommand, OConn);
                com.Parameters.AddWithValue("@currentDate", CurrentDate);
                com.Parameters.AddWithValue("@restaurentId", Program.RestaurentID);
                com.Parameters.AddWithValue("@tanentId", Program.TenantID);
                OleDbDataAdapter da = new OleDbDataAdapter(com);
                DataSet dsPendingOrder = new DataSet("PendingOrderInfo");
                da.Fill(dsPendingOrder);
                if (dsPendingOrder.Tables.Count > 0)
                {
                    DataTable dtStore = dsPendingOrder.Tables[0];
                    //Fetch Details 
                    if (dtStore.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtStore.Rows.Count; i++)
                        {
                            DataRow dr = dtStore.Rows[i];
                            var ordStatus = EmOrderStatus.Pending;
                            var btnActionText = "Update Status";
                            var _OrderType = EmOrderType.Delivery;

                            string OrderType = dr["order_type"] + "";
                            if (OrderType.ToLower() == "dine_in")
                            {
                                _OrderType = EmOrderType.DineIn;
                            }
                            else if (OrderType.ToLower() == "takeout" || OrderType.ToLower() == "take_away")
                            {
                                _OrderType = EmOrderType.TakeOut;
                            }
                            else
                            {
                                _OrderType = EmOrderType.Delivery;
                            }

                            string orderStatus = dr["order_status"] + "";
                            if (orderStatus.ToLower().Trim() == "pending")
                            {
                                ordStatus = EmOrderStatus.Pending;
                                btnActionText = "Pending Status";
                            }
                            else if (orderStatus.ToLower().Trim() == "in_progress")
                            {
                                ordStatus = EmOrderStatus.Confirmed;
                                btnActionText = "In-Progress Status";
                            }
                            else if (orderStatus.ToLower().Trim() == "completed")
                            {
                                ordStatus = EmOrderStatus.Delivered;
                                btnActionText = "Delivered";
                            }

                            App_BAL.CartCL _Order = new CartCL();
                            _Order.OrderDate = DateTime.Parse(dr["created"] + "");
                            _Order.OrderNo = dr["order_id"] + "";
                            _Order.BtnActionStatus = btnActionText;
                            _Order.OrderType = _OrderType;
                            _Order.OrderStatus = ordStatus;
                            _Order.OrderTotal = dr["total"] + "";

                            CartItemsCL newCartItem = new CartItemsCL();
                            newCartItem.orderNo = dr["order_id"] + "";
                            newCartItem.ProductID = int.Parse(dr["product_id"] + "");
                            newCartItem.ProductName = dr["product_name"] + "";
                            newCartItem.OriginalPrice = double.Parse(dr["product_price"] + "");//cItem.cart.product_price 
                            newCartItem.GrandTotal = double.Parse(dr["total_price"] + "");
                            newCartItem.Quantity = int.Parse(dr["quantity"] + "");


                            bool isExist = Program.PlacedOrders.Where(p => p.OrderNo == _Order.OrderNo).Any();
                            if (!isExist)
                            {
                                Program.PlacedOrders.Add(_Order);
                                Program.cartItems.Add(newCartItem);
                            }
                        }

                    }
                }
            }

            //Reservation
            using (OleDbConnection OConn = new OleDbConnection(Program.ConnectionStr))
            {
                OConn.Open();
                string SelectCommand = "Select * from tableorders where  order_date=@currentDate  AND restaurent_id=@restaurentId AND tanent_id=@tanentId";
                OleDbCommand com = new OleDbCommand(SelectCommand, OConn);
                com.Parameters.AddWithValue("@currentDate", CurrentDate);
                com.Parameters.AddWithValue("@restaurentId", Program.RestaurentID);
                com.Parameters.AddWithValue("@tanentId", Program.TenantID);
                OleDbDataAdapter da = new OleDbDataAdapter(com);
                DataSet dsReservationInfo = new DataSet("ReservationInfo");
                da.Fill(dsReservationInfo);
                if (dsReservationInfo.Tables.Count > 0)
                {
                    DataTable dtResInfo = dsReservationInfo.Tables[0];
                    for (int x = 0; x < dtResInfo.Rows.Count; x++)
                    {
                        var dr = dtResInfo.Rows[x];
                        ReservationCL _newReservation = new ReservationCL();
                        _newReservation.TableId = dr["id"] + "";
                        _newReservation.TableNo = dr["table_no"] + "";
                        _newReservation.RestrauntId = dr["restaurent_id"] + "";
                        _newReservation.DinerName = dr["diner_name"] + "";
                        _newReservation.GuestCount = dr["guests"] + "";
                        _newReservation.MobileNo = dr["mobile"] + "";
                        _newReservation.ReservationDate = dr["order_date"] + "";
                        _newReservation.ReservationTime = dr["from_time"] + "-" + dr["to_time"];
                        _newReservation.ReservationStatus = dr["status"] + "";
                        if (_newReservation.ReservationStatus.ToLower() == "completed")
                            _newReservation.ActionText = "Assigned";
                        else
                            _newReservation.ActionText = "Assign Table";

                        var isExist = Program.Reservations.Where(p => p.TableId == _newReservation.TableId).Any();
                        if (!isExist)
                        {
                            Program.Reservations.Add(_newReservation);
                        }
                    }
                }

            }
            //Get the Counting
            Program.OrderCount(EmOrderType.Delivery);
            //Set the Notification Count
            lblOrderCount.DataBindings.Clear();
            var OrderCount = new Binding("Text", Program.OrderBindings, "OrderCount", true, DataSourceUpdateMode.OnPropertyChanged, "0", "");
            lblOrderCount.DataBindings.Add(OrderCount);
        }

        private void rdbTakeWay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTakeWay.Checked)
            {
                uc_CategoryMenu1.SetCurrentControlBtnName("");
                uc_CategoryMenu1.SelectedControls();
                BindOrders(true, EmOrderType.TakeOut);
                IsOrder = true;
            }
        }

        private void lblSettings_Click(object sender, EventArgs e)
        {
            PrinterSetup _prntSet = new PrinterSetup();
            _prntSet.ShowDialog();
        }

        #region LogoPrint
        public byte[] GetLogo(string LogoFileName)
        {
            if (!File.Exists(LogoFileName))
                return null;

            BitmapData data = GetBitmapData(LogoFileName);
            BitArray dots = data.Dots;

            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write((char)0x1B);
            bw.Write('@');

            bw.Write((char)0x1B);
            bw.Write((byte)97);
            bw.Write((byte)1);
            //bw.Write('3');
            //bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)0x1B);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }
                offset += 24;
                bw.Write((char)0x0A);
            }
            // Restore the line spacing to the default of 30 dots.
            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)30);

            bw.Flush();
            byte[] bytes = stream.ToArray();
            return bytes;
        }

        public static BitmapData GetBitmapData(string bmpFileName)
        {
            using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
            {
                var threshold = 127;
                var index = 0;
                double multiplier = 200; // this depends on your printer model. for Beiyang you should use 1000
                double scale = (double)(multiplier / (double)bitmap.Width);
                int xheight = (int)(bitmap.Height * scale);
                int xwidth = (int)(bitmap.Width * scale);
                var dimensions = xwidth * xheight;
                var dots = new BitArray(dimensions);

                for (var y = 0; y < xheight; y++)
                {
                    for (var x = 0; x < xwidth; x++)
                    {
                        var _x = (int)(x / scale);
                        var _y = (int)(y / scale);
                        var color = bitmap.GetPixel(_x, _y);
                        var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                        dots[index] = (luminance < threshold);
                        index++;
                    }
                }

                return new BitmapData()
                {
                    Dots = dots,
                    Height = (int)(bitmap.Height * scale),
                    Width = (int)(bitmap.Width * scale)
                };
            }
        }


        #endregion

        private void btnlogout_Click(object sender, EventArgs e)
        {
            DialogResult msgResult = MessageBox.Show("Do you want to Logout Application ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (msgResult == DialogResult.Yes)
            {
                string URL = Program.BaseUrl;
                string LogoutURL = URL + "/logout?acess_token=" + Program.Token + "&session_id=" + Program.SessionId;
                var GetStatus = DataProviderWrapper.Instance.GetData(LogoutURL, Verbs.GET, "");

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var result = serializer.Deserialize<logoutCL>(GetStatus);
                Program.IsLogined = false;
                Up:
                frmLogin obj = new frmLogin();
                obj.IsMain = true;
                obj.ShowDialog();

                if (!Program.IsLogined)
                {
                    goto Up;
                }
            }
        }

        private void btnCounterSale_Click(object sender, EventArgs e)
        {
            CurrentOrderType = EmOrderType.CounterSale;
            frmCounterSale _saleFrm = new frmCounterSale();
            _saleFrm.ShowDialog();
            if (Program.cartItems.Count > 0)
            {
                var source = new BindingSource(Program.cartItems, null);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = source;
                dataGridView1.ClearSelection();

                Program.TotalCart();
                lblCartTotal.DataBindings.Clear();
                lblGrandTotal.DataBindings.Clear();
                lblTax.DataBindings.Clear();

                var CartTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "CartTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
                lblCartTotal.DataBindings.Add(CartTotalBinding);
                var GrandTotalBinding = new Binding("Text", Program.cartItems.FirstOrDefault(), "GrandTotal", true, DataSourceUpdateMode.Never, "0.00", "N");
                lblGrandTotal.DataBindings.Add(GrandTotalBinding);
            }
        }
    }

    public class BitmapData
    {
        public BitArray Dots
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
    }


}
