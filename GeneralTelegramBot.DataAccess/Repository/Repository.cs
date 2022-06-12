using System.Linq.Expressions;
using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GeneralTelegramBot.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GeneralTelegramBotDbContext dbContext;
        private readonly DbSet<T> dbSet;

        protected Repository(GeneralTelegramBotDbContext dbContext)
        {
            this.dbContext= dbContext;
            this.dbSet= dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            if (tracked)
            {
                IQueryable<T> query = dbSet;

                query = query.Where(filter);
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }
            else
            {
                IQueryable<T> query = dbSet.AsNoTracking();

                query = query.Where(filter);
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }
            
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
