using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Instatus
{
    public class ValidateableViewModel : BindableBase, INotifyDataErrorInfo
    {
        private IDictionary<string, List<Func<string>>> validators = new Dictionary<string, List<Func<string>>>();
        private IDictionary<string, List<string>> errorMessages = new Dictionary<string, List<string>>();
        private IDictionary<string, ValidationResult> validationResults = new Dictionary<string, ValidationResult>();

        public IDictionary<string, ValidationResult> Errors
        {
            get
            {
                return validationResults;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            var eventHandler = this.ErrorsChanged;

            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return errorMessages[propertyName];
        }

        public bool HasErrors
        {
            get 
            {
                return errorMessages.Any(e => e.Value.Any());
            }
        }

        private void AddValidator<T>(string propertyName, Func<T> accessor, Func<T, bool> evaluate, string errorMessage)
        {
            if (!validators.ContainsKey(propertyName))
            {
                validators[propertyName] = new List<Func<string>>();
            }

            validators[propertyName].Add(() =>
            {
                return evaluate(accessor()) ? null : errorMessage;
            });

            if (!validationResults.ContainsKey(propertyName))
            {
                validationResults.Add(propertyName, new ValidationResult());
            }

            if (!errorMessages.ContainsKey(propertyName))
            {
                errorMessages[propertyName] = new List<string>();
            }
        }

        protected void Required(string propertyName, Func<string> accessor, string errorMessage = "Required") 
        {
            AddValidator<string>(propertyName, accessor, value => !string.IsNullOrWhiteSpace(value), errorMessage);
        }

        protected void MinLength(string propertyName, Func<string> accessor, int length, string errorMessage = "Min length {0} characters")
        {
            AddValidator<string>(propertyName, accessor, value => !string.IsNullOrWhiteSpace(value) && value.Length >= length, string.Format(errorMessage, length));
        }

        protected void MaxLength(string propertyName, Func<string> accessor, int length, string errorMessage = "Max length {0} characters")
        {
            AddValidator<string>(propertyName, accessor, value => string.IsNullOrWhiteSpace(value) || value.Length <= length, string.Format(errorMessage, length));
        }

        protected void Min(string propertyName, Func<int> accessor, int min, string errorMessage = "Min {0}")
        {
            AddValidator<int>(propertyName, accessor, value => value >= min, string.Format(errorMessage, min));
        }

        protected void Max(string propertyName, Func<int> accessor, int max, string errorMessage = "Max {0}")
        {
            AddValidator<int>(propertyName, accessor, value => value <= max, string.Format(errorMessage, max));
        }

        protected void Regex(string propertyName, Func<string> accessor, string pattern, string errorMessage = "Invalid")
        {
            var regex = new Regex(pattern);
            
            AddValidator<string>(propertyName, accessor, value => string.IsNullOrWhiteSpace(value) || regex.IsMatch(value), errorMessage);
        }

        private bool ValidateProperty(string propertyName)
        {
            List<Func<string>> propertyValidators;

            if (validators.TryGetValue(propertyName, out propertyValidators))
            {
                var currentErrors = errorMessages[propertyName];
                var propertyErrorMessages = propertyValidators.Select(e => e()).Where(e => e != null).ToList();

                if (!currentErrors.SequenceEqual(propertyErrorMessages))
                {
                    OnErrorsChanged(propertyName);
                }

                errorMessages[propertyName] = propertyErrorMessages;
                validationResults[propertyName].Message = propertyErrorMessages.FirstOrDefault();

                return !propertyErrorMessages.Any();
            }

            return true;
        }

        public ValidateableViewModel()
        {
            PropertyChanged += (c, e) =>
            {
                ValidateProperty(e.PropertyName);
            };
        }
    }
}
