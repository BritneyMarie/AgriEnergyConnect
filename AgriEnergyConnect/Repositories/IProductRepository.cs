public interface IProductRepository
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, int? farmerId = null,
        string? category = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
