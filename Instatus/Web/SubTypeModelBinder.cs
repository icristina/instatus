using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class SubTypeModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var subType = SubTypeDefinitions.Where(t => t.Item3 == modelType && t.Item2 == bindingContext.ModelName).Select(t => t.Item4).FirstOrDefault();
            
            if (subType != null)
            {
                var propertyModel = Activator.CreateInstance(subType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, subType);
                return propertyModel;
            }
            
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        public static List<Tuple<Type, string, Type, Type>> SubTypeDefinitions = new List<Tuple<Type, string, Type, Type>>();

        public static void RegisterSubType<TBaseType, TSubType>(Type modelType, string propertyName) where TSubType : TBaseType
        {
            SubTypeDefinitions.Add(new Tuple<Type,string, Type, Type>(modelType, propertyName, typeof(TBaseType), typeof(TSubType)));            
        }
    }
}