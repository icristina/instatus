using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class FieldViewModel : BindableBase
    {
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

        private bool required;

        public bool Required
        {
            get
            {
                return required;
            }
            set
            {
                SetProperty(ref required, value);
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

        private int min = 0;

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
    }
}
