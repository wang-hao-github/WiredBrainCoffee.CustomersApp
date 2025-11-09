using Microsoft.EntityFrameworkCore;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : class, IModelBase
    {
        private readonly IDbContextFactory<AppDataContext> _dbContextFactory;

        public RepositoryBase(IDbContextFactory<AppDataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public int Add(T entity)
        {
            using var db = _dbContextFactory.CreateDbContext();
            db.Add(entity);
            db.SaveChanges();
            return (int)db.Entry(entity).Property("ID").CurrentValue!;
        }

        public void AddRange(List<T> entities)
        {
            using var db = _dbContextFactory.CreateDbContext();
            db.AddRange(entities);
            db.SaveChanges();
        }

        public bool Update(T entity)
        {
            using var db = _dbContextFactory.CreateDbContext();
            db.Update(entity);
            return db.SaveChanges() > 0;
        }

        public void UpdateRange(List<T> entities)
        {
            using var db = _dbContextFactory.CreateDbContext();
            db.UpdateRange(entities);
            db.SaveChanges();
        }

        public bool Delete(T entity)
        {
            using var db = _dbContextFactory.CreateDbContext();
            // 标记删除或直接删除，根据你的业务处理；这里以删除为例
            db.Remove(entity!);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var entity = db.Set<T>().FirstOrDefault(e => e.ID == id && !e.IsDeleted);
            if (entity == null) { return false; }
            db.Remove(entity!);
            return db.SaveChanges() > 0;
        }

        public void DeleteRange(int[] ids)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var entities = db.Set<T>().Where(e => ids.Contains(e.ID)).ToList();
            db.RemoveRange(entities);
            db.SaveChanges();
        }

        public T? FirstOrDefault(Expression<Func<T, bool>> queryExpress, string? sort = null)
        {
            using var db = _dbContextFactory.CreateDbContext();
            IQueryable<T> query = db.Set<T>().AsNoTracking().Where(e => !e.IsDeleted).Where(queryExpress);
            query = GetOrderedQueryable(query, sort);
            return query.FirstOrDefault();
        }

        public List<T> All(string? sort = null)
        {
            using var db = _dbContextFactory.CreateDbContext();
            IQueryable<T> query = db.Set<T>().AsNoTracking().Where(e => !e.IsDeleted);
            query = GetOrderedQueryable(query, sort);
            return query.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> queryExpress, string? sort = null)
        {
            using var db = _dbContextFactory.CreateDbContext();
            IQueryable<T> query = db.Set<T>().AsNoTracking().Where(e => !e.IsDeleted).Where(queryExpress);
            query = GetOrderedQueryable(query, sort);
            return query.ToList();
        }

        public PageResult<T> GetPage(Expression<Func<T, bool>> queryExpress, int pageIndex = 0, int pageSize = 10, string? sort = null)
        {
            using var db = _dbContextFactory.CreateDbContext();
            IQueryable<T> query = db.Set<T>().AsNoTracking().Where(e => !e.IsDeleted).Where(queryExpress);
            query = GetOrderedQueryable(query, sort);
            var totalCount = query.Count();
            var items = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return new PageResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling((double)totalCount / pageSize),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        #region
        private IOrderedQueryable<T> GetOrderedQueryable(IQueryable<T> source, string? sort = null)
        {
            if (sort == null)
            {
                return source.OrderByDescending(e => e.ID);
            }
            Type enityType = typeof(T);
            ParameterExpression parameter = Expression.Parameter(enityType, "e");
            bool first = true;
            foreach (var kv in sort.Split(";", StringSplitOptions.TrimEntries))
            {
                var parts = kv.Split(",", StringSplitOptions.TrimEntries);
                if (parts.Length == 2)
                {
                    var fileName = parts[0];
                    var direction = parts[1];
                    Expression body = Expression.Property(parameter, fileName);
                    var lambda = Expression.Lambda(body, parameter);
                    string methodName = direction.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                    if (!first)
                    {
                        methodName = direction.ToLower() == "desc" ? "ThenByDescending" : "ThenBy";
                    }
                    var method = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(e => e.Name == methodName && e.GetParameters().Length == 2)
                        .Single().MakeGenericMethod(enityType, body.Type);
                    source = (IOrderedQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
                    first = false;
                }
            }
            return (IOrderedQueryable<T>)source;
        }
        #endregion
    }
}