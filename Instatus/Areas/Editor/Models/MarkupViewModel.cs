using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using System.Web.Mvc;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class MarkupViewModel<T> : BaseViewModel<T> where T : Page
    {
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Head { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }  

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Tracking { get; set; }        
        
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Scripts { get; set; }

        public override void Load(T model)
        {
            var markupSections = model.Document.Parts.OfType<WebSection>().Where(p => p.ViewName == PartsExtensions.MarkupViewName).ToList();

            if (!markupSections.IsEmpty())
            {
                markupSections.Where(m => m.Zone == WebZone.Head).ForFirst(m => Head = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Body).ForFirst(m => Body = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Tracking).ForFirst(m => Tracking = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Scripts).ForFirst(m => Scripts = m.Body);
            }
        }

        public override void Save(T model)
        {
            model.Document.Parts.RemoveAll(p => p.ViewName == PartsExtensions.MarkupViewName);

            model.Document.Parts
                            .AddMarkupPart(WebZone.Head, Head)
                            .AddMarkupPart(WebZone.Body, Body)
                            .AddMarkupPart(WebZone.Tracking, Tracking)
                            .AddMarkupPart(WebZone.Scripts, Scripts);
        }
    }

    internal static class PartsExtensions
    {
        public const string MarkupViewName = "Markup";
        
        public static List<WebPart> AddMarkupPart(this List<WebPart> parts, WebZone zone, string body)
        {
            if (!body.IsEmpty())
            {
                parts.Add(new WebSection()
                {
                    Zone = zone,
                    ViewName = MarkupViewName,
                    Body = body
                });
            }
            
            return parts;
        }
    }
}