using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FileHelpers;

namespace Instatus.Integration.FileHelper
{
    public class CsvResult<T> : ActionResult 
    {
        private string slug;
        private IEnumerable<T> results;
        
        public override void ExecuteResult(ControllerContext context)
        {
            var fileHelperEngine = new FileHelperEngine<T>();
            var response = context.HttpContext.Response;
            
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}-{1:yyyy-MM-dd-HH-mm-ss}.csv", slug, DateTime.UtcNow));

            using (var memoryStream = response.OutputStream)
            using (var textWriter = new StreamWriter(memoryStream))
            {
                fileHelperEngine.WriteStream(textWriter, results);
            }
        }

        public CsvResult(string slug, IEnumerable<T> results)
        {
            this.slug = slug;
            this.results = results;
        }
    }
}
