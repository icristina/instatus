using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class DynamicObjectViewModel
    {
        public ObservableCollection<DynamicPropertyViewModel> Properties { get; set; }

        public void Fill(IDictionary<string, object> data)
        {
            foreach (var property in Properties)
            {
                object value;

                if (data.TryGetValue(property.Name, out value))
                {
                    property.Value = value;
                }
            }
        }

        public void EnableValidation()
        {
            foreach (var property in Properties)
            {
                property.StartValidation();
            }
        }

        public void EnableValidationOnChange()
        {
            foreach (var property in Properties)
            {
                property.PropertyChanged += (c, e) =>
                {                   
                    if (e.PropertyName == "Value")
                    {
                        property.StartValidation();
                        property.ValidateProperty("Value");
                    }
                };
            }
        }

        public DynamicObjectViewModel()
        {
            Properties = new ObservableCollection<DynamicPropertyViewModel>();
        }
    }
}
