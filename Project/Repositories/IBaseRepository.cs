﻿
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBaseRepository<TObject> where TObject:class
    {
        TObject Get(long id);
        Task<TObject> GetAsync(object key);
        ICollection<TObject> GetAll(int num, int page, Func<TObject, object> orderBy, Expression<Func<TObject, bool>> match, Func<TObject, object> orderByDescending = null);
        Task<IEnumerable<TObject>> GetAllAsync(int num,int page,Func<TObject,object> orderBy, Expression<Func<TObject, bool>> match, Func<TObject, object> orderByDescending = null);
        TObject Find(Expression<Func<TObject, bool>> match);
        Task<TObject> FindAsync(Expression<Func<TObject, bool>> match);
        ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match);
        Task<IEnumerable<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match);
        TObject Add(TObject t);
        Task<TObject> AddAsync(TObject t);
        TObject Update(TObject updated, long key);
        Task<TObject> UpdateAsync (TObject updated, object key);
        IEnumerable<TObject> AddAll(IEnumerable<TObject> tlist);
        Task<IEnumerable<TObject>> AddAllAsync(IEnumerable<TObject> tList);
        int Delete(TObject t);
        Task<int> DeleteAsync(TObject t);
        int Count();
        Task<int> CountAsync(Expression<Func<TObject, bool>> match);
        Task<IList<TObject>> UpdateAllAsync(IEnumerable<TObject> updated, string property);
        IList<TObject> UpdateAll(IEnumerable<TObject> updated, string property);
    }
}
