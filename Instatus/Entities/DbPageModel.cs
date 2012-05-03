using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Entities
{
    public class DbPageModel : IPageModel
    {
        private IApplicationModel applicationModel;
        
        public IEnumerable<Page> GetPages(Models.Query query)
        {
            throw new NotImplementedException();
        }

        public Page GetPage(string slug, Models.Set set = null)
        {
            return applicationModel.Pages.FirstOrDefault(p => p.Alias == slug);
        }

        public DbPageModel(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}