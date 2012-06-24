using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Entities;
using Instatus.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Instatus;

namespace Instatus.Areas.Editor.Models
{
    public class PartsViewModel<T> : BaseViewModel<Page> where T : BaseViewModel<Part>, new()
    {
        private Zone zone;
        private int minLength;
        
        public PartsViewModel(Zone zone, int minLength = 2) 
        {
            this.zone = zone;
            this.minLength = minLength;
        } 
        
        [ScaffoldColumn(true)]
        public IEnumerable<T> ViewModels { get; set; }

        public override void Load(Page model)
        {
            ViewModels = model.Document.Parts.Where(p => p.Zone == zone).Select(p =>
            {
                var viewModel = new T();
                viewModel.Load(p);
                return viewModel;
            })
            .ToList()
            .Pad(minLength);
        }

        public override void Save(Page model)
        {
            model.Document.Parts.RemoveAll(p => p.Zone == zone);

            if (!ViewModels.IsEmpty()) 
            {
                var parts = ViewModels
                    .RemoveNullOrEmpty()
                    .Select(m => {
                        var part = m.ToModel();
                        part.Zone = zone;
                        return part;
                    });
                
                if(!parts.IsEmpty())
                    model.Document.Parts.AddRange(parts);
            }
        }
    }
}