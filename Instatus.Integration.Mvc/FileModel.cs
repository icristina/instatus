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

        [Display(Name = "Enable Conversion")]
        public bool EnableConversion { get; set; }
       
        [ScaffoldColumn(false)]
        public HttpPostedFile File
        {
            get
            {
                var files = HttpContext.Current.Request.Files;

                if (files == null || files.Count < 1 || files[0].ContentLength == 0)
                {
                    return null;
                }
                else
                {
                    return files[0];
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null)
            {
                yield return new ValidationResult("File required", new string[] { "FileName" });
            }
            else
            {
                if (File.ContentLength > maximumContentLength)
                {
                    yield return new ValidationResult("File too large", new string[] { "FileName" });
                }

                if (!allowedMimeTypes.Contains(File.ContentType, StringComparer.OrdinalIgnoreCase))
                {
                    yield return new ValidationResult("File type not allowed", new string[] { "FileName" });
                }

                FileName = File.FileName;
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
