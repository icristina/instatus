using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Web.Security;
using Instatus.Data;
using System.Runtime.Serialization;

namespace Instatus.Models
{
    public class Restriction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface IRestrictionEvaluator
    {
        string Name { get; }
        RestrictionResult Evaluate(RestrictionContext context);
    }

    public class RestrictionResultCollection : List<RestrictionResult>
    {
        public RestrictionContext Context { get; private set; }
        
        public bool IsValid {
            get
            {
                return this.All(r => r.IsValid);
            }
        }

        public string Message
        {
            get
            {
                var result = this.First(r => !r.Message.IsEmpty());
                return result.IsEmpty() ? string.Empty : result.Message;
            }
        }

        public RestrictionResultCollection(RestrictionContext context)
        {
            Context = context;
        }

        public void SaveActivities()
        {
            foreach (var activity in this.SelectMany(r => r.Activities))
            {
                Context.DataContext.Activities.Add(activity);
                
                if (Context.Trigger != null)
                {
                    if (Context.Trigger.Activities == null)
                        Context.Trigger.Activities = new List<Activity>();

                    Context.Trigger.Activities.Add(activity);
                }

                if(activity.Page == null)
                    activity.Page = Context.Page;

                if(activity.User == null)
                    activity.User = Context.User;
            }

            Context.DataContext.SaveChanges();
        }
    }

    public class RestrictionResult
    {
        public bool Continue { get; set; }
        public bool IsValid { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public string Message { get; set; }

        public RestrictionResult()
        {
            Continue = false;
            IsValid = false;
            Activities = new List<Activity>();
        }

        public static RestrictionResult Valid(bool condition = true)
        {
            return new RestrictionResult()
            {
                IsValid = condition
            };
        }
    }

    public class RestrictionContext {
        public BaseDataContext DataContext { get; set; }
        public Activity Trigger { get; set; }
        public User User { get; set; }
        public Page Page { get; set; }
    }

    public abstract class BaseRestrictionEvaluator<T> : IRestrictionEvaluator, IPayload
    {
        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public int Priority { get; set; }
        public T Value { get; set; }

        public byte[] Data
        {
            get
            {
                return Value.Serialize();
            }
            set
            {
                Value = value.Deserialize<T>();
            }
        }
        
        public virtual Restriction ToRestriction()
        {
            return new Restriction()
            {
                Name = Name,
                Data = Data,
                Priority = Priority
            };
        }

        public virtual RestrictionResult Evaluate(RestrictionContext context)
        {
            throw new NotImplementedException();
        }
    }
}