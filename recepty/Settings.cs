namespace recepty
{
    class Settings
    {
      public  static void Setup() {

            blydo_BD.start_parse = 110;
            blydo_BD.end_parse = 120;
            Ingr_Parsing.Ingr.Start_blydo_number = 133;
            Ingr_Parsing.Ingr.End_blydo_number = 0;
            
        }
    }
}
