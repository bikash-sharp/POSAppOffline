using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App_Wrapper;
using System.Web.Script.Serialization;
using App_BAL;
using System.Windows.Forms.VisualStyles;
using BestariTerrace.Forms;

namespace BestariTerrace.UserControls
{
    public partial class uc_ConfirmOrder : UserControl
    {
        public static EmGridType _gridType;
        public uc_ConfirmOrder()
        {
            InitializeComponent();
        }

        public void BindReservations()
        {
            // dataGridView1.Columns.Clear();
            if (_gridType == EmGridType.Reservation)
            {
                ////Load with Reservations
                //string URL = Program.BaseUrl;
                //string ChangeOrdStatusURL = URL + "/pendingreservations?acess_token=" + Program.Token;

                //var GetStatus = DataProviderWrapper.Instance.GetData(ChangeOrdStatusURL, Verbs.GET, "");
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //var result = serializer.Deserialize<ReservationListAPICL>(GetStatus);

                //if (result.data != null)
                //{
                //    if (result.data.Count > 0)
                //    {
                //        var dataLst = result.data;
                //        foreach (var item in dataLst)
                //        {
                //            var isExist = Program.Reservations.Where(p => p.TableId == item.Tableorder.id).Any();
                //            if (!isExist)
                //            {
                //                ReservationCL reserve = new ReservationCL();
                //                reserve.TableId = item.Tableorder.id;
                //                reserve.TableNo = "0"; //item.TableNo
                //                reserve.RestrauntId = item.Tableorder.restaurent_id;
                //                reserve.DinerName = item.Tableorder.diner_name;
                //                reserve.GuestCount = item.Tableorder.guests;
                //                reserve.MobileNo = item.Tableorder.mobile;
                //                reserve.ReservationDate = item.Tableorder.date;
                //                reserve.ReservationTime = item.Tableorder.from_time + "-" + item.Tableorder.to_time;
                //                reserve.ReservationStatus = item.Tableorder.status;
                //                if (item.Tableorder.status.ToLower() == "completed")
                //                    reserve.ActionText = "Assigned";
                //                else
                //                    reserve.ActionText = "Assign Table";
                //                Program.Reservations.Add(reserve);
                //            }
                //        }
                //    }
                //}

                var source = new BindingSource(Program.Reservations.OrderByDescending(p => p.ReservationStatus).ToList(), null);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = source;
                dataGridView1.ClearSelection();
                //UpdateGridColumnType(EmGridType.Reservation);
            }

            //Clear All Label DatabBindings
            lblOrderTotal.Visible = false;
            label2.Visible = false;
            lblOrderTotal.DataBindings.Clear();
        }

