using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly IFarmerRepository _farmerRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private const int PageSize = 10;

    public ProductsController(
        IProductRepository productRepo,
        IFarmerRepository farmerRepo,
        UserManager<ApplicationUser> userManager)
    {
        _productRepo = productRepo;
        _farmerRepo = farmerRepo;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? categoryFilter, DateTime? startDate, DateTime? endDate, int page = 1)
    {
        var user = await _userManager.GetUserAsync(User);
        int? farmerId = null;

        if (User.IsInRole(Roles.Farmer) && user != null)
        {
            var farmer = await _farmerRepo.GetByUserIdAsync(user.Id);
            farmerId = farmer?.FarmerId;
        }

        var (items, totalCount) = await _productRepo.GetPagedAsync(
            page, PageSize, farmerId, categoryFilter, startDate, endDate);

        ViewBag.Categories = await _productRepo.GetCategoriesAsync();
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        ViewBag.CategoryFilter = categoryFilter;
        ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

        return View(items);
    }

    [Authorize(Roles = Roles.Farmer)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Farmer)]
    public async Task<IActionResult> Create([Bind("Name,Category,ProductionDate,Description,Price,Quantity")] Product product)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var farmer = await _farmerRepo.GetByUserIdAsync(user.Id);
                if (farmer != null)
                {
                    product.FarmerId = farmer.FarmerId;
                    await _productRepo.AddAsync(product);
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        return View(product);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productRepo.GetByIdAsync(id.Value);
        if (product == null)
            return NotFound();

        return View(product);
    }
}
