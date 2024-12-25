using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUserController : Controller
    {
        private readonly PetShopContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public AdminUserController(PetShopContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/AdminUser
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminUsers
                .OrderBy(x => x.Use_ID)
                .ToListAsync());
        }

        // GET: Admin/AdminUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Use_ID == id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy dữ liệu!";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Admin/AdminUser/Create
        public IActionResult Create()
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            if (userInfo!.Status != 3)
            {
                TempData["Error"] = "Bạn không có quyền thực hiện hành động này!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Đang cập nhật!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AdminUser/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            if (userInfo!.Status != 3)
            {
                TempData["Error"] = "Bạn không có quyền thực hiện hành động này!";
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Use_ID == id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy dữ liệu!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Đang cập nhật!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AdminUser/Lock/5
        [HttpPost]
        public async Task<IActionResult> Lock(int id)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            if (userInfo == null || userInfo.Status != 3)
            {
                TempData["Error"] = "Bạn không có quyền thực hiện hành động này!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin tài khoản cần khóa
            var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Use_ID == id);
            if (user == null)
            {
                TempData["Error"] = "Tài khoản không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            if (user.Status == 0)
            {
                user.Status = 2;
                _context.AdminUsers.Update(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã kích hoạt khóa tài khoản \"{user.Username}\"!";

            }
            else if (user.Status == 1)
            {
                user.Status = 2;
                _context.AdminUsers.Update(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Tài khoản \"{user.Username}\" đã được mở khóa thành công !";

            }
            else if (user.Status == 2)
            {
                user.Status = 1;
                _context.AdminUsers.Update(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Tài khoản \"{user.Username}\" đã bị khóa!";

            }
            return RedirectToAction(nameof(Index));


        }
    }
}