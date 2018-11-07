using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    T Get(long id);
    List<T> GetAll();
    void Add(T entity);
    void DeleteOnSubmit(T entity);
    void DeleteOnSubmit(T entity, bool logicalDelete);
    void DeleteAllOnSubmit(IEnumerable<T> entities);
    void DeleteAllOnSubmit(IEnumerable<T> entities, bool logicalDelete);
    void SubmitChanges();
    void Delete(long id);

    List<T> GetAll(int start, int limit, out int count);
}
