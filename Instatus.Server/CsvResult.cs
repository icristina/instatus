using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Server
{
    public class CsvResult<T> : ActionResult
    {
        private string slug;
        private string headerText;
        private IEnumerable<T> results;

        public override void ExecuteResult(ControllerContext context)
        {
            var fileHelperEngine = new FileHelperEngine<T>()
            {
                HeaderText = headerText
            };

            var response = context.HttpContext.Response;

            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}-{1:yyyy-MM-dd-HH-mm-ss}.csv", slug, DateTime.UtcNow));

            using (var textWriter = new StreamWriter(response.OutputStream))
            {
                fileHelperEngine.WriteStream(textWriter, results);
            }
        }

        public CsvResult(string slug, IEnumerable<T> results, string headerText)
        {
            this.slug = slug;
            this.results = results;
            this.headerText = headerText;
        }
    }
}