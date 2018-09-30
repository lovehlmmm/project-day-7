using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBaseService<TObject> where TObject:class
    {
        Task<IEnumerable<TObject>> GetAllAsync(int num,int page, Func<TObject, object> orderBy, Expression<Func<TObject, bool>> match, Func<TObject, object> orderByDescending = null);
        Task<TObject> FindAsync(Expression<Func<TObject, bool>> match);
        Task<IEnumerable<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match); 
        Task<TObject> AddAsync(TObject t);
        Task<TObject> UpdateAsync(TObject updated, object key);
        Task<bool> DeleteAsync(TObject t,string property);
        Task<TObject> GetAsync(object key);
        Task<int> CountAsync(Expression<Func<TObject, bool>> match);
        ICollection<TObject> GetAll(int num, int page, Func<TObject, object> orderBy, Expression<Func<TObject, bool>> match, Func<TObject, object> orderByDescending = null);
        TObject Find(Expression<Func<TObject, bool>> match);
        ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match);
        IEnumerable<TObject> AddAll(IEnumerable<TObject> tlist);
        Task<IList<TObject>> UpdateAllAsync(IEnumerable<TObject> updated, string property);
        IList<TObject> UpdateAll(IEnumerable<TObject> updated, string property);
    }
}
