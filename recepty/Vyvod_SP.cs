using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace recepty
{
    class Vyvod_SP
    {
   public static void DynamicRecepy(Point startpos, Form form,Recipe recipe)
        {
            Baza baza = new Baza();
            System.Data.Entity.DbSet<Recipe> recepts = baza.Recipe;
            List<string> vs = recipe.Cooking_Method.Cooking_Order.Split('*').ToList();
            
         
            

            int count = vs.Count-1;
            
            form.Controls.RemoveByKey("Dynamic");
            

           
            Panel MainPanel = new Panel();
            MainPanel.Name = "Dynamic";
            MainPanel.Location = startpos;
            MainPanel.Height = form.Height - 410;
            MainPanel.Width = form.Width - startpos.X - 20;
            MainPanel.SendToBack();
            MainPanel.AutoScroll = true;
            form.Controls.Add(MainPanel);
            var label = new TextBox[count];
            var pictures = new PictureBox[count];
            for (int i = 0; i <count; i++)
            {
                if (i == 0)
                {
                    pictures[i] = new PictureBox();
                    pictures[i].Name = "Dynamic";
                    pictures[i].Location = new Point(1,1);
                    pictures[i].Height = 150;
                    pictures[i].Width = 200;
                    pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    pictures[i].Image = Image.FromFile(recipe.Rec_Name+(i+1)+".png");
                    label[i] = new TextBox();
                    label[i].Name = "Dynamic";
                    label[i].Multiline = true;
                    label[i].ReadOnly = true;
                    label[i].Location = new Point(pictures[i].Location.X + pictures[i].Width + 10, pictures[i].Location.Y);
                    label[i].Text = vs[i];
                    label[i].Font = new Font("SECRET FONT", 18, FontStyle.Bold);
                    label[i].Height = pictures[i].Height;
                    label[i].ScrollBars = ScrollBars.None;
                    label[i].Width = MainPanel.Width - pictures[i].Width - 10 - 10 - 40;
                    MainPanel.Controls.Add(pictures[i]);
                    MainPanel.Controls.Add(label[i]);
                   
                    continue;
                }

                pictures[i] = new PictureBox();
                pictures[i].Name = "Dynamic";
                pictures[i].Location = new Point(pictures[i - 1].Location.X, pictures[i - 1].Location.Y + pictures[i - 1].Height + 20);
                pictures[i].Height = 150;
                pictures[i].Width = 200;
                pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictures[i].Image = Image.FromFile(recipe.Rec_Name + (i+1) + ".png");
                label[i] = new TextBox();
                label[i].Name = "Dynamic";
                label[i].Multiline = true;
                label[i].ReadOnly = true;
                label[i].Location = new Point(pictures[i].Location.X + pictures[i].Width + 10, pictures[i].Location.Y);
                label[i].Text = vs[i];
                label[i].Font = new Font("SECRET FONT", 18, FontStyle.Bold);
                label[i].Height = pictures[i].Height;
                label[i].ScrollBars = ScrollBars.None;
                label[i].Width = MainPanel.Width - pictures[i].Width - 10 - 10 - 40;
                MainPanel.Controls.Add(pictures[i]);
                MainPanel.Controls.Add(label[i]);

             



            }

        }
    }
}
