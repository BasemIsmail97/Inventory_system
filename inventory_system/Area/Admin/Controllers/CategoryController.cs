using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.CategoryDtos;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public CategoryController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/Category
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _serviceManger.CategoryService.GetAllCatygoriesAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading categories: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Category/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _serviceManger.CategoryService.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Category/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return View(categoryDto);

            try
            {
                await _serviceManger.CategoryService.CreateCategoryAsync(categoryDto);
                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating category: " + ex.Message);
                return View(categoryDto);
            }
        }

        // GET: Admin/Category/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _serviceManger.CategoryService.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Category/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid category ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(categoryDto);

            try
            {
                await _serviceManger.CategoryService.UpdateCategoryAsync(categoryDto);
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating category: " + ex.Message);
                return View(categoryDto);
            }
        }

        // GET: Admin/Category/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _serviceManger.CategoryService.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Category/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.CategoryService.DeleteCategoryAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Category deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Category not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting category: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}