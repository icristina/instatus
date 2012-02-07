using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using Instatus.Data;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class OverviewViewModel : IViewModel<Page>
    {
        [DisplayName("Title")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        public void Load(Page model)
        {
            this.ApplyValues(model);
        }

        public void Save(Page model)
        {
            model.ApplyValues(this);
        }

        public void Databind()
        {
            
        }

        [ScaffoldColumn(false)]
        public WebStep Step
        {
            get {
                return WebStep.Start;
            }
        }
    }
}