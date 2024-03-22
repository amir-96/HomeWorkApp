using System.Linq.Expressions;
using Domain.BaseModels;

namespace Domain
{
    public interface IRepository<TKey, T> where T : BaseModel
  {
    Task<ServerResponse<List<T>>> Get();
    Task<ServerResponse<T>> Get(TKey id);
    Task<ServerResponse<bool>> Create(T entity);
    Task<ServerResponse<bool>> Update(TKey id, T entityToUpdate);
    Task<ServerResponse<bool>> Exists(Expression<Func<T, bool>> expression);
    Task<ServerResponse<bool>> Delete(TKey id);
    Task<ServerResponse<bool>> Restore(TKey id);
  }
}
