using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards;
using Shards.DTOS.PurchaseOrderDtos;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class PurchaseOrderController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public PurchaseOrderController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/PurchaseOrder
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] PurchaseOrderSpecificationParameters parameters)
        {
            try
            {
                var purchaseOrders = await _serviceManger.PurchaseOrderService.GetAllPurchaseOrderAsync(parameters);

                // جلب Suppliers للـ Filter
                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();

                return View(purchaseOrders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading purchase orders: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/PurchaseOrder/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var purchaseOrder = await _serviceManger.PurchaseOrderService.GetPurchaseOrderByIdAsync(id);
                return View(purchaseOrder);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/PurchaseOrder/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // جلب Suppliers و Products للـ Dropdown
            ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
            ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                new ProductSpecificationParameters())).Data;

            // جلب User ID من الـ Claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CurrentUserId = userId;

            return View();
        }

        // POST: Admin/PurchaseOrder/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdatePurchaseOrderDto purchaseOrderDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(purchaseOrderDto);
            }

            try
            {
                // إضافة User ID الحالي
                purchaseOrderDto.ApplicationUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                await _serviceManger.PurchaseOrderService.CreatePurchaseOrderAsync(purchaseOrderDto);
                TempData["SuccessMessage"] = "Purchase order created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating purchase order: " + ex.Message);
                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(purchaseOrderDto);
            }
        }

        // GET: Admin/PurchaseOrder/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var purchaseOrder = await _serviceManger.PurchaseOrderService.GetPurchaseOrderByIdAsync(id);

                // تحويل PurchaseDetailsDto إلى CreateOrUpdatePurchaseOrderDto
                var purchaseOrderDto = new CreateOrUpdatePurchaseOrderDto
                {
                    Id = id,
                    InvoiceNumber = purchaseOrder.InvoiceNumber,
                    PaymentStatus = purchaseOrder.PaymentStatus,
                    OrderStatus = purchaseOrder.OrderStatus,
                    RemainingAmount = purchaseOrder.RemainingAmount,
                    SupplierId = purchaseOrder.SupplierId,
                    ApplicationUserId = purchaseOrder.ApplicationUserId,
                    PurchaseOrderDetails = purchaseOrder.PurchaseOrderDetails
                };

                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;

                return View(purchaseOrderDto);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/PurchaseOrder/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrUpdatePurchaseOrderDto purchaseOrderDto)
        {
            if (id != purchaseOrderDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid purchase order ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(purchaseOrderDto);
            }

            try
            {
                await _serviceManger.PurchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrderDto);
                TempData["SuccessMessage"] = "Purchase order updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating purchase order: " + ex.Message);
                ViewBag.Suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                ViewBag.Products = (await _serviceManger.ProductService.GetAllProductsAsync(
                    new ProductSpecificationParameters())).Data;
                return View(purchaseOrderDto);
            }
        }

        // GET: Admin/PurchaseOrder/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var purchaseOrder = await _serviceManger.PurchaseOrderService.GetPurchaseOrderByIdAsync(id);
                return View(purchaseOrder);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/PurchaseOrder/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.PurchaseOrderService.DeletePurchaseOrderAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Purchase order deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Purchase order not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting purchase order: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}