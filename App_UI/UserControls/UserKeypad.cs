using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestariTerrace.UserControls
{
    public partial class UserKeypad : UserControl
    {
        public string keyValue { get; set; }
        public UserKeypad()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            keyValue = btn.Text;
        }
    }
}
