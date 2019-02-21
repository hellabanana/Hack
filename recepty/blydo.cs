using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recepty
{
    class blydo
    {
        public string BlydoName { get; set; }
        public string BlydoPicture { get; set; }
        public string BlydoIngredienty { get; set; }
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
}
