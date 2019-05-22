using System.Collections.Generic;

namespace DAL.repos
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public interface IRepository<T>
    {
        IEnumerable<T> ReadAll();
        T Create(T obj);
        T Read(int id, bool details);
        void Update(T obj);
        void Delete(int id);
    }
}
