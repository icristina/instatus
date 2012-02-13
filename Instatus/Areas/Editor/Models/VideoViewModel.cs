using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class VideoViewModel<T> : BaseViewModel<T, IDataContext> where T : Page
    {       
        public string Uri { get; set; }

        public override void Load(T model)
        {
            model.Links.OfType<Video>().ForFirst(v => Uri = v.Uri);
        }

        public override void Save(T model)
        {
            model.Links.OfType<Video>().ForFirst(v => model.Links.Remove(v));
            
            if (!Uri.IsEmpty())
            {
                model.Links.Add(new Video()
                {
                    Uri = Uri
                });
            }            
        }
    }
}