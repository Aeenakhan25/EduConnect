namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle & Generic Design:
//   IRepository provides a focused contract for standard CRUD operations.
//   If a concrete repository is needed, consumers depend on this segregated
//   interface rather than a bloated, all-in-one data context.
public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
