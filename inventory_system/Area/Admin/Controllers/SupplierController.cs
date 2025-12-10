using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.SpplierDtos;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class SupplierController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public SupplierController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/Supplier
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var suppliers = await _serviceManger.SupplierService.GetAllSuppliersAsync();
                return View(suppliers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading suppliers: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Supplier/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var supplier = await _serviceManger.SupplierService.GetSupplierByIdAsync(id);
                return View(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Supplier/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Supplier/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return View(supplierDto);

            try
            {
                await _serviceManger.SupplierService.CreateSupplierAsync(supplierDto);
                TempData["SuccessMessage"] = "Supplier created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating supplier: " + ex.Message);
                return View(supplierDto);
            }
        }

        // GET: Admin/Supplier/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var supplier = await _serviceManger.SupplierService.GetSupplierByIdAsync(id);
                return View(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierDto supplierDto)
        {
            if (id != supplierDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid supplier ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(supplierDto);

            try
            {
                await _serviceManger.SupplierService.UpdateSupplierAsync(supplierDto);
                TempData["SuccessMessage"] = "Supplier updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating supplier: " + ex.Message);
                return View(supplierDto);
            }
        }

        // GET: Admin/Supplier/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var supplier = await _serviceManger.SupplierService.GetSupplierByIdAsync(id);
                return View(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.SupplierService.DeleteSupplierAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Supplier deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Supplier not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting supplier: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}