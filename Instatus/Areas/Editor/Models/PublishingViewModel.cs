using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class PublishingViewModel : IViewModel<Page>
    {
        public int Priority { get; set; }

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