using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Repositories;
using Services;
namespace Services
{
    public class BaseService<TObject> : IBaseService<TObject> where TObject : class
    {
        private readonly IBaseRepository<TObject> _baseRepository;

        public BaseService(IBaseRepository<TObject> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<IEnumerable<TObject>> GetAllAsync(int num, int page, Func<TObject, object> orderBy, Expression<Func<TObject, bool>> match)
        {
            return await _baseRepository.GetAllAsync(num, page, orderBy, match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await _baseRepository.FindAsync(match);
        }

        public async Task<IEnumerable<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await _baseRepository.FindAllAsync(match);
        }

        public async Task<TObject> AddAsync(TObject t)
        {
            return await _baseRepository.AddAsync(t);
        }

        public async Task<TObject> UpdateAsync(TObject updated, long key)
        {
            return await _baseRepository.UpdateAsync(updated, key);
        }

        public async Task<bool> DeleteAsync(TObject t, string property)
        {
            object obj = t.GetPropValue(property);
            return await  _baseRepository.UpdateAsync(t,obj)!=null;
        }

        public async Task<TObject> GetAsync(object key)
        {
            return await GetAsync(key);
        }

        public async Task<int> CountAsync(Expression<Func<TObject, bool>> match)
        {
            return await _baseRepository.CountAsync(match);
;        }

        public ICollection<TObject> GetAll(int num, int page, Func<TObject, object> orderBy, Expression<Func<TObject, bool>> match)
        {
            return _baseRepository.GetAll(num, page, orderBy, match);
        }
    }
}
