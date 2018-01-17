using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public class CommonForm : Form
    {
        private Button button1;
        public static bool IsClosed = false;
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
            DialogResult msgResult = MessageBox.Show("Do you want to Exit Application ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (msgResult == DialogResult.No)
            {
                e.Cancel = true;
                IsClosed = false;
            }
            else
            {
                if(Program.IsLogined)
                {
                    frmManagerExit mgr = new frmManagerExit();
                    mgr.ShowDialog();
                    if (mgr.IsOK)
                    {
                        Program.ClearData();
                        Application.ExitThread();
                    }
                }
                else
                {
                    Program.ClearData();
                    Application.ExitThread();
                }
                IsClosed = true;
            }
            //base.OnFormClosing(e);
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::BestariTerrace.Properties.Resources.downlogo;
            this.button1.Location = new System.Drawing.Point(58, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(214, 79);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CommonForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Name = "CommonForm";
            this.ResumeLayout(false);

        }
    }
}
