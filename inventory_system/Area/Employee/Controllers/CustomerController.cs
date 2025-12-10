using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.CustomerDtos;

namespace YourProject.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    [Route("Employee/[controller]")]
    public class CustomerController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public CustomerController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Employee/Customer
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _serviceManger.CustomerService.GetAllCatygoriesAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading customers: " + ex.Message;
                return View();
            }
        }

        // GET: Employee/Customer/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var customer = await _serviceManger.CustomerService.GetCategoryByIdAsync(id);
                return View(customer);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Employee/Customer/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Customer/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return View(customerDto);

            try
            {
                await _serviceManger.CustomerService.CreateCategoryAsync(customerDto);
                TempData["SuccessMessage"] = "Customer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating customer: " + ex.Message);
                return View(customerDto);
            }
        }

        // GET: Employee/Customer/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await _serviceManger.CustomerService.GetCategoryByIdAsync(id);
                return View(customer);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Employee/Customer/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid customer ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(customerDto);

            try
            {
                await _serviceManger.CustomerService.UpdateCategoryAsync(customerDto);
                TempData["SuccessMessage"] = "Customer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating customer: " + ex.Message);
                return View(customerDto);
            }
        }

        // GET: Employee/Customer/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customer = await _serviceManger.CustomerService.GetCategoryByIdAsync(id);
                return View(customer);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Employee/Customer/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.CustomerService.DeleteCategoryAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Customer deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Customer not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting customer: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}