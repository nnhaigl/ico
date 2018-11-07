using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ICOCore.Repositories.Base
{
    public partial class Repository<T> : IDisposable, IRepository<T> where T : class //: remove to Generic common category
    {
        protected InvestmentDataContext dataContext;
        private bool _disposed;

        public Repository()
        {
            //dataContext = SingleDataContext.CurrentInstance();
            dataContext = new InvestmentDataContext();
            _disposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (dataContext != null)
                        dataContext.Dispose();
                }

                // Indicate that the instance has been disposed.
                dataContext = null;
                _disposed = true;
            }
        }

        public void ClearCache()
        {
            dataContext.GetType().InvokeMember("ClearCache",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                null, dataContext, null);
        }

        public virtual T Get(long id)
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            var whereExpression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(
                    Expression.Property(
                        itemParameter, typeof(T).GetPrimaryKey().Name
                        ),
                    Expression.Constant(id)
                    ),
                new[] { itemParameter }
                );

            var item = GetTable().Where(whereExpression).SingleOrDefault();

            //if (item == null)
            //{
            //    throw new PrimaryKeyNotFoundException(string.Format("No {0} with primary key {1} found",
            //                                                        typeof(T).FullName, id));
            //}

            return item;
        }

        public virtual List<T> GetAll(int start, int limit, out int count)
        {
            count = dataContext.GetTable<T>().Count();
            return dataContext.GetTable<T>().Skip(start).Take(limit).ToList();
        }

        public virtual List<T> GetAll()
        {
            return dataContext.GetTable<T>().ToList();
        }

        public virtual void Add(T entity)
        {
            GetTable().InsertOnSubmit(entity);
        }

        public virtual void Update(T entity)
        {
            GetTable().Attach(entity);
            dataContext.Refresh(RefreshMode.KeepCurrentValues, entity);
        }

        public virtual void UpdateAll(List<T> entities)
        {
            GetTable().AttachAll(entities);
            dataContext.Refresh(RefreshMode.KeepCurrentValues, entities);
        }

        public virtual void DeleteOnSubmit(T entity, bool logicalDelete)
        {

            GetTable().DeleteOnSubmit(entity);
        }

        public virtual void DeleteOnSubmit(T entity)
        {
            DeleteOnSubmit(entity, true);
        }

        //public virtual void Delete(int id)
        //{
        //    DeleteOnSubmit(Get(id), false);
        //}

        public virtual void Delete(long id)
        {
            DeleteOnSubmit(Get(id), false);
        }

        public virtual void DeleteAllOnSubmit(IEnumerable<T> entities, bool logicalDelete)
        {

            GetTable().DeleteAllOnSubmit(entities);
        }

        public virtual void DeleteAllOnSubmit(IEnumerable<T> entities)
        {
            DeleteAllOnSubmit(entities, true);
        }

        public virtual void SubmitChanges()
        {
            try
            {
                dataContext.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }
            catch
            {
                foreach (System.Data.Linq.ObjectChangeConflict occ in dataContext.ChangeConflicts)
                {
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                }
                dataContext.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
            }
        }

        public virtual Table<T> GetTable()
        {
            return dataContext.GetTable<T>();
        }

        public int Count()
        {
            return Table().Count();
        }


        public virtual IQueryable<T> Table()
        {
            return dataContext.GetTable<T>();
        }

        //public Dto ToDto (T source)
        //{
        //    return (Dto)Mapper.Map(source, typeof(T), typeof(Dto));
        //}


    }
}
