using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string>? Classes { get; set; }
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector Parse(string query)
        {
            // פירוק המחרוזת לחלקים לפי רווחים
            string[] parts = query.Split(' ');

            // יצירת אוביקט שורש
            Selector root = new Selector();
            Selector current = root;

            // לולאה על כל חלקי המחרוזת
            foreach (string part in parts)
            {
                // פירוק החלק לפי # ו-
                string[] subParts = part.Split('#', '.');

                // עדכון מאפייני הסלקטור הנוכחי
                foreach (string subPart in subParts)
                {
                    if (subPart.StartsWith("#"))
                    {
                        current.Id = subPart.Substring(1);
                    }
                    else if (subPart.StartsWith("."))
                    {
                        if(current.Classes==null)
                            current.Classes = new List<string>();
                        current.Classes.Add(subPart.Substring(1));
                    }
                    else
                    {
                        // בדיקה אם שם תקין של תגית HTML
                        if (IsValidHtmlTagName(subPart))
                        {
                            current.TagName = subPart;
                        }
                    }
                }

                // יצירת אוביקט סלקטור חדש והוספתו כבן
                Selector child = new Selector();
                current.Child = child;
                current = child;
            }

            return root;
        }
        private static bool IsValidHtmlTagName(string tagName)=> HtmlHelper.htmlHelper.HtmlTags.Contains(tagName) || HtmlHelper.htmlHelper.HtmlVoidTags.Contains(tagName);
        
    }
}
