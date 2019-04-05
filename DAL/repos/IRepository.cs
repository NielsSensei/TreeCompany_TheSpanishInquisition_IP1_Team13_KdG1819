using System.Collections.Generic;

namespace DAL
{
    public interface IRepository<T>
    {
        IEnumerable<T> ReadAll();
        //TODO: ipv kijken in memory query op db voor vergelijking.
        T Create(T obj);
        T Read(int id, bool details);
        //TODO: Reform update structure.
        void Update(T obj);
        void Delete(int id);
    }
}