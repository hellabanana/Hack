using System;
using System.Windows.Forms;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;


namespace recepty
{
    public partial class Form1 : Form
    {
       
        blydo blydo = new blydo();
        

        #region Скачивание_страницы
        public string DownloadPage(string url) {               
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
          return  webClient.DownloadString(url);
        }
        #endregion


        #region Парсинг инфы из страницы
        public async void PageParsing(int PageNumber) {
            HtmlParser parser = new HtmlParser();
            AngleSharp.Dom.Html.IHtmlDocument document = await parser.ParseAsync(DownloadPage("https://e-dostavka.by/recipe/hot/" + PageNumber + ".html"));
            blydo.BlydoName = document.QuerySelector("div.description").TextContent; //название блюда
           blydo.BlydoPicture = document.QuerySelector("img.retina_redy").GetAttribute("src"); //картинка
            blydo.IngName= document.QuerySelectorAll("li.not_in_cart a");
            blydo.Count = document.QuerySelectorAll("li.not_in_cart span");
            blydo.BlydoSP_Picture = document.QuerySelectorAll("a.fancy_img");
            blydo.BlydoSposobPrigotovleniya = document.QuerySelectorAll("a.fancy_img");



        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            for (int i = 100 ; i < 101; i++)
            {
            
                await Task.Run(() => PageParsing(i));
                listBox1.Items.Add(blydo.BlydoName);
                dataGridView1.RowCount += 1;
                dataGridView1.Rows.Add(blydo.BlydoName, blydo.BlydoPicture);
                pictureBox1.ImageLocation = blydo.BlydoPicture;
                foreach (var k in blydo.GetIng())
                {
                    listBox1.Items.Add(k);

                }
                foreach (var k in blydo.BlydoSposobPrigotovleniya)
                {
                    listBox1.Items.Add(k.GetAttribute("data-body"));

                }
               
            }
            
        }
    }
}
