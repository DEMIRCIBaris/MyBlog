using Microsoft.EntityFrameworkCore;
using MyBlog.Shared.Data.Abstract;
using MyBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Data.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {

        private readonly DbContext _context;

        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            //_context.Articles==_context.Set<Articles>();
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().CountAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            //Remove metodunun asenkron hali olmadığı için böyle bir çözüm bulduk
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
            
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            //Update metodunun asenkron hali olmadığı için böyle bir çözüm bulduk
            await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
            return entity;
        }
    }
}
