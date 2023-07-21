using RomaF5.Models;

namespace RomaF5.IRepository
{
	public interface IRepository<T>
	{
		Task<T> GetByIdAsync(int? id);
		Task<List<T>> GetAllAsync();
		Task AddAsync(T entity);
		Task Update(T entity);
		Task Delete(T entity);
    }
}
