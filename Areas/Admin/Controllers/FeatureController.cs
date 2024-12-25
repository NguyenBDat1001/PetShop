using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly PetShopContext _context;

        public FeatureController(PetShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Feature
        public async Task<IActionResult> Index()
        {
            return View(await _context.Features
                .OrderBy(x => x.Fea_ID)
                .ToListAsync());
        }

        // GET: Admin/Feature/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.Fea_ID == id);

            if (feature == null)
            {
                TempData["Error"] = "Không tìm thấy Feature!";
                return RedirectToAction(nameof(Index));
            }

            return View(feature);
        }

        // GET: Admin/Feature/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Feature/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Feature feature)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    feature.CreatedDate = DateTime.Now;
                    feature.CreatedBy = HttpContext.Session.Get<AdminUser>("userInfo")?.Username ?? "Unknown";

                    _context.Add(feature);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Feature đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi khi tạo Feature: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại!";
            }

            return View(feature);
        }

        // GET: Admin/Feature/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var feature = await _context.Features.FindAsync(id);

            if (feature == null)
            {
                TempData["Error"] = "Không tìm thấy Feature!";
                return RedirectToAction(nameof(Index));
            }

            return View(feature);
        }

        // POST: Admin/Feature/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Feature feature)
        {
            var existingFeature = await _context.Features.FindAsync(id);
            if (existingFeature == null)
            {
                TempData["Error"] = "Không tìm thấy Feature để chỉnh sửa!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingFeature.Icon = feature.Icon;
                    existingFeature.Title = feature.Title;
                    existingFeature.Subtitle = feature.Subtitle;
                    existingFeature.DisplayOrder = feature.DisplayOrder;
                    existingFeature.UpdatedDate = DateTime.Now;
                    existingFeature.UpdatedBy = HttpContext.Session.Get<AdminUser>("userInfo")?.Username ?? "Unknown";

                    _context.Update(existingFeature);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật Feature thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi khi cập nhật: {ex.Message}";
                }
            }

            return View(feature);
        }

        // GET: Admin/Feature/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var feature = await _context.Features
                .FirstOrDefaultAsync(m => m.Fea_ID == id);

            if (feature == null)
            {
                TempData["Error"] = "Không tìm thấy Feature!";
                return RedirectToAction(nameof(Index));
            }

            return View(feature);
        }

        // POST: Admin/Feature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _context.Features.FindAsync(id);

            if (feature != null)
            {
                _context.Features.Remove(feature);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Feature đã được xóa thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy Feature để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}