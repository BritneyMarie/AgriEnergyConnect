using Microsoft.EntityFrameworkCore;

public class FarmerRepository : IFarmerRepository
{
    private readonly ApplicationDbContext _context;

    public FarmerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Farmer>> GetAllAsync()
    {
        return await _context.Farmers.AsNoTracking().ToListAsync();
    }

    public async Task<Farmer?> GetByIdAsync(int id)
    {
        return await _context.Farmers.FindAsync(id);
    }

    public async Task<Farmer?> GetByIdWithProductsAsync(int id)
    {
        return await _context.Farmers
            .Include(f => f.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FarmerId == id);
    }

    public async Task<Farmer?> GetByUserIdAsync(string userId)
    {
        return await _context.Farmers
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.UserId == userId);
    }

    public async Task<(IEnumerable<Farmer> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null)
    {
        var query = _context.Farmers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(f =>
                f.FarmName.Contains(search) ||
                f.ContactPerson.Contains(search) ||
                f.Email.Contains(search));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(f => f.FarmName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Farmer farmer)
    {
        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Farmer farmer)
    {
        _context.Farmers.Update(farmer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer != null)
        {
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
        }
    }
}