        public void BindData(bool? IsOrderConfirmed = null, EmOrderType OrderType = EmOrderType.DineIn)
        {
            //GetPendingOrders();
            var OrderLst = Program.PlacedOrders.Where(p => p.OrderType == OrderType).ToList();
            //Clear All DatabBindings and Columns
            lblOrderTotal.DataBindings.Clear();
            lblOrderTotal.Visible = true;
            label2.Visible = true;
            Program.OrderCount(OrderType);
            if (OrderType == EmOrderType.Delivery)
            {
                var OrderSum = new Binding("Text", Program.OrderBindings, "SumDeliveryConfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
                lblOrderTotal.DataBindings.Add(OrderSum);
            }
            else if (OrderType == EmOrderType.DineIn)
            {
                var OrderSum = new Binding("Text", Program.OrderBindings, "SumDineInConfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
                lblOrderTotal.DataBindings.Add(OrderSum);
            }
            else if (OrderType == EmOrderType.Reservation)
            {
                var OrderSum = new Binding("Text", Program.OrderBindings, "SumReservationConfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
                lblOrderTotal.DataBindings.Add(OrderSum);
            }
            else if (OrderType == EmOrderType.TakeOut)
            {
                var OrderSum = new Binding("Text", Program.OrderBindings, "SumTakeAwayConfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
                lblOrderTotal.DataBindings.Add(OrderSum);
            }
            //if (IsOrderConfirmed.Value)
            //{
            //    var OrderSum = new Binding("Text", Program.OrderBindings, "SumConfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
            //    lblOrderTotal.DataBindings.Add(OrderSum);

            //}
            //else
            //{
            //    var OrderSum = new Binding("Text", Program.OrderBindings, "SumUnconfirmedAmountTotal", true, DataSourceUpdateMode.Never, "0.00", "F");
            //    lblOrderTotal.DataBindings.Add(OrderSum);
            //}
            if (_gridType == EmGridType.Delivery || _gridType == EmGridType.OrderIn || _gridType == EmGridType.TakeAway)
            {
                var source = new BindingSource(OrderLst, null);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = source;
                dataGridView1.ClearSelection();
            }

        }


        //private void CreateControl(App_BAL.CartCL itm)
        //{
        //    Panel pnl = new Panel();
        //    pnl.Height = 60;
        //    //pnl.Width = flyLayout.Width - 10;
        //    pnl.BackColor = Color.LightGray;

        //    Label lbl = new Label();
        //    lbl.Text = itm.OrderNo;
        //    lbl.AutoSize = false;
        //    lbl.Height = 50;
        //    lbl.Width = 200;
        //    lbl.Location = new Point(10, 5);
        //    lbl.TextAlign = ContentAlignment.MiddleCenter;
        //    pnl.Controls.Add(lbl);

        //    Button btn = new Button();
        //    if (itm.IsOrderConfirmed)
        //    {
        //        btn.Text = "Delivered";
        //        btn.Enabled = false;
        //    }
        //    else
        //    {
        //        btn.Text = "Confirm";
        //        btn.Click += btn_Click;
        //    }
        //    btn.Location = new Point(pnl.Width - 130, 15);
        //    btn.Font = new Font("Segoe UI Semilight", 12, FontStyle.Bold);
        //    btn.Tag = itm.OrderNo;
        //    btn.Height = 35;
        //    btn.Width = 120;
        //    btn.Cursor = Cursors.Hand;
        //    btn.FlatStyle = FlatStyle.Flat;
        //    btn.FlatAppearance.BorderColor = Color.FromArgb(251, 51, 51);
        //    btn.FlatAppearance.BorderSize = 2;
        //    btn.FlatAppearance.MouseOverBackColor = Color.White;
        //    btn.FlatAppearance.MouseDownBackColor = Color.White;
        //    pnl.Controls.Add(btn);

        //    //flyLayout.Controls.Add(pnl);
        //}

        //void btn_Click(object sender, EventArgs e)
        //{
        //    if (sender is Button)
        //    {
        //        var OrderNo = (sender as Button).Tag.ToString();
        //        var itm = Program.PlacedOrders.Where(p => p.OrderNo == OrderNo).FirstOrDefault();
        //        if (itm != null)
        //        {
        //            string URL = Program.BaseUrl;
        //            string ChangeOrdStatusURL = URL + "/confirmorder?order_id=" + OrderNo + "&order_status=completed&acess_token=" + Program.Token;

        //            var GetStatus = DataProviderWrapper.Instance.GetData(ChangeOrdStatusURL, Verbs.GET, "");
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            var result = serializer.Deserialize<MessageCL>(GetStatus);
        //            itm.IsOrderConfirmed = true;
        //            BindData(false);
        //            Program.OrderCount();
        //        }
        //    }
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (this.dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    if (_gridType == EmGridType.OrderIn || _gridType == EmGridType.TakeAway)
                    {
                        if (row != null)
                        {
                            string OrderNo = row.Cells["OrderNo"].Value.ToString();
                            var SelectedOrder = Program.PlacedOrders.Where(p => p.OrderNo == OrderNo).FirstOrDefault();
                            if (e.ColumnIndex == 5)
                            {
                                if (!string.IsNullOrEmpty(OrderNo))
                                {

                                    try
                                    {
                                        frmMain.Print(PrinterSetup.GetPrinterName(EmPrinterType.CashCounter), frmMain.GetDocument(OrderNo, EmPrinterType.CashCounter));
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "Reprint Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    //if (itm != null)
                                    //{
                                    //    string URL = Program.BaseUrl;
                                    //    string ChangeOrdStatusURL = URL + "/confirmorder?order_id=" + OrderNo + "&order_status=completed&acess_token=" + Program.Token;

                                    //    var GetStatus = DataProviderWrapper.Instance.GetData(ChangeOrdStatusURL, Verbs.GET, "");
                                    //    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    //    var result = serializer.Deserialize<MessageCL>(GetStatus);
                                    //    if (result.status)
                                    //    {
                                    //        itm.IsOrderConfirmed = true;
                                    //        itm.OrderStatus = EmOrderStatus.Delivered;
                                    //        BindData(false, EmOrderType.Delivery);
                                    //        
                                    //    }
                                    //}
                                    //Program.OrderCount();
                                }
                            }
                            else if (e.ColumnIndex == 4)
                            {
                                string URL = Program.BaseUrl;
                                string DeleteOrderUrl = URL + "/deleteDineInOrder?order_id=" + OrderNo + "&acess_token=" + Program.Token;
                                var GetStatus = DataProviderWrapper.Instance.GetData(DeleteOrderUrl, Verbs.GET, "");
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                try
                                {
                                    var result = serializer.Deserialize<MessageCL>(GetStatus);
                                    if (result.status)
                                    {
                                        if (SelectedOrder != null)
                                        {
                                            Program.PlacedOrders.Remove(SelectedOrder);
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    var err = ex.Message;
                                    MessageBox.Show("Unable to delete the order!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else if (_gridType == EmGridType.Reservation)
                    {
                        if (row != null)
                        {
                            string status = row.Cells["Assign"].Value.ToString();
                            if (status.ToLower().Trim() == "assign table")
                            {
                                string TableId = row.Cells["TableId"].Value.ToString();
                                if (!string.IsNullOrEmpty(TableId))
                                {

                                    var itm = Program.Reservations.FirstOrDefault(p => p.TableId == TableId);
                                    if (itm != null)
                                    {
                                        frmAssign objAssign = new frmAssign();
                                        frmAssign.GuestCount = int.Parse(itm.GuestCount);
                                        objAssign.ShowDialog();
                                        if (!String.IsNullOrEmpty(frmAssign.TableNo))
                                        {
                                            itm.ReservationStatus = EmOrderStatus.Confirmed.ToString();
                                            itm.TableNo = frmAssign.TableNo;

                                            string URL = Program.BaseUrl;
                                            string ChangeReservationStatusURL = URL + "/changereservationstatus?res_id=" + itm.TableId + "&status=Completed&acess_token=" + Program.Token;
                                            var GetStatus = DataProviderWrapper.Instance.GetData(ChangeReservationStatusURL, Verbs.GET, "");
                                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                                            var result = serializer.Deserialize<MessageCL>(GetStatus);
                                            if (result.status)
                                            {
                                                itm.ReservationStatus = EmOrderStatus.Confirmed.ToString();
                                                itm.ActionText = "Assigned";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (_gridType == EmGridType.Delivery)
                    {
                        string OrderNo = row.Cells["OrderNo"].Value.ToString();
                        string OrderStatus = row.Cells["OrderStatus"].Value.ToString();
                        if (e.ColumnIndex == 4)
                        {
                            if (!string.IsNullOrEmpty(OrderNo))
                            {
                                var SelectedOrder = Program.PlacedOrders.Where(p => p.OrderNo == OrderNo).FirstOrDefault();
                                if (SelectedOrder != null)
                                {
                                    bool isAllowed = false;
                                    string statusUpdated = "";
                                    var _status = EmOrderStatus.Pending;
                                    string btnText = "";
                                    if (OrderStatus == "Pending")
                                    {
                                        statusUpdated = "in_progress";
                                        isAllowed = true;
                                        _status = EmOrderStatus.Confirmed;
                                        btnText = "In-Progress Status";
                                    }
                                    else if (OrderStatus == "Confirmed")
                                    {
                                        statusUpdated = "Completed";
                                        _status = EmOrderStatus.Delivered;
                                        btnText = "Delivered";
                                        isAllowed = true;
                                    }
                                    else if (OrderStatus == "Delivered")
                                    {
                                        statusUpdated = "Completed";
                                        isAllowed = false;
                                    }

                                    if (isAllowed)
                                    {
                                        string URL = Program.BaseUrl;
                                        string ChangeOrdStatusURL = URL + "confirmorder?order_id=" + OrderNo + "&order_status=" + statusUpdated + "&acess_token=" + Program.Token;

                                        var GetStatus = DataProviderWrapper.Instance.GetData(ChangeOrdStatusURL, Verbs.GET, "");
                                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                                        var result = serializer.Deserialize<MessageCL>(GetStatus);
                                        if (result.status)
                                        {
                                            SelectedOrder.IsOrderConfirmed = true;
                                            SelectedOrder.OrderStatus = _status;
                                            SelectedOrder.BtnActionStatus = btnText;
                                            //BindData(false, EmOrderType.Delivery);
                                        }
                                    }

                                }
                                Program.OrderCount(EmOrderType.Delivery);
                            }
                        }
                        else if (e.ColumnIndex == 4)
                        {

                        }
                    }
                }
            }


            if (_gridType == EmGridType.OrderIn)
                BindData(null, EmOrderType.DineIn);
            else if (_gridType == EmGridType.TakeAway)
                BindData(null, EmOrderType.TakeOut);
        }

        public void UpdateGridColumns(EmGridType type)
        {
            //Remove all Columns initially
            dataGridView1.Columns.Clear();
            _gridType = type;
            // Create Columns based on Reservations or Order-In/Delivery
            //Do the Bindings.
            if (type == EmGridType.Reservation)
            {
                //Columns
                DataGridViewTextBoxColumn txtid = new DataGridViewTextBoxColumn();
                txtid.DataPropertyName = "TableId";
                txtid.HeaderText = "ID";
                txtid.Resizable = DataGridViewTriState.False;
                txtid.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtid);
                dataGridView1.Columns[0].Name = "TableId";
                dataGridView1.Columns[0].Visible = false;

                DataGridViewTextBoxColumn txtrestrauntId = new DataGridViewTextBoxColumn();
                txtrestrauntId.DataPropertyName = "RestrauntId";
                txtrestrauntId.HeaderText = "Restaurent Id";
                txtrestrauntId.Resizable = DataGridViewTriState.False;
                txtrestrauntId.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtrestrauntId);
                dataGridView1.Columns[1].Name = "RestrauntId";
                dataGridView1.Columns[1].Visible = false;


                DataGridViewTextBoxColumn txtDinerName = new DataGridViewTextBoxColumn();
                txtDinerName.DataPropertyName = "DinerName";
                txtDinerName.HeaderText = "Diner Name";
                txtDinerName.Resizable = DataGridViewTriState.False;
                txtDinerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtDinerName);


                DataGridViewTextBoxColumn txtMobile = new DataGridViewTextBoxColumn();
                txtMobile.DataPropertyName = "MobileNo";
                txtMobile.HeaderText = "Mobile No.";
                txtMobile.Resizable = DataGridViewTriState.False;
                txtMobile.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtMobile);

                DataGridViewTextBoxColumn txtBookingDt = new DataGridViewTextBoxColumn();
                txtBookingDt.DataPropertyName = "ReservationDate";
                txtBookingDt.HeaderText = "Booking Date";
                txtBookingDt.Resizable = DataGridViewTriState.False;
                txtBookingDt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtBookingDt);

                DataGridViewTextBoxColumn txtBookingTime = new DataGridViewTextBoxColumn();
                txtBookingTime.DataPropertyName = "ReservationTime";
                txtBookingTime.HeaderText = "Booking Time";
                txtBookingTime.Resizable = DataGridViewTriState.False;
                txtBookingTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtBookingTime);

                DataGridViewTextBoxColumn txtGuestCount = new DataGridViewTextBoxColumn();
                txtGuestCount.DataPropertyName = "GuestCount";
                txtGuestCount.HeaderText = "Guest Count";
                txtGuestCount.Resizable = DataGridViewTriState.False;
                txtGuestCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtGuestCount);

                DataGridViewTextBoxColumn txtTableNo = new DataGridViewTextBoxColumn();
                txtTableNo.DataPropertyName = "TableNo";
                txtTableNo.HeaderText = "Table";
                txtTableNo.Resizable = DataGridViewTriState.False;
                txtTableNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtTableNo);

                DataGridViewTextBoxColumn txtStatus = new DataGridViewTextBoxColumn();
                txtStatus.DataPropertyName = "ReservationStatus";
                txtStatus.HeaderText = "Status";
                txtStatus.Resizable = DataGridViewTriState.False;
                txtStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtStatus);
                dataGridView1.Columns[8].Name = "ReservationStatus";

                DataGridViewButtonColumn btnAction = new DataGridViewButtonColumn();
                btnAction.DataPropertyName = "ActionText";
                btnAction.HeaderText = "Action";
                btnAction.Text = "Assign Table";
                btnAction.UseColumnTextForButtonValue = false;
                btnAction.Resizable = DataGridViewTriState.False;
                btnAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnAction);
                dataGridView1.Columns[9].Name = "Assign";

            }
            else if (type == EmGridType.OrderIn)
            {
                //Columns
                DataGridViewTextBoxColumn txtOrderNo = new DataGridViewTextBoxColumn();
                txtOrderNo.DataPropertyName = "OrderNo";
                txtOrderNo.HeaderText = "Order No";
                txtOrderNo.Resizable = DataGridViewTriState.False;
                txtOrderNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrderNo);
                dataGridView1.Columns[0].Name = "OrderNo";


                DataGridViewTextBoxColumn txtOrdertype = new DataGridViewTextBoxColumn();
                txtOrdertype.DataPropertyName = "OrderType";
                txtOrdertype.HeaderText = "Order Type";
                txtOrdertype.Resizable = DataGridViewTriState.False;
                txtOrdertype.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrdertype);
                dataGridView1.Columns[1].Name = "OrderType";

                DataGridViewTextBoxColumn txtTotal = new DataGridViewTextBoxColumn();
                txtTotal.DataPropertyName = "OrderTotal";
                txtTotal.HeaderText = "Total";
                txtTotal.Resizable = DataGridViewTriState.False;
                txtTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtTotal);
                dataGridView1.Columns[2].Name = "OrderTotal";

                //DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                //txtCol.DataPropertyName = "OrderStatus";
                //txtCol.HeaderText = "Status";
                //txtCol.Resizable = DataGridViewTriState.False;
                //txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //dataGridView1.Columns.Add(txtCol);

                DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                txtCol.DataPropertyName = "TableNo";
                txtCol.HeaderText = "Table No.";
                txtCol.Resizable = DataGridViewTriState.False;
                txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtCol);

                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.DataPropertyName = "Action";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                btnDelete.Resizable = DataGridViewTriState.False;
                btnDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnDelete);
                dataGridView1.Columns[4].Name = "Delete";

                DataGridViewButtonColumn btnPrint = new DataGridViewButtonColumn();
                btnPrint.DataPropertyName = "Action";
                btnPrint.Text = "RePrint";
                btnPrint.UseColumnTextForButtonValue = true;
                btnPrint.Resizable = DataGridViewTriState.False;
                btnPrint.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnPrint);
                dataGridView1.Columns[5].Name = "Print";

                //DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                //btnCol.DataPropertyName = "OrderStatus";
                //btnCol.HeaderText = "Status";
                //btnCol.Text = "Pending";
                //btnCol.UseColumnTextForButtonValue = true;
                //btnCol.Resizable = DataGridViewTriState.False;
                //btnCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //dataGridView1.Columns.Add(btnCol);
                //dataGridView1.Columns[3].Name = "OrderStatus";

            }
            else if (type == EmGridType.Delivery)
            {
                //Columns
                DataGridViewTextBoxColumn txtOrderNo = new DataGridViewTextBoxColumn();
                txtOrderNo.DataPropertyName = "OrderNo";
                txtOrderNo.HeaderText = "Order No";
                txtOrderNo.Resizable = DataGridViewTriState.False;
                txtOrderNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrderNo);
                dataGridView1.Columns[0].Name = "OrderNo";

                DataGridViewTextBoxColumn txtOrdertype = new DataGridViewTextBoxColumn();
                txtOrdertype.DataPropertyName = "OrderType";
                txtOrdertype.HeaderText = "Order Type";
                txtOrdertype.Resizable = DataGridViewTriState.False;
                txtOrdertype.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrdertype);

                DataGridViewTextBoxColumn txtTotal = new DataGridViewTextBoxColumn();
                txtTotal.DataPropertyName = "OrderTotal";
                txtTotal.HeaderText = "Total";
                txtTotal.Resizable = DataGridViewTriState.False;
                txtTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtTotal);

                DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                txtCol.DataPropertyName = "OrderStatus";
                txtCol.HeaderText = "Status";
                txtCol.Resizable = DataGridViewTriState.False;
                txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtCol);
                dataGridView1.Columns[3].Name = "OrderStatus";

                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                btnCol.DataPropertyName = "BtnActionStatus";
                btnCol.HeaderText = "Action";
                btnCol.Text = "Update Status";
                btnCol.UseColumnTextForButtonValue = false;
                btnCol.Resizable = DataGridViewTriState.False;
                btnCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnCol);
            }
            else if (type == EmGridType.TakeAway)
            {
                //this is work for if order type is dinein and takeout
                DataGridViewTextBoxColumn txtOrderNo = new DataGridViewTextBoxColumn();
                txtOrderNo.DataPropertyName = "OrderNo";
                txtOrderNo.HeaderText = "Order No";
                txtOrderNo.Resizable = DataGridViewTriState.False;
                txtOrderNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrderNo);
                dataGridView1.Columns[0].Name = "OrderNo";


                DataGridViewTextBoxColumn txtOrdertype = new DataGridViewTextBoxColumn();
                txtOrdertype.DataPropertyName = "OrderType";
                txtOrdertype.HeaderText = "Order Type";
                txtOrdertype.Resizable = DataGridViewTriState.False;
                txtOrdertype.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtOrdertype);
                dataGridView1.Columns[1].Name = "OrderType";

                DataGridViewTextBoxColumn txtTotal = new DataGridViewTextBoxColumn();
                txtTotal.DataPropertyName = "OrderTotal";
                txtTotal.HeaderText = "Total";
                txtTotal.Resizable = DataGridViewTriState.False;
                txtTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtTotal);
                dataGridView1.Columns[2].Name = "OrderTotal";

                DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                txtCol.DataPropertyName = "OrderStatus";
                txtCol.HeaderText = "Status";
                txtCol.Resizable = DataGridViewTriState.False;
                txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(txtCol);

                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.DataPropertyName = "Action";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                btnDelete.Resizable = DataGridViewTriState.False;
                btnDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnDelete);
                dataGridView1.Columns[4].Name = "Delete";

                DataGridViewButtonColumn btnPrint = new DataGridViewButtonColumn();
                btnPrint.DataPropertyName = "Print";
                btnPrint.Text = "Re-Print";
                btnPrint.UseColumnTextForButtonValue = true;
                btnPrint.Resizable = DataGridViewTriState.False;
                btnPrint.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns.Add(btnPrint);
                dataGridView1.Columns[5].Name = "Print";


            }

        }

        public void UpdateGridColumnType(EmGridType _type)
        {
            //if (_type == EmGridType.Reservation)
            //{
            //    //int i = 0;
            //    //while (i < dataGridView1.Rows.Count)
            //    //{
            //    //    var _row = dataGridView1.Rows[i];
            //    //    //var _allCells = dataGridView1.Rows[i].Cells;
            //    //    //_allCells.RemoveAt(6);
            //    //    //DataGridViewCell[] _cellList = null;
            //    //    var _cell = _row.Cells[6];
            //    //    if (_cell.Value.ToString() == "Pending")
            //    //    {
            //    //        var TextCell = new DataGridViewTextBoxCell();
            //    //        TextCell.Value = _row.Cells[6].Value;
            //    //        _cell.Value = TextCell;
            //    //        //_allCells.Add(TextCell);

            //    //        //_allCells.CopyTo(_cellList, 0);
            //    //        //dataGridView1.Rows[i].Cells.Clear();
            //    //        //dataGridView1.Rows[i].Cells.AddRange(_cellList);
            //    //    }
            //    //}

            //    dataGridView1.Rows[0].Cells[6].ReadOnly = true;
            //    dataGridView1.Rows[0].Cells.Remove(dataGridView1.Rows[0].Cells[6]);
            //}

        }
    }


}
