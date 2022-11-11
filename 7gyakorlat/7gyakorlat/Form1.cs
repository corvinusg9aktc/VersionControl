using _7gyakorlat.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace _7gyakorlat
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();

        Excel.Application xlApp;
        Excel.Workbook xlWb;
        Excel.Worksheet xlSheet;

        public Form1()
        {
            InitializeComponent();

            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();



            List<decimal> Nyereségek = new List<decimal>();
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());

        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                xlApp = new Excel.Application();
                xlWb = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWb.ActiveSheet;

                CreateExcel();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                xlWb.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWb = null;
                xlSheet = null;
            }
        }

        private void CreateExcel()
        {
            string[] headers = new string[]
            {
                "Időszak",
                "Nyereség",
            };
            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }


            object[,] values = new object[Ticks.Count, headers.Length];


            int counter = 0;

            foreach (Tick f in Ticks)
            {
                values[counter, 0] = f.TradingDay;
                values[counter, 1] = f.Price;
                counter++;

            }
            
        }
    }
}