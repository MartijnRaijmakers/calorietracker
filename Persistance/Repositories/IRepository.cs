using Persistance.Dto;

namespace Persistance.Repositories
{
    public interface IRepository<T> where T : DtoEntity
    {
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(int id);
    }
}
