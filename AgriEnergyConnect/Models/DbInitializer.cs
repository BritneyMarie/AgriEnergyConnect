using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task Initialize(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        context.Database.EnsureCreated();

        if (!await roleManager.RoleExistsAsync(Roles.Farmer))
            await roleManager.CreateAsync(new IdentityRole(Roles.Farmer));

        if (!await roleManager.RoleExistsAsync(Roles.Employee))
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee));

        if (context.Users.Any())
            return;

        var employeeUser = new ApplicationUser
        {
            UserName = "employee@agrienergy.com",
            Email = "employee@agrienergy.com",
            FirstName = "John",
            LastName = "Employee",
            IsEmployee = true,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(employeeUser, "Employee123!");
        await userManager.AddToRoleAsync(employeeUser, Roles.Employee);

        var farmerUser = new ApplicationUser
        {
            UserName = "farmer@agrienergy.com",
            Email = "farmer@agrienergy.com",
            FirstName = "Jane",
            LastName = "Farmer",
            IsFarmer = true,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(farmerUser, "Farmer123!");
        await userManager.AddToRoleAsync(farmerUser, Roles.Farmer);

        var farmer = new Farmer
        {
            FarmName = "Green Valley Farms",
            ContactPerson = "Jane Farmer",
            PhoneNumber = "555-123-4567",
            Address = "123 Farm Road, Ruralville",
            Email = "farmer@agrienergy.com",
            UserId = farmerUser.Id
        };
        context.Farmers.Add(farmer);

        var products = new List<Product>
        {
            new Product {
                Name = "Organic Wheat",
                Category = "Grains",
                ProductionDate = DateTime.Now.AddDays(-30),
                Description = "High-quality organic wheat",
                Price = 5.99m,
                Quantity = 1000,
                Farmer = farmer
            },
            new Product {
                Name = "Free-range Eggs",
                Category = "Dairy",
                ProductionDate = DateTime.Now.AddDays(-7),
                Description = "Fresh free-range eggs",
                Price = 3.50m,
                Quantity = 200,
                Farmer = farmer
            }
        };
        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
