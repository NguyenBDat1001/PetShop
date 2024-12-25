using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypeController : Controller
    {
        private readonly PetShopContext _context;

        public TypeController(PetShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Type
        public async Task<IActionResult> Index()
        {
            return View(await _context.Types
                .Include(p => p.Products)
                .OrderBy(x => x.Typ_ID)
                .ToListAsync());
        }

        // GET: Admin/Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID loại không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var type = await _context.Types
                .FirstOrDefaultAsync(m => m.Typ_ID == id);
            if (type == null)
            {
                TempData["Error"] = "Không tìm thấy loại.";
                return RedirectToAction(nameof(Index));
            }

            return View(type);
        }

        // GET: Admin/Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Type/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Models.Type type)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";
            if (ModelState.IsValid)
            {
                try
                {
                    type.CreatedBy = userName;
                    type.CreatedDate = DateTime.Now;

                    _context.Add(type);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Loại đã được tạo thành công!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Đã xảy ra lỗi khi tạo loại.";
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
            return View(type);
        }

        // GET: Admin/Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID loại không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var type = await _context.Types.FindAsync(id);
            if (type == null)
            {
                TempData["Error"] = "Không tìm thấy loại.";
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        // POST: Admin/Type/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Typ_ID,Name,DisplayOrder")] Models.Type type)
        {
            var existingType = await _context.Types.FindAsync(id);
            if (existingType == null)
            {
                TempData["Error"] = "Không tìm thấy loại để chỉnh sửa.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (existingType != null)
                    {
                        var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                        var userName = userInfo?.Username ?? "Unknown";

                        existingType.Name = type.Name;
                        existingType.DisplayOrder = type.DisplayOrder;
                        existingType.UpdatedBy = userName;
                        existingType.UpdatedDate = DateTime.Now;

                        _context.Update(existingType);
                        await _context.SaveChangesAsync();

                        TempData["Success"] = "Loại đã được cập nhật thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật loại: {ex.Message}";
                }
            }
            return View(type);
        }

        // GET: Admin/Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID loại không được cung cấp.";
                return RedirectToAction(nameof(Index));
            }

            var type = await _context.Types
                .FirstOrDefaultAsync(m => m.Typ_ID == id);
            if (type == null)
            {
                TempData["Error"] = "Không tìm thấy loại.";
                return RedirectToAction(nameof(Index));
            }

            return View(type);
        }

        // POST: Admin/Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.Types.FindAsync(id);
            try
            {
                if (type != null)
                {
                    _context.Types.Remove(type);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Loại đã được xóa thành công!";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Không thể xóa loại vì có dữ liệu liên quan.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}