using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Repositories.Interfaces;

namespace University.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(DbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var dbEntities = Db.Model
                .FindEntityType(typeof(TEntity))
                .GetNavigations()
                .Select(x => x.PropertyInfo.PropertyType);
            var modelProperties = typeof(TEntity)
                .GetProperties()
                .Select(x => new Tuple<string, Type>(x.Name, x.PropertyType));

            var pathesToInclude = MultiIncludes(dbEntities, modelProperties);
            pathesToInclude.RemoveAll(x => pathesToInclude.Any(y => x != y && y.Contains(x)));

            var query = DbSet.AsQueryable();
            foreach (var path in pathesToInclude)
            {
                query = query.Include(path);
            }

            return await query.ToListAsync();
        }

        // Not tested on another trees
        private List<string> MultiIncludes(IEnumerable<Type> entities, IEnumerable<Tuple<string, Type>> properties, List<string> pathes = default, string currentPath = default)
        {
            if (pathes == default) 
                pathes = new List<string>();

            var intersection = properties.Where(x => entities.Contains(x.Item2));

            foreach(var item in intersection)
            {
                var prefix = string.IsNullOrEmpty(currentPath) ? string.Empty : $"{currentPath}.";
                var nextPath = $"{prefix}{item.Item1}";
                pathes.Add(nextPath);
                var entitiesToNext = Db.Model
                    .FindEntityType(item.Item2)
                    .GetNavigations()
                    .Select(x => x.PropertyInfo.PropertyType);

                var propertiesToNext = item.Item2
                    .GetProperties()
                    .Select(x => new Tuple<string, Type>(x.Name, x.PropertyType));

                MultiIncludes(entitiesToNext, propertiesToNext, pathes, nextPath);
            }

            return pathes;
        }

        public async Task<TEntity> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task Create(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Db.SaveChangesAsync();
        }

        public async Task AddOrUpdate(TEntity entity)
        {
            if (DbSet.Contains(entity))
            {
                DbSet.Update(entity);
                await Db.SaveChangesAsync();
                return;
            }

            await Create(entity);
        }

        public async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await Db.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var entity = await GetById(id);
            Db.Entry(entity).State = EntityState.Deleted;
            await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
