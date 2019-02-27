using System;
using System.Collections.Generic;
using System.Net.Http;
using AngleSharp;
using recepty;
using System.Linq;

namespace Ingr_Parsing
{
    class Ingr
    {
      public  static int Start_blydo_number { get; set; }
      public  static int End_blydo_number { get; set; }
      public  static string Convert_Price( string c) {
            unsafe {
                fixed(char *p= c)
                {

                    for (int i = 0; i < c.Length; i++)

                    {
                      
                        if ((p[i] == 'к') || (p[i] == 'р') ){

                            for (int z = i; z < c.Length-1; z++)
                            {
                                p[z]=p[z+1];

                            }
                           

                        }

                    }
                    p[c.Length - 3] = '0';

                    p[c.Length - 2] = '0';
                    
                    p[c.Length - 1] = '0';

                    p[c.Length ] = '0';





                }
            }
            return c.Replace('.',',');
        }
      private static string DownloadPage(string url)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            HttpContent content = response.Content;
            string reply = content.ReadAsStringAsync().Result;          
            return reply;
        }
      public static  void Ingr_Start_Parsing() {
            #region переменные
            HashSet<string> gg = new HashSet<string>();
            Baza db = new Baza();
            AngleSharp.Parser.Html.HtmlParser sss = new AngleSharp.Parser.Html.HtmlParser();
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            Ingridients[] ing = new Ingridients[gg.Count];
            int k = 0;
            List<Ingridients> ingridients_List = new List<Ingridients>();
            #endregion



            for (int i = Start_blydo_number; i > End_blydo_number; i--)
            {
                try
                {
                    var document = sss.Parse(DownloadPage("https://e-dostavka.by/recipe/" + i + ".html"));
                    var file = document.QuerySelectorAll("li.not_in_cart a");

                    for (int iss = 0; iss < file.Length; iss++)
                    {
                        gg.Add(file[iss].GetAttribute("href"));
                        
                    }
                }
                catch (Exception ) { continue; }
            } 
            foreach (var item in db.Ingridients) {
                db.Ingridients.Attach(item);
                db.Ingridients.Remove(item);

            }
            foreach (var cgus in gg)
            {

                try
                {
                    var document = sss.Parse(DownloadPage(cgus));
                    ingridients_List.Add(new Ingridients());
                    ingridients_List[k].Ing_Name = document.QuerySelector("img.retina_redy").GetAttribute("alt");
                    ingridients_List[k].ING_Price = double.Parse(Convert_Price(document.QuerySelector("div.price").TextContent));
                    ingridients_List[k].Ing_Weight = document.QuerySelector("small.kg").TextContent;
                    ingridients_List[k].Ing_Id = k;
                    k++;
                }
                catch (Exception) { continue; }
               
            }
            var sortedList = ingridients_List.GroupBy(f => f.Ing_Name).Select(d => d.First());
            k = 0;
            foreach (var sorted in sortedList)
            {
                try
                {
                    if (sorted.Ing_Name != null)
                    {
                        ing[k] = new Ingridients();
                        ing[k].Ing_Name = sorted.Ing_Name;
                        ing[k].ING_Price = sorted.ING_Price;
                        ing[k].Ing_Weight = sorted.Ing_Weight;
                        ing[k].Ing_Id = k;
                        db.Ingridients.Add(ing[k]);
                        k++;
                    }
                }
                catch (Exception) { continue; }
            }
            db.SaveChanges();
            
        }
    }
}
