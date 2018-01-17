using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App_BAL
{
    public class DataTransfer
    {

    }

    public enum EmPrinterType
    {
        Kitchen,
        CashCounter
    }

    public enum EmOrderStatus
    {
        Pending = 1,
        InProgress = 4,
        Confirmed = 2,
        Delivered = 3
    }
    public enum EmOrderType
    {
        TakeOut = 1,
        Delivery = 2,
        DineIn = 3,
        Reservation = 4,
        CounterSale=5
    }
    public enum EmPaymentType
    {
        Cash = 1,
        Card = 2,
        Wallet = 3
    }

    public enum EmDiscountType
    {
        Amount,
        Percent
    }

    public enum EmGridType
    {
        Reservation,
        OrderIn,
        TakeAway,
        Delivery
    }

    public class PrintLine
    {
        public int LineNo { get; set; }
        public string LineText { get; set; }
    }
    public class ReservationCL : INotifyPropertyChanged
    {
        private string ReservationStatusValue;
        private string TableNoValue;
        private string BtnText;
        public string TableId { get; set; }
        public string RestrauntId { get; set; }
        public string DinerName { get; set; }
        public string MobileNo { get; set; }
        public string ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public string GuestCount { get; set; }
        public DateTime PlaceDate
        {
            get; set;
        }
        public string ReservationStatus
        {
            get
            {
                return this.ReservationStatusValue;
            }
            set
            {
                if (value != this.ReservationStatusValue)
                {
                    this.ReservationStatusValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public string ActionText
        {
            get
            {
                return this.BtnText;
            }
            set
            {
                if (value != this.BtnText)
                {
                    this.BtnText = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        public string TableNo
        {
            get
            {
                return this.TableNoValue;
            }
            set
            {
                if (value != this.TableNoValue)
                {
                    this.TableNoValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class CartCL : INotifyPropertyChanged
    {
        private EmOrderStatus _OrderStatus { get; set; }
        public string BtnActionStatus { get; set; }
        public int SrNo { get; set; }
        public int OrderID { get; set; }
        public string TableNo { get; set; }
        public string OrderNo { get; set; }
        public string OrderTotal { get; set; }
        public double DiscountAmt { get; set; }
        public string DiscountType { get; set; }
        public string OrderRemarks { get; set; }
        public EmOrderType OrderType { get; set; }
        public BindingList<CartItemsCL> Items { get; set; }
        public bool IsOrderConfirmed { get; set; }
        public EmOrderStatus OrderStatus
        {
            get
            {
                return this._OrderStatus;
            }

            set
            {
                this._OrderStatus = value;
                this.NotifyPropertyChanged();
            }
        }
        public EmPaymentType PaymentType { get; set; }
        public bool IsCurrentOrder { get; set; }
        public string TransactionID { get; set; }
        public CartCL()
        {
            Items = new BindingList<CartItemsCL>();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class PlaceOrderBinding : INotifyPropertyChanged
    {
        private int _orderCount = 0;
        private double _sumDineInConfirmedAmountTotal = 0.00;
        private double _sumDineInUnconfirmedAmountTotal = 0.00;
        private double _sumTakeAwayConfirmedAmountTotal = 0.00;
        private double _sumTakeAWayUnconfirmedAmountTotal = 0.00;
        private double _sumDeliveryConfirmedAmountTotal = 0.00;
        private double _sumDeliveryUnconfirmedAmountTotal = 0.00;
        private double _sumReservationConfirmedAmountTotal = 0.00;
        private double _sumReservationDeliveryUnconfirmedAmountTotal = 0.00;
        public int OrderCount
        {
            get
            {
                return this._orderCount;
            }
            set
            {
                if (value != this._orderCount)
                {
                    this._orderCount = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumDineInConfirmedAmountTotal
        {
            get
            {
                return this._sumDineInConfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumDineInConfirmedAmountTotal)
                {
                    this._sumDineInConfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumDineInUnconfirmedAmountTotal
        {
            get
            {
                return this._sumDineInUnconfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumDineInUnconfirmedAmountTotal)
                {
                    this._sumDineInUnconfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumTakeAwayConfirmedAmountTotal
        {
            get
            {
                return this._sumTakeAwayConfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumTakeAwayConfirmedAmountTotal)
                {
                    this._sumTakeAwayConfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumTakeAwayUnconfirmedAmountTotal
        {
            get
            {
                return this._sumTakeAWayUnconfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumTakeAWayUnconfirmedAmountTotal)
                {
                    this._sumTakeAWayUnconfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumDeliveryConfirmedAmountTotal
        {
            get
            {
                return this._sumDeliveryConfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumDeliveryConfirmedAmountTotal)
                {
                    this._sumDeliveryConfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumDeliveryUnconfirmedAmountTotal
        {
            get
            {
                return this._sumDeliveryUnconfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumDeliveryUnconfirmedAmountTotal)
                {
                    this._sumDeliveryUnconfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumReservationConfirmedAmountTotal
        {
            get
            {
                return this._sumReservationConfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumReservationConfirmedAmountTotal)
                {
                    this._sumReservationConfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double SumReservationUnconfirmedAmountTotal
        {
            get
            {
                return this._sumReservationDeliveryUnconfirmedAmountTotal;
            }
            set
            {
                if (value != this._sumReservationDeliveryUnconfirmedAmountTotal)
                {
                    this._sumReservationDeliveryUnconfirmedAmountTotal = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CartItemsCL : INotifyPropertyChanged
    {
        //Used in Case of Counter Sales
        public bool IsCounterSale { get; set; }
        public string Description { get; set;}
        public decimal SaleTotal { get; set; }
        private int QuantityValue = 0;
        private double PriceValue = 0;
        private double CartTotalValue = 0;
        private double GrandTotalValue = 0;
        private double TaxValue = 0;
        public string orderNo { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public string FoodType { get; set; }
        public double OriginalPrice { get; set; }
        public double Price
        {
            get
            {
                return this.PriceValue;
            }
            set
            {
                if (value != this.PriceValue)
                {
                    this.PriceValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        public int Quantity
        {
            get
            {
                return this.QuantityValue;
            }
            set
            {
                if (value != this.QuantityValue)
                {
                    this.QuantityValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        public double CartTotal
        {
            get
            {
                return this.CartTotalValue;
            }
            set
            {
                if (value != this.CartTotalValue)
                {
                    this.CartTotalValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double GrandTotal
        {
            get
            {
                return this.GrandTotalValue;
            }
            set
            {
                if (value != this.GrandTotalValue)
                {
                    this.GrandTotalValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        public double Tax
        {
            get
            {
                return this.TaxValue;
            }
            set
            {
                if (value != this.TaxValue)
                {
                    this.TaxValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CalculateCart : INotifyPropertyChanged
    {
        private double CartTotalValue = 0;
        private double GrandTotalValue = 0;
        private double TaxValue = 0;
        public double CartTotal
        {
            get
            {
                return this.CartTotalValue;
            }
            set
            {
                if (value != this.CartTotalValue)
                {
                    this.CartTotalValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public double GrandTotal
        {
            get
            {
                return this.GrandTotalValue;
            }
            set
            {
                if (value != this.GrandTotalValue)
                {
                    this.GrandTotalValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        public double Tax
        {
            get
            {
                return this.TaxValue;
            }
            set
            {
                if (value != this.TaxValue)
                {
                    this.TaxValue = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CategoryListCL
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductListCL
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int CategoryID { get; set; }
        public string ImageName { get; set; }
        public string ProductNumber { get; set; }
    }

    public enum Verbs
    {
        GET,
        POST,
        PUT
    }

    public class MessageCL
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string order_status { get; set; }
    }

    public class LoginCL
    {
        public bool status { get; set; }
        public string data { get; set; }
        public string message { get; set; }
        public string sessionid { get; set; }
        public string outlet_Type { get; set; }
    }

    public class CategoryCL
    {
        public bool status { get; set; }
        public Object data { get; set; }
        public string message { get; set; }
    }

    public class ProductCL
    {
        public string id { get; set; }
        public string restaurent_id { get; set; }
        public string tanent_id { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string food_type { get; set; }
        public string description { get; set; }
        public string product_image { get; set; }
        public string created { get; set; }
        public string order_type { get; set; }
        public string product_code { get; set; }
    }

    public class RestaurantCL
    {
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurant_name { get; set; }
        public string restaurant_type { get; set; }
        public string location { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string min_item_price { get; set; }
        public string min_order_price { get; set; }
        public string order_type { get; set; }
        public string shipping_charge { get; set; }
        public string delivery_time { get; set; }
        public string delivery_format { get; set; }
        public string payment_types { get; set; }
        public string restaurant_image { get; set; }
        public string created { get; set; }
        public string delivery_hours { get; set; }
        public string reserve_table { get; set; }
        public string closing_day { get; set; }
        public object supported_payment_types { get; set; }
        public object ref_url { get; set; }
    }

    public class Datum
    {
        public ProductCL Product { get; set; }
        public RestaurantCL Restaurant { get; set; }
        public List<object> Productimage { get; set; }
        public Tableorder Tableorder { get; set; }
        public Order Order { get; set; }
        public List<CartProduct> Cart_products { get; set; }
        public Orderdetail Orderdetail { get; set; }
        public Datum()
        {
            Cart_products = new List<CartProduct>();
            Orderdetail = new Orderdetail();
        }
    }

    public class ProductListAPICL
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
        public string message { get; set; }
    }

    public class Tableorder
    {
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurent_id { get; set; }
        public string user_id { get; set; }
        public string diner_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string date { get; set; }
        public string from_time { get; set; }
        public string to_time { get; set; }
        public string guests { get; set; }
        public object special_request { get; set; }
        public string terms_cond { get; set; }
        public string status { get; set; }
        public string created { get; set; }
        public string bookingDate
        {
            get
            {
                return date + Environment.NewLine + from_time + " - " + to_time;
            }
        }
    }

    public class ReservationListAPICL
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class TransactionAPICL
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string orderid { get; set; }
    }

    public class logoutCL
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    #region PendingOrders
    public class Orderdetail
    {
        public Orderdetail() { }
        public string order_id { get; set; }
        public string total { get; set; }
        public string order_status { get; set; }
        public string order_type { get; set; }
        public string product_id { get; set; }
        public string id { get; set; }
        public object transaction_id { get; set; }
        public string restaurant_id { get; set; }
        public string tanent_id { get; set; }
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string adress { get; set; }
        public object payment_type { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string email { get; set; }
        public string contact_number { get; set; }
        public string cart_subtotal { get; set; }
        public string country { get; set; }
        public string shipping { get; set; }
        public string payment_mode { get; set; }
        public string payment_status { get; set; }
        public object bar_code { get; set; }
        public string created { get; set; }
        public object currency { get; set; }
        public List<Cart> cart { get; set; }
    }

    public class Order
    {
        public Orderdetail Orderdetail { get; set; }
        public Order()
        {
            Orderdetail = new Orderdetail();
        }
    }

    public class Cart2
    {
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurent_id { get; set; }
        public string product_id { get; set; }
        public object product_price { get; set; }
        public string user_id { get; set; }
        public string ip_adress { get; set; }
        public string quantity { get; set; }
        public string delivery_type { get; set; }
        public string total_price { get; set; }
        public string status { get; set; }
        public string created { get; set; }
    }

    public class Cart
    {
        public Cart()
        {
            cart = new Cart2();
            Restaurant = new Restaurant();
            Product = new Product();
            User = new User();
        }
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurent_id { get; set; }
        public string product_id { get; set; }
        public string user_id { get; set; }
        public string ip_adress { get; set; }
        public string quantity { get; set; }
        public string total_price { get; set; }
        public string delivery_type { get; set; }
        public string status { get; set; }
        public string created { get; set; }
        public Cart2 cart { get; set; }
        public Restaurant Restaurant { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }

    public class Restaurant
    {
        public Restaurant() { }
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurant_name { get; set; }
        public string main_category { get; set; }
        public string restaurant_type { get; set; }
        public string gst_status { get; set; }
        public string location { get; set; }
        public string contact_no { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string min_item_price { get; set; }
        public string min_order_price { get; set; }
        public string order_type { get; set; }
        public string shipping_charge { get; set; }
        public string delivery_time { get; set; }
        public string delivery_format { get; set; }
        public string payment_types { get; set; }
        public string restaurant_image { get; set; }
        public string created { get; set; }
        public string delivery_hours { get; set; }
        public string reserve_table { get; set; }
        public string closing_day { get; set; }
        public object supported_payment_types { get; set; }
        public object ref_url { get; set; }
        public string gst_no { get; set; }
        public string address { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
        public string restaurent_id { get; set; }
        public string tanent_id { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public object total_stock { get; set; }
        public object remaining_stock { get; set; }
        public string main_category { get; set; }
        public string outlet_categories { get; set; }
        public string food_type { get; set; }
        public string description { get; set; }
        public string product_image { get; set; }
        public string created { get; set; }
        public string order_type { get; set; }
        public string product_code { get; set; }
        public Product()
        {

        }
    }

    public class User
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object company_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public object qr_code { get; set; }
        public string user_role { get; set; }
        public string device_token { get; set; }
        public string contact_number { get; set; }
        public string wallet { get; set; }
        public string adress { get; set; }
        public string acess_token { get; set; }
        public string transaction_pin { get; set; }
        public object tanent_category { get; set; }
        public string created { get; set; }
        public User()
        {

        }
    }

    public class CartProduct
    {
        public Cart Cart { get; set; }
        public Restaurant Restaurant { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public CartProduct()
        {
            Cart = new Cart();
            Restaurant = new Restaurant();
            Product = new Product();
            User = new User();
        }
    }

    public class PendingOrderAPI
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
        public string message { get; set; }
        public PendingOrderAPI()
        {
            data = new List<Datum>();
        }
    }
    #endregion

    #region PostOrder
    public class PostOrder
    {
        public string tn_id { get; set; }
        public string product_id { get; set; }
        public string quantity { get; set; }
        public int total_price { get; set; }
    }

    public class NewOrder
    {
        public string product_id { get; set; }
        public string quantity { get; set; }
        public int price { get; set; }
        public string delivery_type { get; set; }
        public string product_name { get; set; }
    }
    #endregion

    #region Store Details
    public class Employee
    {
        public string id { get; set; }
        public string restaurant_id { get; set; }
        public string tanent_id { get; set; }
        public string employee_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string access_type { get; set; }
        public string acess_token { get; set; }
        public string position { get; set; }
        public string created { get; set; }
        public string daily_cash_limit { get; set; }
        public string outlet_type { get; set; }
    }



    public class Message
    {
        public Employee Employee { get; set; }
        public Restaurant Restaurant { get; set; }
        public Message()
        {
            Employee = new Employee();
            Restaurant = new Restaurant();
        }
    }

    public class StoreDetails
    {
        public bool status { get; set; }
        public Message message { get; set; }
    }
    #endregion
}
