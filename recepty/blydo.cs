using System.Collections.Generic;
using System.Windows.Forms;

namespace recepty
{
    class blydo
    {
        public string BlydoName { get; set;}
        public string BlydoPicture { get; set;}
        public string BlydoIngredienty { get; set;}
        public AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> BlydoSposobPrigotovleniya { get; set; }
        public AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> BlydoSP_Picture { get; set; }
        public AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> IngName { get; set; }
        public AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement>  Count {get;set ;} 


        public List<string> GetIng() {
            List<string> vs = new List<string>();
            for (int i = 0; i < IngName.Length; i++) {
                
                vs.Add(IngName[i].TextContent+" "+Count[i].TextContent +" * ");
            }
            return vs;
        }
        
    }

    class blydo_BD {

        public static int start_parse { get; set; }
        public static int end_parse { get; set; }
        public static double Blydo_Price(blydo blyd) {
            Baza bd = new Baza();
            double Summa = 0;
            System.Data.Entity.DbSet<Ingridients> ingridients = bd.Ingridients;
            List<string> List_Ing = blyd.GetIng();
            List<string> in_Price = new List<string>();
           
            int Pos = 0;
            

            foreach (var ing in ingridients)
            {
                
                Pos = 0;
                foreach (var k in in_Price) {
                    if (k.Substring(0, 5) == ing.Ing_Name.Substring(0, 5)) { Pos++; break; }

                }
          



                if (Pos == 0)
                {

                    for (int i = 0; i < blyd.GetIng().Count; i++)
                    {

                        if (List_Ing[i].Substring(0, 5) == ing.Ing_Name.Substring(0, 5))
                        {
                            Summa += ing.ING_Price;
                           
                            Pos = i;
                            in_Price.Add(List_Ing[i].Substring(0, 5));
                            break;

                        }

                    }
                }
                
                }

            return Summa;

        }
               

        }



    }
   

