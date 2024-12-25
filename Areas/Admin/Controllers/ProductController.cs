using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Areas.Admin.DTOs.request;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly PetShopContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public ProductController(PetShopContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(c => c.Category)
                .Include(t => t.Type);
            return View(await products.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID sản phẩm không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products
                 .Include(c => c.Category)
                 .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.Pro_ID == id);

            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["Cat_ID"] = new SelectList(_context.Categories.OrderBy(x => x.Name), "Cat_ID", "Name");
            ViewData["Typ_ID"] = new SelectList(_context.Types.OrderBy(x => x.Name), "Typ_ID", "Name");

            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDto request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";

            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product
                    {
                        Pro_ID = request.Pro_ID,
                        Cat_ID = request.Cat_ID,
                        Typ_ID = request.Typ_ID,
                        Name = request.Name,
                        Brand = request.Brand,
                        Intro = request.Intro,
                        Price = request.Price,
                        Quantity = request.Quantity,
                        Discount = request.Discount,
                        Unit = request.Unit,
                        Rate = request.Rate,
                        Description = request.Description,
                        Details = request.Details,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now
                    };

                    if (request.Avatar != null)
                    {
                        product.Avatar = await HandleFileUpload(request.Avatar);
                    }
                    else product.Avatar = "default_product.png";

                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi thêm sản phẩm: {ex.Message}";
                }
            }

            ViewData["Cat_ID"] = new SelectList(_context.Categories.OrderBy(x => x.Name), "Cat_ID", "Name", request.Cat_ID);
            ViewData["Typ_ID"] = new SelectList(_context.Types.OrderBy(x => x.Name), "Typ_ID", "Name", request.Typ_ID);

            return View(request);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID sản phẩm không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Cat_ID"] = new SelectList(_context.Categories.OrderBy(x => x.Name), "Cat_ID", "Name", product.Cat_ID);
            ViewData["Typ_ID"] = new SelectList(_context.Types.OrderBy(x => x.Name), "Typ_ID", "Name", product.Typ_ID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductDto request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm để chỉnh sửa!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (request.Avatar != null)
                    {
                        if (!string.IsNullOrEmpty(product.Avatar))
                        {
                            await DeleteFile(product.Avatar);
                        }

                        product.Avatar = await HandleFileUpload(request.Avatar);
                    }

                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo?.Username ?? "Unknown";

                    product.Cat_ID = request.Cat_ID;
                    product.Typ_ID = request.Typ_ID;
                    product.Name = request.Name;
                    product.Intro = request.Intro;
                    product.Price = request.Price;
                    product.Brand = request.Brand;
                    product.Quantity = request.Quantity;
                    product.Discount = request.Discount;
                    product.Unit = request.Unit;
                    product.Rate = request.Rate;
                    product.Description = request.Description;
                    product.Details = request.Details;

                    product.UpdatedBy = userName;
                    product.UpdatedDate = DateTime.Now;

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật sản phẩm: {ex.Message}";
                }
            }

            ViewData["Cat_ID"] = new SelectList(_context.Categories.OrderBy(x => x.Name), "Cat_ID", "Name", request.Cat_ID);
            ViewData["Typ_ID"] = new SelectList(_context.Types.OrderBy(x => x.Name), "Typ_ID", "Name", request.Typ_ID);

            return View(request);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID sản phẩm không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products
                .Include(c => c.Category)
                .Include(t => t.Type)
                .FirstOrDefaultAsync(m => m.Pro_ID == id);

            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Avatar))
                {
                    await DeleteFile(product.Avatar);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sản phẩm đã được xóa thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> HandleFileUpload(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "products", newFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return newFileName;
        }

        private async Task DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "products", fileName);

            if (System.IO.File.Exists(filePath))
            {
                await Task.Run(() => System.IO.File.Delete(filePath));
            }
        }
    }
}