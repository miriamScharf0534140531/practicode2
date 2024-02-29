using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace p2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _htmlHelper=new HtmlHelper();
        public static HtmlHelper htmlHelper { get { return _htmlHelper; } }
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            string Tags = File.ReadAllText("seed/HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(Tags);
            string voidTags = File.ReadAllText("seed/HtmlTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTags);

        }
       
    }

}

