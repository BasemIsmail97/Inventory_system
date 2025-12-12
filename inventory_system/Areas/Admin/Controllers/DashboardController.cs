using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class DashboardController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public DashboardController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/Dashboard
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // جلب إحصائيات عامة
                var allProducts = await _serviceManger.ProductService.GetAllProductsAsync(
                    new Shards.ProductSpecificationParameters());

                var allCategories = await _serviceManger.CategoryService.GetAllCatygoriesAsync();
                var allSuppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                var allCustomers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();

                var allPurchaseOrders = await _serviceManger.PurchaseOrderService.GetAllPurchaseOrderAsync(
                    new Shards.PurchaseOrderSpecificationParameters());

                var allSalesOrders = await _serviceManger.SalesOrderService.GetAllSalesOrderAsync(
                    new Shards.SalesOrderSpecificationParameters());

                ViewBag.TotalProducts = allProducts.TotalCount;
                ViewBag.TotalCategories = allCategories.Count();
                ViewBag.TotalSuppliers = allSuppliers.Count();
                ViewBag.TotalCustomers = allCustomers.Count();
                ViewBag.TotalPurchaseOrders = allPurchaseOrders.TotalCount;
                ViewBag.TotalSalesOrders = allSalesOrders.TotalCount;
                ViewBag.TotalPurchaseAmount = allPurchaseOrders.Data.Sum(p => p.TotalAmount);
                ViewBag.TotalSalesAmount = allSalesOrders.Data.Sum(s => s.TotalAmount);

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading dashboard: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Dashboard/Statistics
        [HttpGet("Statistics")]
        public async Task<IActionResult> Statistics()
        {
            
            return View();
        }

        // GET: Admin/Dashboard/Reports
        [HttpGet("Reports")]
        public async Task<IActionResult> Reports()
        {
           
            return View();
        }
    }
}