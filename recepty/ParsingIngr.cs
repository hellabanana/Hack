using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using AngleSharp;
using System.Data.Entity;
using recepty;

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
            HashSet<string> gg = new HashSet<string>();

            

            var sss = new AngleSharp.Parser.Html.HtmlParser();

            var config = Configuration.Default.WithDefaultLoader();

            var context = BrowsingContext.New(config);

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
                catch (Exception ex) { continue; }
            }

            Baza db = new Baza();
            Ingridients[] ing = new Ingridients[gg.Count];
            int k = 0;

            foreach (var cgus in gg)
            {

                try
                {
                    var document = sss.Parse(DownloadPage(cgus));
                    ing[k] = new Ingridients();
                    ing[k].Ing_Name = document.QuerySelector("img.retina_redy").GetAttribute("alt");
                    ing[k].ING_Price=double.Parse( Convert_Price( document.QuerySelector("div.price").TextContent));
                    ing[k].Ing_Weight=     document.QuerySelector("small.kg").TextContent;
                    ing[k].Ing_Id = k;
                    db.Ingridients.Add(ing[k]);
                    k++;
                }
                catch (Exception) { continue; }
               
            }
            db.SaveChanges();



           
               
            
        }
    }
}
