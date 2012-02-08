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
    public class VideoViewModel<T> : BaseViewModel<T, BaseDataContext> where T : Page
    {       
        public string Uri { get; set; }

        public override void Load(T model)
        {
            var video = model.Links.OfType<Video>().FirstOrDefault();

            if (video != null)
                Uri = video.Uri;
        }

        public override void Save(T model)
        {
            
        }
    }
}