using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plano.Data.Interface
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Create(T _object);

        //public void Update(T _object);

        public IEnumerable<T> GetAll();

        public T GetById(int Id);

        //public void Delete(T _object);
    }
}
