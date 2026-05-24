using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Roles.Employee)]
public class FarmersController : Controller
{
    private readonly IFarmerRepository _farmerRepo;
    private const int PageSize = 10;

    public FarmersController(IFarmerRepository farmerRepo)
    {
        _farmerRepo = farmerRepo;
    }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        var (items, totalCount) = await _farmerRepo.GetPagedAsync(page, PageSize, search);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        ViewBag.Search = search;

        return View(items);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FarmName,ContactPerson,PhoneNumber,Address,Email")] Farmer farmer)
    {
        if (ModelState.IsValid)
        {
            await _farmerRepo.AddAsync(farmer);
            return RedirectToAction(nameof(Index));
        }
        return View(farmer);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var farmer = await _farmerRepo.GetByIdWithProductsAsync(id.Value);
        if (farmer == null)
            return NotFound();

        return View(farmer);
    }
}
