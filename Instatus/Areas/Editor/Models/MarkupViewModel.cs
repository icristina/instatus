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
        public string Banner { get; set; }         
        
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Header { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Figure { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Menu { get; set; } 

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Aside { get; set; }   

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Footer { get; set; }   
        
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Scripts { get; set; }

        public override void Load(T model)
        {
            var markupSections = model.Document.Parts.OfType<WebMarkup>().ToList();

            if (!markupSections.IsEmpty())
            {
                markupSections.Where(m => m.Zone == WebZone.Head).ForFirst(m => Head = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Banner).ForFirst(m => Banner = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Header).ForFirst(m => Header = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Figure).ForFirst(m => Figure = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Menu).ForFirst(m => Menu = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Body).ForFirst(m => Body = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Aside).ForFirst(m => Aside = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Footer).ForFirst(m => Footer = m.Body);
                markupSections.Where(m => m.Zone == WebZone.Scripts).ForFirst(m => Scripts = m.Body);
            }
        }

        public override void Save(T model)
        {
            model.Document.Parts.RemoveAll(p => p is WebMarkup);

            model.Document.Parts
                            .AddMarkupPart(WebZone.Head, Head)
                            .AddMarkupPart(WebZone.Banner, Banner)
                            .AddMarkupPart(WebZone.Header, Header)
                            .AddMarkupPart(WebZone.Figure, Figure)
                            .AddMarkupPart(WebZone.Menu, Menu)
                            .AddMarkupPart(WebZone.Body, Body)
                            .AddMarkupPart(WebZone.Aside, Aside)
                            .AddMarkupPart(WebZone.Footer, Footer)
                            .AddMarkupPart(WebZone.Scripts, Scripts);
        }
    }

    internal static class PartsExtensions
    {
        public static List<WebPart> AddMarkupPart(this List<WebPart> parts, WebZone zone, string body)
        {
            if (!body.IsEmpty())
            {
                parts.Add(new WebMarkup()
                {
                    Zone = zone,
                    Body = body
                });
            }
            
            return parts;
        }
    }
}