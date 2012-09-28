using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Integration.HtmlAgilityPack
{
    public static class HtmlNodeExtensions
    {
        public static string GetText(this HtmlNode node, string elementName)
        {
            return node.Descendants(elementName).Select(n => n.InnerText).FirstOrDefault();
        }
    }
}
