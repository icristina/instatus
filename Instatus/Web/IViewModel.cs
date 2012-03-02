using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Instatus.Data;
using System.Collections;
using System.Web.Mvc;

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
            model.ActivateCollections();
            this.ApplyValues(model);
            this.ApplyAction<IViewModel<TModel>>(m => m.Load(model));
        }

        public virtual void Save(TModel model)
        {
            model.ActivateCollections();
            model.ApplyValues(this);
            this.ApplyAction<IViewModel<TModel>>(m => m.Save(model));
        }

        public virtual void Databind() {
            this.ApplyAction<IViewModel<TModel>>(m => m.Databind());
        }

        public BaseViewModel()
        {
            Step = WebStep.Start;
        }
    }

    public class BaseViewModel<TModel, TContext> : BaseViewModel<TModel>
    {
        private TContext context;

        [ScaffoldColumn(false)]
        public TContext Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
                this.ApplyAction<BaseViewModel<TModel, TContext>>(m => m.Context = context); // add context to all child properties as required
            }
        }

        public int[] LoadMultiAssociation<T>(IEnumerable<T> list)
        {
            return list.IsEmpty() ? null : list.Select(t => t.GetKey().AsInteger()).ToArray();
        }

        public ICollection<T> SaveMultiAssociation<T>(IDbSet<T> set, ICollection<T> list, IList keys) where T : class
        {
            if (keys.IsEmpty())
                return new List<T>();

            var selectedKeys = keys.Cast<object>().ToList();            

            if (list == null)
            {
                list = new List<T>();
            }
            else
            {
                var items = list.ToArray(); // clone list to iterate
                
                foreach (var item in items)
                {
                    var id = item.GetKey();

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

        public int? LoadAssociation<T, T2>(ICollection<T> list)
            where T : class
            where T2 : T
        {
            if (list.IsEmpty() || !list.OfType<T2>().Any())
                return null;
            
            return list.OfType<T2>().Select(t => t.GetKey().AsInteger()).First();
        }

        public ICollection<T> SaveAssociation<T, T2>(IDbSet<T> set, ICollection<T> list, int? id) 
            where T : class 
            where T2 : T
        {
            if (list.IsEmpty())
                list = new List<T>();
            
            list.OfType<T2>().ForFirst(o => list.Remove(o)); // remove first instance, even if id null

            if (id.HasValue)
            {
                list.Add(set.Find(id));
            }

            return list;
        }

        public SelectList DatabindSelectList<T, T2>(IDbSet<T> set, int? id, string dataValueField = "Id", string dataTextField = "Name") 
            where T : class
            where T2 : T
        {
            return new SelectList(set.OfType<T2>().ToList(), dataValueField, dataTextField, id);
        }

        public MultiSelectList DatabindMultiSelectList<T, T2>(IDbSet<T> set, int[] id, string dataValueField = "Id", string dataTextField = "Name") 
            where T : class
            where T2 : T
        {
            return new MultiSelectList(set.OfType<T2>().ToList(), dataValueField, dataTextField, id);
        }
    }
}