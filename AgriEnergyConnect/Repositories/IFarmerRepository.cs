using System.Linq.Expressions;

public interface IFarmerRepository
{
    Task<IEnumerable<Farmer>> GetAllAsync();
    Task<Farmer?> GetByIdAsync(int id);
    Task<Farmer?> GetByIdWithProductsAsync(int id);
    Task<Farmer?> GetByUserIdAsync(string userId);
    Task<(IEnumerable<Farmer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search = null);
    Task AddAsync(Farmer farmer);
    Task UpdateAsync(Farmer farmer);
    Task DeleteAsync(int id);
}
