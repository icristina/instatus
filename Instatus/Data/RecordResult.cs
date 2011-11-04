using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Data
{
    // similar to http://msdn.microsoft.com/en-us/library/system.data.entity.validation.dbentityvalidationresult(v=vs.103).aspx
    public class RecordResult<T> : Record<T>
    {
        public IEnumerable<ValidationResult> ValidationErrors { get; set; }

        public bool IsValid
        {
            get
            {
                return ValidationErrors.IsEmpty();
            }
        }

        public RecordResult(T model, IEnumerable<ValidationResult> validationErrors = null)
        {
            Entry = model;
            ValidationErrors = validationErrors;
        }

        public static RecordResult<T> Failed
        {
            get {
                return new RecordResult<T>(default(T), null);
            }
        }
    }
}