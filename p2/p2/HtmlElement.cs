using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2
{
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Attributes { get; set; }
        public List<string>? Classes { get; set; }
        public string? InnerHtml { get; set; }
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement>? Children { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
        }
        /// מחזיר רשימה של כל הצאצאים של אלמנט מסויים 
        public IEnumerable<HtmlElement> Descendants()
        {
            // יצירת תור חדש
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            // דחיפת האלמנט הנוכחי לתור
            queue.Enqueue(this);
            // לולאה כל עוד התור לא ריק
            while (queue.Count > 0)
            {
                // שליפת האלמנט הראשון מהתור
                HtmlElement element = queue.Dequeue();
                // החזרת האלמנט
                yield return element;
                // הוספת ילדי האלמנט לתור (אם קיימים)
                if (element.Children != null)
                {
                    foreach (HtmlElement child in element.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        //מחזיר רשימת אלמנטים שמקימיים את הסלקטור מהאלמנט הנוכחי
        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            foreach (var ele in Descendants())
            {
                if (ele.FindElementsRecursively(selector, result))
                    result.Add(ele);
            }
            return result;
        }
        private bool FindElementsRecursively(Selector selector, HashSet<HtmlElement> result)
        {
            if (!IsMatch(selector))
                return false;
            //אם אין לסלקטור ילדים סימן שזה מקיים את הסלקטור
            if (selector.Child == null)
                return true;
            foreach (var child in Descendants())
            {
                if (child.FindElementsRecursively(selector.Child, result))
                    return true;
            }
            return false;
        }
        private bool IsMatch(Selector selector)
        {
            bool x=false, y=false, z = false;
            if (selector?.TagName == null) x=true;
            if (Name != null&& Name.Equals(selector.TagName)) x = true;
            if (selector?.Id == null|| selector.Id.Equals(Id)) y = true;
            int xx = selector.Classes.Intersect(Classes).Count(), yy= selector.Classes.Count;
            if (selector?.Classes?.Intersect(Classes).Count() == selector?.Classes?.Count)z = true;
            return x && y && z;
            //return ((selector?.TagName == null || Name==null|| Name.Equals(selector.TagName))
            //    && (selector?.Id == null || selector.Id.Equals(Id))
            //    && (selector?.Classes?.Intersect(Classes).Count() == selector?.Classes?.Count));
        }
    }

}
