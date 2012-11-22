using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Instatus.Integration.Mvc
{
    public class FileModel : IValidatableObject
    {
        private string[] allowedMimeTypes;
        private int maximumContentLength;
        
        [Display(Name = "Upload File")]
        [DataType("Upload")]
        public string FileName { get; set; }

        [ScaffoldColumn(false)]
        public Stream InputStream
        {
            get 
            {
                return HttpContext.Current.Request.Files[0].InputStream;
            }            
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var files = HttpContext.Current.Request.Files;

            if (files == null || files.Count != 1 || files[0].ContentLength == 0)
            {
                yield return new ValidationResult("File required", new string[] { "FileName" });
            }
            else
            {
                var file = files[0];

                if (file.ContentLength > maximumContentLength)
                {
                    yield return new ValidationResult("File too large", new string[] { "FileName" });
                }

                if (!allowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
                {
                    yield return new ValidationResult("File type not allowed", new string[] { "FileName" });
                }

                FileName = file.FileName;
            }
        }

        public FileModel()
        {

        }

        public FileModel(string[] allowedMimeTypes, int maximumContentLength)
        {
            this.allowedMimeTypes = allowedMimeTypes;
            this.maximumContentLength = maximumContentLength;
        }
    }
}
