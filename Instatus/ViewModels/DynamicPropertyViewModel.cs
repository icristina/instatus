using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class DynamicPropertyViewModel : ValidateableViewModel
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        
        private string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetProperty(ref title, value);
            }
        }

        private object value;

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                SetProperty(ref this.value, value);
            }
        }

        private DataType dataType = DataType.Text;

        public DataType DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                SetProperty(ref dataType, value);
            }
        }

        private bool isRequired;

        public bool IsRequired
        {
            get
            {
                return isRequired;
            }
            set
            {
                SetProperty(ref isRequired, value);
            }
        }

        private string pattern;

        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                SetProperty(ref pattern, value);
            }
        }

        private int minLength = 0;

        public int MinLength
        {
            get
            {
                return minLength;
            }
            set
            {
                SetProperty(ref minLength, value);
            }
        }

        private int maxLength = int.MaxValue;

        public int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                SetProperty(ref maxLength, value);
            }
        }

        private int min = int.MinValue;

        public int Min
        {
            get
            {
                return min;
            }
            set
            {
                SetProperty(ref min, value);
            }
        }

        private int max = int.MaxValue;

        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                SetProperty(ref max, value);
            }
        }

        private bool startedValidation = false;

        public void StartValidation()
        {
            if (startedValidation)
            {
                return;
            }
            
            if (IsRequired)
            {
                AddRequiredValidator("Value", () => Value as string);
            }

            if (MinLength > 0) 
            {
                AddMinLengthValidator("Value", () => Value as string, MinLength);
            }

            if (MaxLength < int.MaxValue)
            {
                AddMaxLengthValidator("Value", () => Value as string, MaxLength);
            }

            if (Min > int.MinValue)
            {
                AddMinValidator("Value", () => Value is string ? int.Parse((string)Value) : (int)Value, Min);
            }

            if (Max < int.MaxValue)
            {
                AddMaxValidator("Value", () => Value is string ? int.Parse((string)Value) : (int)Value, Max);
            }

            if (!string.IsNullOrEmpty(Pattern))
            {
                AddRegexValidator("Value", () => Value as string, Pattern);
            }

            startedValidation = true;
        }

        public DynamicPropertyViewModel()
        {
            ValidatableProperty("Value");
        }
    }
}