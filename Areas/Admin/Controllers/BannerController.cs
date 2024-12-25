using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Areas.Admin.DTOs.request;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly PetShopContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public BannerController(PetShopContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Banner
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banners
                .OrderBy(x => x.Ban_ID)
                .ToListAsync());
        }

        // GET: Admin/Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID Banner không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Ban_ID == id);
            if (banner == null)
            {
                TempData["Error"] = "Không tìm thấy Banner!";
                return RedirectToAction(nameof(Index));
            }

            return View(banner);
        }

        // GET: Admin/Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BannerDto request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";
            if (ModelState.IsValid)
            {
                try
                {
                    var banner = new Banner
                    {
                        Ban_ID = request.Ban_ID,
                        Title = request.Title,
                        DisplayOrder = request.DisplayOrder,
                        DisplayPosition = request.DisplayPosition,
                        Url = request.Url,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now
                    };

                    if (request.Image != null)
                    {
                        banner.Image = await HandleFileUpload(request.Image);
                    }

                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Banner đã được tạo thành công!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi khi tạo Banner: {ex.Message}";
                }
                return RedirectToAction(nameof(Index));
            }
            else TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại!";
            return View(request);
        }

        // GET: Admin/Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID Banner không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["Error"] = "Không tìm thấy Banner!";
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BannerDto request)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["Error"] = "Không tìm thấy Banner để chỉnh sửa!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (request.Image != null)
                    {
                        if (!string.IsNullOrEmpty(banner.Image))
                        {
                            await DeleteFile(banner.Image);
                        }

                        banner.Image = await HandleFileUpload(request.Image);
                    }
                    if (request.Image == null && string.IsNullOrEmpty(banner.Image))
                    {
                        ModelState.AddModelError("Image", "Hình ảnh là bắt buộc.");
                        return View(request);
                    }

                    var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
                    var userName = userInfo?.Username ?? "Unknown";

                    banner.Title = request.Title;
                    banner.DisplayOrder = request.DisplayOrder;
                    banner.Url = request.Url;
                    banner.DisplayPosition = request.DisplayPosition;
                    banner.UpdatedBy = userName;
                    banner.UpdatedDate = DateTime.Now;

                    _context.Update(banner);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật Banner thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật banner: {ex.Message}";
                }
            }
            return View(request);
        }

        // GET: Admin/Banner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID Banner không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Ban_ID == id);
            if (banner == null)
            {
                TempData["Error"] = "Không tìm thấy Banner!";
                return RedirectToAction(nameof(Index));
            }

            return View(banner);
        }

        // POST: Admin/Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);

            if (banner != null)
            {
                if (!string.IsNullOrEmpty(banner.Image))
                {
                    await DeleteFile(banner.Image);
                }
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Banner đã được xóa thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy Banner để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> HandleFileUpload(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "banners", newFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return newFileName;
        }

        private async Task DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_hostEnv.WebRootPath, "data", "banners", fileName);

            if (System.IO.File.Exists(filePath))
            {
                await Task.Run(() => System.IO.File.Delete(filePath));
            }
        }
    }
}