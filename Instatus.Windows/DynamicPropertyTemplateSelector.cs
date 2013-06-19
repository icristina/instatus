using Instatus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Instatus.Windows
{
    public class DynamicPropertyTemplateSelector : DataTemplateSelector
    {
        // default template
        private DataTemplate textTemplate;

        public DataTemplate TextTemplate 
        {
            get 
            {
                return textTemplate;
            }
            set 
            {
                textTemplate = value;
            }
        }
        
        // additional templates
        private DataTemplate dateTemplate;

        public DataTemplate DateTemplate
        {
            get
            {
                return dateTemplate ?? textTemplate;
            }
            set
            {
                dateTemplate = value;
            }
        }

        private DataTemplate dateTimeTemplate;

        public DataTemplate DateTimeTemplate
        {
            get
            {
                return dateTimeTemplate ?? textTemplate;
            }
            set
            {
                dateTimeTemplate = value;
            }
        }

        private DataTemplate emailAddressTemplate;

        public DataTemplate EmailAddressTemplate
        {
            get
            {
                return emailAddressTemplate ?? textTemplate;
            }
            set
            {
                emailAddressTemplate = value;
            }
        }

        private DataTemplate multilineTextTemplate;

        public DataTemplate MultilineTextTemplate
        {
            get
            {
                return multilineTextTemplate ?? textTemplate;
            }
            set
            {
                multilineTextTemplate = value;
            }
        }

        private DataTemplate phoneNumberTemplate;

        public DataTemplate PhoneNumberTemplate
        {
            get
            {
                return phoneNumberTemplate ?? textTemplate;
            }
            set
            {
                phoneNumberTemplate = value;
            }
        }

        private DataTemplate postalCodeTemplate;

        public DataTemplate PostalCodeTemplate
        {
            get
            {
                return postalCodeTemplate ?? textTemplate;
            }
            set
            {
                postalCodeTemplate = value;
            }
        }

        private DataTemplate numberTemplate;

        public DataTemplate NumberTemplate
        {
            get
            {
                return numberTemplate ?? textTemplate;
            }
            set
            {
                numberTemplate = value;
            }
        }

        private DataTemplate booleanTemplate;

        public DataTemplate BooleanTemplate
        {
            get
            {
                return booleanTemplate ?? textTemplate;
            }
            set
            {
                booleanTemplate = value;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var dataType = ((DynamicPropertyViewModel)item).DataType;
            
            switch (dataType)
            {
                case DataType.Boolean:
                    return BooleanTemplate;
                case DataType.Date:
                    return DateTemplate;
                case DataType.DateTime:
                    return DateTimeTemplate;
                case DataType.EmailAddress:
                    return EmailAddressTemplate;
                case DataType.MultilineText:
                    return MultilineTextTemplate;
                case DataType.Number:
                    return NumberTemplate;
                case DataType.PhoneNumber:
                    return PhoneNumberTemplate;
                case DataType.PostalCode:
                    return PostalCodeTemplate;
                default:
                    return TextTemplate;
            }
        }
    }
}
