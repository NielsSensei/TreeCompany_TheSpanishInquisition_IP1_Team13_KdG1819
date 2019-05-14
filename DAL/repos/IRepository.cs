using System.Collections.Generic;

namespace DAL.repos
{
    public interface IRepository<T>
    {
        IEnumerable<T> ReadAll();
        T Create(T obj);
        T Read(int id, bool details);
        void Update(T obj);
        void Delete(int id);
    }
}
