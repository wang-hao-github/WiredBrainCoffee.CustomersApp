using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> where T : class, IModelBase
    {
        int Add(T entity);
        void AddRange(List<T> entities);
        bool Update(T entity);
        void UpdateRange(List<T> entities);

        bool Delete(T entity);
        bool Delete(int id);
        void DeleteRange(int[] ids);

        T? FirstOrDefault(Expression<Func<T, bool>> queryExpress, string? sort = null);
        List<T> All(string? sort = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryExpress"></param>
        /// <param name="sort">fieldName1,asc;fieldName2,desc;etc</param>
        /// <returns></returns>
        List<T> List(Expression<Func<T, bool>> queryExpress, string? sort = null);
        PageResult<T> GetPage(Expression<Func<T, bool>> queryExpress, int pageIndex = 0, int pageSize = 10, string? sort = null);

    }
}
