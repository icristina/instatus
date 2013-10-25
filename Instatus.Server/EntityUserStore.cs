using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public class EntityUserStore<TContext, TUser> : IUserStore<TUser>
        where TContext : DbContext, new()
        where TUser : class, IUser
    {
        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            using (var context = new TContext())
            {
                return await context.Set<TUser>().FirstOrDefaultAsync(u => u.UserName == userName);
            }
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
