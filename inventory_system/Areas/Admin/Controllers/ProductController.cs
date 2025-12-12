using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards;
using Shards.DTOS.ProductDtos;


namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public ProductController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/Product
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] ProductSpecificationParameters parameters)
        {
            try
            {
                var products = await _serviceManger.ProductService.GetAllProductsAsync(parameters);

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading products: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Product/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _serviceManger.ProductService.GetProductByIdAsync(id);
                return View(product);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Product/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
          
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
               
                return View(productDto);
            }

            try
            {
                await _serviceManger.ProductService.CreateProductAsync(productDto);
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating product: " + ex.Message);
               
                return View(productDto);
            }
        }

        // GET: Admin/Product/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _serviceManger.ProductService.GetProductByIdAsync(id);

                // تحويل ProductDetailsDto إلى CreateOrUpdateProductDto
                var productDto = new CreateOrUpdateProductDto
                {
                    Id = id,
                    Name = product.Name,
                    Description = product.Description,
                    QuantityInStock = product.QuantityInStock,
                    MinimumStockLevel = product.MinimumStockLevel,
                    PictureUrl = product.PictureUrl,
                    CategoryId = product.CategoryId,
                    SupplierId = product.SupplierId
                };

               
                return View(productDto);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Product/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrUpdateProductDto productDto)
        {
            if (id != productDto.Id)
            {
                TempData["ErrorMessage"] = "Invalid product ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                
                return View(productDto);
            }

            try
            {
                await _serviceManger.ProductService.UpdateProductAsync(productDto);
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating product: " + ex.Message);
               
                return View(productDto);
            }
        }

        // GET: Admin/Product/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _serviceManger.ProductService.GetProductByIdAsync(id);
                return View(product);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Product/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _serviceManger.ProductService.DeleteProductAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Product deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Product not found";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting product: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}