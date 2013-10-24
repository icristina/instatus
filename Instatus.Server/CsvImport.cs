using AutoMapper;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Instatus.Server
{
    public static class CsvImport
    {
        private static void SeedFromFile<TRow, TEntity>(DbContext context, string relativePath, Action<DbContext, TRow, TEntity> update = null) where TEntity : class
        {
            var fileHelper = new FileHelperEngine<TRow>();
            var absolutePath = HttpContext.Current.Server.MapPath(relativePath);
            var entitySet = context.Set<TEntity>();
            var rows = fileHelper.ReadFile(absolutePath) as TRow[];

            if (Mapper.FindTypeMapFor<TRow, TEntity>() == null)
            {
                Mapper.CreateMap<TRow, TEntity>();
            }

            var entities = rows.Select(r => Mapper.Map<TEntity>(r)).ToList();

            for (var i = 0; i < rows.Length; i++)
            {
                var entity = entities[i];
                var row = rows[i];

                if (update != null)
                {
                    update(context, row, entity);
                }

                entitySet.Add(entity);
            }

            context.SaveChanges();
        }
    }
}
