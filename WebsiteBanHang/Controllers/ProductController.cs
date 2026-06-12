using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers; // Đã sửa lại đúng chuẩn

public class ProductController : Controller
{
	private readonly IProductRepository _productRepository;
	private readonly ICategoryRepository _categoryRepository;

	public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
	{
		_productRepository = productRepository;
		_categoryRepository = categoryRepository;
	}

	public IActionResult Add()
	{
		var categories = _categoryRepository.GetAllCategories();
		ViewBag.Categories = new SelectList(categories, "Id", "Name");
		return View();
	}

	// Đã xóa hàm [HttpPost] Add bị trùng ở đây

	public IActionResult Index()
	{
		var products = _productRepository.GetAll();
		return View(products);
	}

	public IActionResult Display(int id)
	{
		var product = _productRepository.GetById(id);
		if (product == null) return NotFound();
		return View(product);
	}

/*	public IActionResult Update(int id)
	{
		var product = _productRepository.GetById(id);
		if (product == null) return NotFound();
		return View(product);
	}

	[HttpPost]
	public IActionResult Update(Product product)
	{
		if (ModelState.IsValid)
		{
			_productRepository.Update(product);
			return RedirectToAction("Index");
		}
		return View(product);
	}*/

	public IActionResult Delete(int id)
	{
		var product = _productRepository.GetById(id);
		if (product == null) return NotFound();
		return View(product);
	}

	[HttpPost, ActionName("Delete")]
	public IActionResult DeleteConfirmed(int id)
	{
		_productRepository.Delete(id);
		return RedirectToAction("Index");
	}

	// Hàm Add xử lý upload hình ảnh 
	[HttpPost]
	public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
	{
		if (ModelState.IsValid)
		{
			if (imageUrl != null)
			{
				product.ImageUrl = await SaveImage(imageUrl);
			}

			if (imageUrls != null)
			{
				product.ImageUrls = new List<string>();
				foreach (var file in imageUrls)
				{
					product.ImageUrls.Add(await SaveImage(file));
				}
			}

			_productRepository.Add(product);
			return RedirectToAction("Index");
		}

		return View(product);
	}

	private async Task<string> SaveImage(IFormFile image)
	{
		var savePath = Path.Combine("wwwroot/images", image.FileName);
		using (var fileStream = new FileStream(savePath, FileMode.Create))
		{
			await image.CopyToAsync(fileStream);
		}
		return "/images/" + image.FileName;
	}

    // Hiển thị biểu mẫu cập nhật sản phẩm
    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null) return NotFound();

        // Load danh sách Category
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

        return View(product);
    }

    // Tiến hành cập nhật sản phẩm
    [HttpPost]
    public IActionResult Update(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.Update(product);
            return RedirectToAction("Index");
        }

        // Load lại danh sách Category nếu form bị lỗi
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

        return View(product);
    }
}