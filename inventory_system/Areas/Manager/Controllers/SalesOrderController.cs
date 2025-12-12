using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards;
using Shards.DTOS.SalesOrderDtos;

namespace YourProject.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    [Route("Manager/[controller]")]
    public class SalesOrderController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public SalesOrderController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Manager/SalesOrder
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] SalesOrderSpecificationParameters parameters)
        {
            try
            {
                var salesOrders = await _serviceManger.SalesOrderService.GetAllSalesOrderAsync(parameters);

                // جلب Customers للـ Filter
                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();

                return View(salesOrders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading sales orders: " + ex.Message;
                return View();
            }
        }

        // GET: Manager/SalesOrder/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var salesOrder = await _serviceManger.SalesOrderService.GetSalesOrderByIdAsync(id);
                return View(salesOrder);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Manager/SalesOrder/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // جلب Customers و Products للـ Dropdown
            ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
            ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                new ProductSpecificationParameters())).Data;

            // جلب User ID من الـ Claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CurrentUserId = userId;

            return View();
        }

        // POST: Manager/SalesOrder/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdateSalesOrderDto salesOrderDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(salesOrderDto);
            }

            try
            {
                // إضافة User ID الحالي
                salesOrderDto.ApplicationUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                await _serviceManger.SalesOrderService.CreateSalesOrderAsync(salesOrderDto);
                TempData["SuccessMessage"] = "Sales order created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating sales order: " + ex.Message);
                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(salesOrderDto);
            }
        }

        // GET: Manager/SalesOrder/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var salesOrder = await _serviceManger.SalesOrderService.GetSalesOrderByIdAsync(id);

                // تحويل SalesDetailsDto إلى CreateOrUpdateSalesOrderDto
                var salesOrderDto = new CreateOrUpdateSalesOrderDto
                {
                    Id = id,
                    InvoiceNumber = salesOrder.InvoiceNumber,
                    PaymentStatus = salesOrder.PaymentStatus,
                    OrderStatus = salesOrder.OrderStatus,
                    RemainingAmount = salesOrder.RemainingAmount,
                    CustomerId = salesOrder.CustomerId,
                    ApplicationUserId = salesOrder.ApplicationUserId,
                    SalesOrderDetails = salesOrder.SalesOrderDetails
                };

                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;

                return View(salesOrderDto);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Manager/SalesOrder/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrUpdateSalesOrderDto salesOrderDto)
        {
            if (id != salesOrderDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid sales order ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(salesOrderDto);
            }

            try
            {
                await _serviceManger.SalesOrderService.UpdateSalesOrderAsync(salesOrderDto);
                TempData["SuccessMessage"] = "Sales order updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating sales order: " + ex.Message);
                ViewBag.Customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(salesOrderDto);
            }
        }

        // GET: Manager/SalesOrder/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var salesOrder = await _serviceManger.SalesOrderService.GetSalesOrderByIdAsync(id);
                return View(salesOrder);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Manager/SalesOrder/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.SalesOrderService.DeleteSalesOrderAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Sales order deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Sales order not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting sales order: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}