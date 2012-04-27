using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class MarkupViewModel : BaseViewModel<Page>
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

        public override void Load(Page model)
        {
            var markupSections = model.Document.Parts.Where(p => p.IsRawHtml).ToList();

            if (!markupSections.IsEmpty())
            {
                markupSections.Where(m => m.Zone == Zone.Head).ForFirst(m => Head = m.Body);
                markupSections.Where(m => m.Zone == Zone.Banner).ForFirst(m => Banner = m.Body);
                markupSections.Where(m => m.Zone == Zone.Header).ForFirst(m => Header = m.Body);
                markupSections.Where(m => m.Zone == Zone.Figure).ForFirst(m => Figure = m.Body);
                markupSections.Where(m => m.Zone == Zone.Menu).ForFirst(m => Menu = m.Body);
                markupSections.Where(m => m.Zone == Zone.Body).ForFirst(m => Body = m.Body);
                markupSections.Where(m => m.Zone == Zone.Aside).ForFirst(m => Aside = m.Body);
                markupSections.Where(m => m.Zone == Zone.Footer).ForFirst(m => Footer = m.Body);
                markupSections.Where(m => m.Zone == Zone.Scripts).ForFirst(m => Scripts = m.Body);
            }
        }

        public override void Save(Page model)
        {
            model.Document.Parts.RemoveAll(p => p.IsRawHtml);

            model.Document.Parts
                            .AddMarkupPart(Zone.Head, Head)
                            .AddMarkupPart(Zone.Banner, Banner)
                            .AddMarkupPart(Zone.Header, Header)
                            .AddMarkupPart(Zone.Figure, Figure)
                            .AddMarkupPart(Zone.Menu, Menu)
                            .AddMarkupPart(Zone.Body, Body)
                            .AddMarkupPart(Zone.Aside, Aside)
                            .AddMarkupPart(Zone.Footer, Footer)
                            .AddMarkupPart(Zone.Scripts, Scripts);
        }
    }

    internal static class PartsExtensions
    {
        public static List<Part> AddMarkupPart(this List<Part> parts, Zone zone, string body)
        {
            if (!body.IsEmpty())
            {
                parts.Add(new Part()
                {
                    Zone = zone,
                    Body = body
                });
            }
            
            return parts;
        }
    }
}