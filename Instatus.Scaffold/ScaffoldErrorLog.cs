using Elmah;
using Instatus.Core;
using Instatus.Scaffold.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold
{
    public class ScaffoldErrorLog : ErrorLog
    {
        public override ErrorLogEntry GetError(string id)
        {
            using (var context = AppContext.CreateContainer())
            {
                var entityStorage = context.Resolve<IEntityStorage>();
                var log = entityStorage.Set<Log>().Find(int.Parse(id));

                if (log != null)
                {
                    return new ErrorLogEntry(this, id, ErrorXml.DecodeString(log.Data));
                }
                else
                {
                    return null;
                }
            }
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            using (var container = AppContext.CreateContainer())
            {
                var entityStorage = container.Resolve<IEntityStorage>();
                var logs = entityStorage
                    .Set<Log>()
                    .OrderByDescending(e => e.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var log in logs)
                {
                    errorEntryList.Add(new ErrorLogEntry(this, log.Id.ToString(), ErrorXml.DecodeString(log.Data)));
                }

                return logs.Count();
            }
        }

        public override string Log(Error error)
        {
            using (var context = AppContext.CreateContainer())
            {
                var entityStorage = context.Resolve<IEntityStorage>();
                var log = new Log()
                {
                    StatusCode = error.StatusCode,
                    Data = ErrorXml.EncodeString(error)
                };

                entityStorage.Set<Log>().Add(log);
                entityStorage.SaveChanges();

                return log.Id.ToString();
            }
        }

        // constructors required by Elmah
        public ScaffoldErrorLog(IDictionary config)
        {

        }

        public ScaffoldErrorLog(string connectionString)
        {

        }
    }
}