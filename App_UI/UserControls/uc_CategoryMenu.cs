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
using App_BAL;
using System.Web.Script.Serialization;
using System.Collections;

namespace BestariTerrace.UserControls
{
    public partial class uc_CategoryMenu : UserControl
    {
        public static string currentBtnName = "All";
        public uc_CategoryMenu()
        {
            InitializeComponent();
        }

        private void uc_CategoryMenu_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.VerticalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Enabled = false;
        }

        public void BindData()
        {
            flowLayoutPanel1.Controls.Clear();

            string URL = Program.BaseUrl;
            string CategoryURL = URL + "/getmenu?acess_token=" + Program.Token;

            var CategoryList = DataProviderWrapper.Instance.GetData(CategoryURL, Verbs.GET, "");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<CategoryCL>(CategoryList);

            var AllCategoryList = ((IEnumerable)result.data).Cast<object>()
                                   .Select(x => x == null ? x : x.ToString())
                                   .ToArray();

            CreateButton(0, "All");  //this is mandotry to show all items
            foreach (var item in AllCategoryList)
            {
                int CategoryID = Convert.ToInt32(item.ToString().Split(',')[0].Replace("[", ""));
                string CategoryName = item.ToString().Split(',')[1].Replace("]", "");

                CreateButton(CategoryID, CategoryName);
            }
            
            SelectedControls();
        }

        public void CreateButton(int ID, string Name)
        {
            Button btn = new Button();
            btn.Name = "btn_" + ID;
            btn.Text = Name;
            btn.Tag = ID;
            btn.AutoSize = false;
            btn.Height = flowLayoutPanel1.Height - 20;
            btn.Width = 150;
            btn.Margin = new Padding(0, -30, 0, -30);
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI Semilight", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(225, 225, 225);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.White;
            btn.FlatAppearance.MouseDownBackColor = Color.White;
            btn.Click += Btn_Click;
            btn.MouseHover += Btn_MouseHover;
            btn.MouseLeave += Btn_MouseLeave;

            btn.BackColor = Color.FromArgb(255, 255, 255);
            if (Name == "All")
            {
                btn.ForeColor = Color.FromArgb(251, 51, 51);
            }
            else
            {
                btn.ForeColor = Color.Gray;
            }
            flowLayoutPanel1.Padding = new Padding(0);
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            //Point scroll = new Point(0, 0);
            //flowLayoutPanel1.AutoScrollPosition = scroll;
            flowLayoutPanel1.AutoSize = false;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.Controls.Add(btn);

        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            SelectedControls();
        }

        private void Btn_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.ForeColor = Color.FromArgb(251, 51, 51);
        }

        public event EventHandler EventCategoryClicked;
        private void Btn_Click(object sender, EventArgs e)
        {
            Button Cur_btn = (Button)sender;
            currentBtnName = Cur_btn.Text;

            SelectedControls();

            EventCategoryClicked(Cur_btn.Text, EventArgs.Empty);

        }

        public void SelectedControls()
        {
            foreach (var control in flowLayoutPanel1.Controls)
            {
                Button btn = (Button)control;
                if (btn != null)
                {
                    if (btn.Text == currentBtnName)
                    {
                        btn.ForeColor = Color.FromArgb(251, 51, 51);
                    }
                    else
                    {
                        btn.ForeColor = Color.Gray;
                    }
                }
            }
        }

        public string GetCurrentControlBtnName()
        {
            return currentBtnName;
        }

        public void SetCurrentControlBtnName( string name)
        {
            currentBtnName = name;
        }
    }
}
