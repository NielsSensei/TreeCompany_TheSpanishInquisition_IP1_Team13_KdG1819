using System.Collections.Generic;

namespace DAL
{
    public interface IRepository<T>
    {
        IEnumerable<T> ReadAll();
        //TODO: ipv kijken in memory query op db voor vergelijking.
        T Create(T obj);
        //TODO: Might need to think about detail read vs regular read.
        T Read(int id);
        //TODO: Reform update structure.
        void Update(T obj);
        void Delete(int id);
    }
}