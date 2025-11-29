using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;

using System.Text.Json;


namespace Presistence.Data
{
    public class DataSeeding(InventoryDbContext _dbContext ,UserManager<ApplicationUser>  _userManager ,RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task SeedDataAsync()
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();


                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
                #region Category Seeding
                if (!_dbContext.Categories.Any())
                {
                    var CatygoriesData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\categories_seed.json");
                    var Catygories = await JsonSerializer.DeserializeAsync<List<Category>>(CatygoriesData);
                    if (Catygories != null && Catygories.Any())
                    {
                        await _dbContext.Categories.AddRangeAsync(Catygories);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Suppliers seeding
                if (!_dbContext.Suppliers.Any())
                {

                    var SuppliersData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\suppliers_seed.json");
                    var Suppliers = await JsonSerializer.DeserializeAsync<List<Supplier>>(SuppliersData);
                    if (Suppliers != null && Suppliers.Any())
                    {
                        await _dbContext.Suppliers.AddRangeAsync(Suppliers);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Customer Seeding
                if (!_dbContext.Customers.Any())
                {
                    var CustomersData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\customers_seed.json");
                    var Customers = await JsonSerializer.DeserializeAsync<List<Customer>>(CustomersData);
                    if (Customers != null && Customers.Any())
                    {
                        await _dbContext.Customers.AddRangeAsync(Customers);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Products Seeding
                if (!_dbContext.Products.Any())
                {

                    var ProductsData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\products_seed.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products != null && Products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(Products);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Purchaseorders seeding
                if (!_dbContext.PurchaseOrders.Any())
                {

                    var PurchaseOrdersData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\purchaseorders_seed.json");
                    var PurchaseOrders = await JsonSerializer.DeserializeAsync<List<PurchaseOrder>>(PurchaseOrdersData);
                    if (PurchaseOrders != null && PurchaseOrders.Any())
                    {
                        await _dbContext.PurchaseOrders.AddRangeAsync(PurchaseOrders);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region PurchaseordersDetail seeding
                if (!_dbContext.PurchaseOrderDetails.Any())
                {

                    var PurchaseOrdersDetailData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\purchaseorderdetails_seed.json");
                    var PurchaseOrdersDetail = await JsonSerializer.DeserializeAsync<List<PurchaseOrderDetail>>(PurchaseOrdersDetailData);
                    if (PurchaseOrdersDetail != null && PurchaseOrdersDetail.Any())
                    {
                        await _dbContext.PurchaseOrderDetails.AddRangeAsync(PurchaseOrdersDetail);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Salesorder seeding
                if (!_dbContext.SalesOrders.Any())
                {

                    var SalesOrdersData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\salesorders_seed.json");
                    var SalesOrders = await JsonSerializer.DeserializeAsync<List<SalesOrder>>(SalesOrdersData);
                    if (SalesOrders != null && SalesOrders.Any())
                    {
                        await _dbContext.SalesOrders.AddRangeAsync(SalesOrders);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion
                #region Salesorder detail seeding
                if (!_dbContext.SalesOrderDetails.Any())
                {

                    var SalesOrdersDetailData = File.OpenRead("..\\infrastructure\\Presistence\\Data\\DataSeding\\salesorderdetails_seed.json");
                    var SalesOrdersDetail = await JsonSerializer.DeserializeAsync<List<SalesOrderDetail>>(SalesOrdersDetailData);
                    if (SalesOrdersDetail != null && SalesOrdersDetail.Any())
                    {
                        await _dbContext.SalesOrderDetails.AddRangeAsync(SalesOrdersDetail);
                    }
                    await _dbContext.SaveChangesAsync();

                }
                #endregion



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during seeding: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw; 
            }

        }

        public async Task SeedIdentityDataAsync()
        {
            try
            {


                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admain"));
                    await _roleManager.CreateAsync(new IdentityRole("Manger"));
                    await _roleManager.CreateAsync(new IdentityRole("Employee"));
                }
                if (!_userManager.Users.Any())
                {
                    var AdminUser = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Ali",
                        UserName = "AhmedAli",
                        Email = "Ahmed_Ali@gmail.com",
                        PhoneNumber = "01124598756"

                    };
                    var MangerUser = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Mahmoud",
                        UserName = "MohamedMahmoud",
                        Email = "Mohamed_Mahmoud@gmail.com",
                        PhoneNumber = "01124592746"

                    };
                    var EmployeeUser = new ApplicationUser()
                    {
                        FirstName = "Samir",
                        LastName = "Ahmed",
                        UserName = "SamirAhmed",
                        Email = "Samir_Ahmed@gmail.com",
                        PhoneNumber = "01124592766"

                    };
                    await _userManager.CreateAsync(AdminUser, "Abc&&123");
                    await _userManager.CreateAsync(MangerUser, "Abc&&123");
                    await _userManager.CreateAsync(EmployeeUser, "Abc&&123");

                    await _userManager.AddToRoleAsync(AdminUser, "Admain");
                    await _userManager.AddToRoleAsync(MangerUser, "Manger");
                    await _userManager.AddToRoleAsync(EmployeeUser, "Employee");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during identity seeding: {ex.Message}");
                throw;
            }
            
        }
    }
}
