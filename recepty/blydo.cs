using System.Collections.Generic;

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

                vs.Add(IngName[i].TextContent+" "+Count[i].TextContent);
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
            

            foreach (var ing in ingridients)
            {
                for (int i = 0; i < blyd.GetIng().Count; i++)
                {
                    if (List_Ing[i].Substring(0, 5) == ing.Ing_Name.Substring(0,5)) {
                        Summa += ing.ING_Price;
                        break;

                    }
 
                }
                           
            }
                return Summa;

        }



    }
   
}
