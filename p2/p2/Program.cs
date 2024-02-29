using p2;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;


static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
string url = "https://learn.malkabruk.co.il/";
string html = await Load(url);
string d = "<html>" +
                "<div id=\"1\" class=\"class1 cl2\" name=\"miriam\">" +
                     "<h3>" +
                        "header" +
                     "</h3>" +
                     "inner" +
                     "<input/>" +
                     "<div>" +
                        "<h3 id=\"ff\">" +
                        "</h3>" +
                     "</div>" +
                "</div>" +
            "</html>";
var cleanHTML = new Regex("^\\s*$").Replace(html, "");//מוריד שורות ריקות
//var cleanHTML = new Regex("\\n").Replace(c, "");//מוריד שורות ריקות
List<string> htmlLines = new Regex("<(.*?)>").Split(cleanHTML).Where(x => x.Length > 0).ToList();
//var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(d);//מכין תכונות לאוביקט מסויים
HtmlElement root = new HtmlElement();
HtmlElement currentElement = root;
static HtmlElement CreateHtmlElement(string s)
{
    HtmlElement htmlElement = new HtmlElement();
    Converter<Match, string> convert = x => x.ToString();
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(s).ToList().ConvertAll(convert);
    foreach (var attribute in attributes)
    {
        if (attribute.StartsWith(("class=")))
        {
            // טיפול ב-class
            List<string> classes = attribute.Split('=').ToList();
            htmlElement.Classes = classes[1].Split(' ').ToList();
        }
    }
    //טיפול בתכונות
    htmlElement.Attributes = attributes.ToList().FindAll(x => !x.StartsWith("class="));
    //טיפול בid
    string ss = attributes.Find(x => x.StartsWith("id"))?.Split("=")?.ToList()[1];
    htmlElement.Id = ss;
    //טיפול בשם
    htmlElement.Name = s.Split(' ')[0];
    ;
    return htmlElement;
}
foreach (var item in htmlLines)
{
    if (item.StartsWith("/html"))
        break;
    else if (item.StartsWith("/"))
    {
        // סימן שמדובר בתגית סוגרת - עלי לעלות רמה למעלה בעץ
        if (currentElement?.Parent != null)
            currentElement = currentElement?.Parent;
    }
    else if (item.EndsWith("/"))
    {
        HtmlElement newElement=CreateHtmlElement(item);
        if (currentElement.Children == null)
            currentElement.Children = new List<HtmlElement>();
        newElement.Parent = currentElement;
        currentElement.Children.Add(newElement);
    }
    else if (HtmlHelper.htmlHelper.HtmlTags.Contains(item.Split(' ')[0])|| HtmlHelper.htmlHelper.HtmlVoidTags.Contains(item.Split(' ')[0]))
    {
        HtmlElement newElement = CreateHtmlElement(item);
        // הוספת האלמנט החדש לרשימת הילדים של האלמנט הנוכחי
        if (currentElement?.Children == null)
            currentElement.Children = new List<HtmlElement>();
        newElement.Parent = currentElement;
        currentElement.Children.Add(newElement);
        // עדכון האלמנט הנוכחי להיות האלמנט החדש
        currentElement = newElement;
    }
    else if(currentElement!=null)
        currentElement.InnerHtml= item;
}

//var h = root.Children[0].Children[0].Children[0];
var x=root.FindElements(Selector.Parse("div"));