using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Off_BAL
{
    //Get Data From Server
    public class ProductCL
    {
        public string id { get; set; }
        public string restaurent_id { get; set; }
        public string tanent_id { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string manage_stock { get; set; }
        public string total_stock { get; set; }
        public string remaining_stock { get; set; }
        public string main_category { get; set; }
        public string outlet_categories { get; set; }
        public string food_type { get; set; }
        public string description { get; set; }
        public string product_image { get; set; }
        public string created { get; set; }
        public string order_type { get; set; }
        public string product_code { get; set; }
    }

    public class ProductV
    {
        public ProductCL Product { get; set; }
    }

    public class Employeesession
    {
        public string id { get; set; }
        public string restaurent_id { get; set; }
        public string session_id { get; set; }
        public string session_start_time { get; set; }
        public string session_end_time { get; set; }
        public string session_status { get; set; }
        public string created { get; set; }
    }

    public class SessionEmployee
    {
        public Employeesession Employeesession { get; set; }

        public SessionEmployee()
        {
            Employeesession = new Employeesession();
        }
    }

    public class SessionuserCL
    {
        public string id { get; set; }
        public string session_id { get; set; }
        public string employee_id { get; set; }
        public string tanent_id { get; set; }
        public string restaurent_id { get; set; }
        public string emp_loggedin_time { get; set; }
        public string emp_loggedout_time { get; set; }
        public string emp_status { get; set; }
        public string created { get; set; }
    }

    public class SessionUserV
    {
        public SessionuserCL Sessionuser { get; set; }
    }

    public class OrderdetailCL
    {
        public string id { get; set; }
        public string order_id { get; set; }
        public string transaction_id { get; set; }
        public string product_id { get; set; }
        public string restaurant_id { get; set; }
        public string tanent_id { get; set; }
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string adress { get; set; }
        public string payment_type { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string email { get; set; }
        public string contact_number { get; set; }
        public string cart_subtotal { get; set; }
        public string country { get; set; }
        public string shipping { get; set; }
        public string total { get; set; }
        public string final_total { get; set; }
        public string payment_mode { get; set; }
        public string payment_status { get; set; }
        public string order_status { get; set; }
        public string discount { get; set; }
        public string discount_type { get; set; }
        public string order_type { get; set; }
        public string bar_code { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public string employee_id { get; set; }
        public string quantity { get; set; }
        public string created { get; set; }
        public string currency { get; set; }
        public string voucher_code { get; set; }
        public string voucher_discount { get; set; }
        public string voucher_value { get; set; }
    }

    public class OrderDetailV
    {
        public OrderdetailCL Orderdetail { get; set; }
    }

    public class Restaurant
    {
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurant_name { get; set; }
        public string main_category { get; set; }
        public string restaurant_type { get; set; }
        public string location { get; set; }
        public string gst { get; set; }
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
        public string supported_payment_types { get; set; }
        public object ref_url { get; set; }
        public string gst_no { get; set; }
        public string gst_value { get; set; }
        public string is_active { get; set; }
        public string last_active { get; set; }
    }

    public class RestaurentDetail
    {
        public Restaurant Restaurant { get; set; }
    }

    public class EmployeeCL
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
    }

    public class EmployeeV
    {
        public EmployeeCL Employee { get; set; }
    }

    public class Cart
    {
        public string id { get; set; }
        public string tanent_id { get; set; }
        public string restaurent_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string user_id { get; set; }
        public string ip_adress { get; set; }
        public string quantity { get; set; }
        public string delivery_type { get; set; }
        public string total_price { get; set; }
        public string status { get; set; }
        public string created { get; set; }
    }

    public class CartData
    {
        public Cart Cart { get; set; }
    }

    public class DatabaseData
    {
        public List<ProductV> products { get; set; }
        public List<SessionEmployee> sessionEmployees { get; set; }
        public List<SessionUserV> sessionUsers { get; set; }
        public List<OrderDetailV> orderDetails { get; set; }
        public List<RestaurentDetail> restaurentDetails { get; set; }
        public List<EmployeeV> employees { get; set; }
        public List<CartData> cartData { get; set; }
        public List<object> tableOrders { get; set; }
        public DatabaseData ()
        {
            products = new List<ProductV>();
            sessionEmployees = new List<SessionEmployee>();
            sessionUsers = new List<SessionUserV>();
            orderDetails = new List<OrderDetailV>();
            restaurentDetails = new List<RestaurentDetail>();
            employees = new List<EmployeeV>();
            cartData = new List<CartData>();
            tableOrders = new List<object>();
        }
    }

    public class GetData
    {
        public bool status { get; set; }
        public DatabaseData databaseData { get; set; }
        public string message { get; set; }
        public GetData()
        {
            databaseData = new DatabaseData();
        }
    }
    
}
