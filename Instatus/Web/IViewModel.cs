using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Web
{
    public interface IViewModel
    {
        WebStep Step { get; }
    }
    
    public interface IViewModel<TModel> : IViewModel
    {
        void Load(TModel model);
        void Save(TModel model);
        void Databind();
    }

    public class BaseViewModel<TModel> : IViewModel<TModel>
    {
        [ScaffoldColumn(false)]
        public WebStep Step { get; set; }
        
        public virtual void Load(TModel model)
        {
            this.ApplyValues(model);
        }

        public virtual void Save(TModel model)
        {
            model.ApplyValues(this);
        }

        public virtual void Databind() {

        }

        public BaseViewModel()
        {
            Step = WebStep.Start;
        }
    }

    public static class BindingHelpers
    {
        public static ICollection<T> UpdateList<T, TKey>(IDbSet<T> set, ICollection<T> list, IEnumerable<TKey> selected) where T : class
        {
            if (selected.IsEmpty())
                return new List<T>();
            
            var selectedKeys = selected.ToList();

            if (list == null)
            {
                list = new List<T>();
            }
            else
            {
                var items = list.ToArray(); // clone list to iterate
                
                foreach (var item in items)
                {
                    var id = (TKey)item.GetKey();

                    if (!(selectedKeys.Contains(id)))
                    {
                        list.Remove(item);
                    }
                    else
                    {
                        selectedKeys.Remove(id);
                    }
                }
            }

            foreach (var id in selectedKeys)
            {
                var item = set.Find(id);
                list.Add(item);
            }

            return list;
        }
    }

    public class BaseViewModel<TModel, TContext> : BaseViewModel<TModel>
    {
        [ScaffoldColumn(false)]
        public TContext Context { get; set; }

        public ICollection<T> UpdateList<T, TKey>(IDbSet<T> set, ICollection<T> list, IEnumerable<TKey> selected) where T : class
        {
            return BindingHelpers.UpdateList<T, TKey>(set, list, selected);
        }
    }
}