using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly PetShopContext _context;

        public CategoryController(PetShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories
                .Include(p => p.Products)
                .OrderBy(x => x.Cat_ID)
                .ToListAsync());
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID danh mục không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cat_ID == id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy danh mục.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";
            if (ModelState.IsValid)
            {
                try
                {
                    category.CreatedBy = userName;
                    category.CreatedDate = DateTime.Now;

                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Danh mục đã được tạo thành công!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Đã xảy ra lỗi khi tạo danh mục.";
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID danh mục không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy danh mục.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cat_ID,Name,DisplayOrder")] Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                TempData["Error"] = "Không tìm thấy danh mục để chỉnh sửa.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (existingCategory != null)
                    {
                        var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                        var userName = userInfo?.Username ?? "Unknown";

                        existingCategory.Name = category.Name;
                        existingCategory.DisplayOrder = category.DisplayOrder;
                        existingCategory.UpdatedBy = userName;
                        existingCategory.UpdatedDate = DateTime.Now;

                        _context.Update(existingCategory);
                        await _context.SaveChangesAsync();

                        TempData["Success"] = "Danh mục đã được cập nhật thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật danh mục: {ex.Message}";
                }
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID danh mục không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cat_ID == id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy danh mục.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            try
            {
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Danh mục đã được xóa thành công!";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Không thể xóa danh mục vì có dữ liệu liên quan.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}