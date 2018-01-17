using App_BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestariTerrace.Forms
{
    public partial class PrinterSetup : Form
    {
        public static string filePath = Path.Combine(Environment.CurrentDirectory, "Printer.txt");
        public PrinterSetup()
        {
            InitializeComponent();
        }

        private void btnPrinterSave_Click(object sender, EventArgs e)
        {
            bool isExist = File.Exists(filePath);
            try
            {
                using (StreamWriter writer = new StreamWriter("Printer.txt"))
                {
                    writer.WriteLine("Kitchen$" + txtKitchen.Text.Trim());
                    writer.WriteLine("CashCounter$" + txtCashPrinter.Text.Trim());
                }
                MessageBox.Show("Printer Setup done successfully", "Printer Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Printer Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrinterSetup_Load(object sender, EventArgs e)
        {
            
            try
            {
                
                FileInfo _fileinfo = new FileInfo(filePath);
                if(_fileinfo.Exists)
                {
                    string[] lines = File.ReadAllLines(filePath);
                    txtKitchen.Text = lines[0].Split('$')[1];
                    txtCashPrinter.Text = lines[1].Split('$')[1];
                }
                else
                {
                    MessageBox.Show("Printer Setting Not found", "Printer Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Printer Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string GetPrinterName(EmPrinterType printerType)
        {
            string result = string.Empty;
            try
            {
                FileInfo _fileinfo = new FileInfo(filePath);
                if (_fileinfo.Exists)
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if(printerType == EmPrinterType.Kitchen)
                    {
                        result = lines[0].Split('$')[1];
                    }
                    else if(printerType == EmPrinterType.CashCounter)
                    {
                        result = lines[1].Split('$')[1];
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
