using System;
using System.Windows.Forms;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using System.Drawing;
using System.Collections.Generic;

namespace recepty
{
    public partial class Form1 : Form
    {
        List<Recipe> list_recp = new List<Recipe>();
        int Check = 0;
        public Form1()
        {
            InitializeComponent();
            Settings.Setup();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            list_recp.Clear();
            
            try
            {
                Baza baza = new Baza();
                int nm = 0;

                Recipe recipe = new Recipe();
                foreach (var item in baza.Recipe)
                {
                    if (double.Parse(textBox1.Text) >= item.Rec_Price) { nm = item.Rec_Id; list_recp.Add(item); }
                   

                }
                label1.Text = $"1/{list_recp.Count}";
                if (nm > 0)

                {
                    label3.Text = baza.Recipe.Find(nm).Rec_Name;

                    pictureBox1.Image = Image.FromFile(baza.Recipe.Find(nm).Rec_Name + "0" + ".png");
                    Vyvod_SP.DynamicRecepy(new System.Drawing.Point(10, 348), this, baza.Recipe.Find(nm));
                    label2.Text = "Cтоимость: " + baza.Recipe.Find(nm).Rec_Price.ToString("F2") + " руб";
                    button3.Enabled = true;



                }
            }catch (Exception) {toolStripStatusLabel1.Text = "Ошибка!"; }
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
            //toolStripStatusLabel1.Text="Обновление ингридиентов";
            //await Task.Run(() => Ingr_Parsing.Ingr.Ingr_Start_Parsing(toolStripStatusLabel1));
            toolStripStatusLabel1.Text = "Обновление рецептов";
               await Task.Run(() => ParsingBlydo.Start_Parse(toolStripStatusLabel1));
        



              
            }catch (Exception) { toolStripStatusLabel1.Text = "Ошибка! Проверьте соединение с интернетом"; }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (Check == 0) { button2.Enabled = false; }
            else
            {

                
                    Check--;
                    label1.Text = $"{Check+1}/{list_recp.Count}";
                label3.Text = list_recp[Check].Rec_Name;
                pictureBox1.Image = Image.FromFile(list_recp[Check].Rec_Name + "0" + ".png");
                label2.Text = "Cтоимость: " + list_recp[Check].Rec_Price.ToString("F2") + " руб";
                Vyvod_SP.DynamicRecepy(new System.Drawing.Point(10, 348), this, list_recp[Check]);
                
            
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Check++;
            if (Check >= list_recp.Count) {
                Check = 0;
                label1.Text = $"1/{list_recp.Count}";
                label3.Text = list_recp[Check].Rec_Name;
                label2.Text = "Cтоимость: " + list_recp[Check].Rec_Price.ToString("F2")+" руб";
                pictureBox1.Image = Image.FromFile(list_recp[Check].Rec_Name + "0" + ".png");
                Vyvod_SP.DynamicRecepy(new System.Drawing.Point(10, 348), this, list_recp[0]);
            }

            else
            {
                label1.Text = $"{Check+1}/{list_recp.Count}";
                button2.Enabled = true;
                label3.Text = list_recp[Check].Rec_Name;
                label2.Text = "Cтоимость: "+list_recp[Check].Rec_Price.ToString("F2") + " руб";
                pictureBox1.Image = Image.FromFile(list_recp[Check].Rec_Name + "0" + ".png");
                Vyvod_SP.DynamicRecepy(new System.Drawing.Point(10, 348), this, list_recp[Check]);
            }
        }
    }
}
