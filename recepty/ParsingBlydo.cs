using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace recepty
{
  abstract  class ParsingBlydo
    {

      static  blydo blydo = new blydo();

        #region Парсинг инфы из страницы
    static  public async void PageParsing(int PageNumber)
        {
           
                HtmlParser parser = new HtmlParser();
                AngleSharp.Dom.Html.IHtmlDocument document = await parser.ParseAsync(DownloadPage("https://e-dostavka.by/recipe/hot/" + PageNumber + ".html"));
                blydo.BlydoName = document.QuerySelector("h1").TextContent; //название блюда
                blydo.BlydoPicture = document.QuerySelector("img.retina_redy").GetAttribute("src"); //картинка
                blydo.IngName = document.QuerySelectorAll("li.not_in_cart a");
                blydo.Count = document.QuerySelectorAll("li.not_in_cart span");
                blydo.BlydoSP_Picture = document.QuerySelectorAll("a.fancy_img");
                blydo.BlydoSposobPrigotovleniya = document.QuerySelectorAll("a.fancy_img");
          



        }
        #endregion

        #region Скачивание_страницы
     static   public string DownloadPage(string url)
        {
            
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
            
            
            return webClient.DownloadString(url);
        }
        #endregion

        static public async void Start_Parse(ToolStripStatusLabel tl) {
            Baza db = new Baza();
            Recipe recipe = new Recipe();
            WebClient wc = new WebClient();
            Regex reg = new Regex(@"\....$");
            tl.Text = "Удаление старых данных";
            List<Recipe> list_Rec = new List<Recipe>();
            foreach (var item in db.Cooking_Method)
            {
                db.Cooking_Method.Attach(item);
                db.Cooking_Method.Remove(item);


            }
            foreach (var item in db.Recipe)
            {
                db.Recipe.Attach(item);
                db.Recipe.Remove(item);


            }

            for (int i = blydo_BD.start_parse; i < blydo_BD.end_parse; i++)
            {
                tl.Text = $"Обновлене: {i} из {blydo_BD.end_parse}";

                await Task.Run(() => PageParsing(i));
                int Counter = 0;

                list_Rec.Add(new Recipe());
                

                foreach (var k in blydo.GetIng())
                {
                    
                    
                    list_Rec[Counter].Ing_List += k;
                    

                }
                try
                {
                    wc.DownloadFile(blydo.BlydoPicture, blydo.BlydoName + "0" + ".png");
                }
                catch { continue; }
                list_Rec[Counter].Rec_Pic = blydo.BlydoName + "0" + ".png" + " * ";
                int number = 1;
                foreach (var item in blydo.BlydoSP_Picture)
                {
                    wc.DownloadFile("https://e-dostavka.by"+item.GetAttribute("href"), blydo.BlydoName + Convert.ToString(number) +".png");
                    list_Rec[Counter].Rec_Pic += blydo.BlydoName + Convert.ToString(number) + ".png" + " * ";
                    number++;
                }
                list_Rec[Counter].Ingr_Count = blydo.GetIng().Count.ToString();
             
                list_Rec[Counter].Rec_Name = blydo.BlydoName;
                

                list_Rec[Counter].Cooking_Method = new Cooking_Method();


                
                foreach (var k in blydo.BlydoSposobPrigotovleniya)
                {
                  
                    list_Rec[Counter].Cooking_Method.Cooking_Order += k.GetAttribute("data-body").Replace("&nbsp;", "")+" * ";
                  
                   

                }
                
                db.Cooking_Method.Add(list_Rec[Counter].Cooking_Method);

                list_Rec[Counter].Rec_Price = blydo_BD.Blydo_Price(blydo);
                
                foreach (var item in list_Rec)
                {
                    if (item.Ing_List!=null)
                    db.Recipe.Add(item);

                }
                db.SaveChanges();
              
               
            }
            tl.Text = "Готово!";

        }
    }
}
