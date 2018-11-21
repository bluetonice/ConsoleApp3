using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public interface IGenericRepository 
    {
        Guid Save<T>(T entity) where T : class, IBaseEntity;
        T Get<T>(Guid id) where T : class;
        void Update<T>(T entity) where T : class;
        bool Delete<T>(Guid id) where T : class;
        IEnumerable<T> All<T>() where T : class;
        IEnumerable<T> Search<T>(BaseSearchModel search) where T : class;

        void SearchByUniqueId<T>(Guid UniqueId) where T : class, IBaseEntity;
    }
}
