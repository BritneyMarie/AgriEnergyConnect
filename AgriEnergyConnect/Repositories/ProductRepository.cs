using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, int? farmerId = null,
        string? category = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Products
            .Include(p => p.Farmer)
            .AsNoTracking()
            .AsQueryable();

        if (farmerId.HasValue)
            query = query.Where(p => p.FarmerId == farmerId.Value);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (startDate.HasValue)
            query = query.Where(p => p.ProductionDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.ProductionDate <= endDate.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.ProductionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Farmer)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
