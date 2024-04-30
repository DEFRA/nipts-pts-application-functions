namespace Defra.PTS.Application.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Find(object id);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void Delete(object id);
        Task<int> SaveChanges();

    }
}
