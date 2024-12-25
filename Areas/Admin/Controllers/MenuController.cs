using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly PetShopContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public MenuController(PetShopContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Menu
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus
                .Include(m => m.Parent)
                .Include(m => m.Children)
                .ToListAsync();
            return View(menus);
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID  không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var menu = await _context.Menus
                .Include(m => m.Parent)
                .Include(m => m.Children)
                .FirstOrDefaultAsync(m => m.Men_ID == id);

            if (menu == null)
            {
                TempData["Error"] = "Không tìm thấy Menu!";
                return RedirectToAction(nameof(Index));
            }

            return View(menu);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            // Lấy danh sách menu cha (chỉ các menu không có cha hoặc tất cả các menu)
            var menus = _context.Menus
                .Where(m => m.Parent_ID == null) // Lọc menu gốc (không có cha)
                .Select(m => new SelectListItem
                {
                    Value = m.Men_ID.ToString(),
                    Text = m.Title
                }).ToList();

            // Thêm tùy chọn không có menu cha
            menus.Insert(0, new SelectListItem { Value = "0", Text = "Không có Menu cha" });

            ViewBag.Men_ID = menus;

            return View();
        }

        // GET: Admin/Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                TempData["Error"] = "Không tìm thấy Menu!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy danh sách menu cha (loại bỏ chính menu hiện tại)
            var menus = _context.Menus
                .Where(m => m.Parent_ID == null || m.Men_ID != id) // Không cho chính nó làm cha
                .Select(m => new SelectListItem
                {
                    Value = m.Men_ID.ToString(),
                    Text = m.Title
                }).ToList();

            // Thêm tùy chọn "Không có Menu cha"
            menus.Insert(0, new SelectListItem { Value = "0", Text = "Không có Menu cha" });

            ViewBag.Men_ID = menus;
            return View(menu);
        }


        // POST: Admin/Type/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Parent_ID, Title, Url, DisplayOrder")] Menu menu)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";
            if (ModelState.IsValid)
            {
                try
                {
                    if (menu.Parent_ID == 0)
                    {
                        menu.Parent_ID = null;
                    }
                    menu.CreatedBy = userName;
                    menu.CreatedDate = DateTime.Now;

                    _context.Add(menu);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Menu đã được tạo thành công!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Đã xảy ra lỗi khi tạo Menu.";
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
            var menus = _context.Menus
               .Where(m => m.Parent_ID == null)
               .Select(m => new SelectListItem
               {
                   Value = m.Men_ID.ToString(),
                   Text = m.Title
               }).ToList();

            menus.Insert(0, new SelectListItem { Value = "0", Text = "Không có Menu cha" });

            ViewBag.Men_ID = menus;
            return View(menu);
        }
        // POST: Admin/Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Men_ID, Parent_ID, Title, Url, DisplayOrder")] Menu menu)
        {
            if (id != menu.Men_ID)
            {
                TempData["Error"] = "ID không khớp!";
                return RedirectToAction(nameof(Index));
            }

            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";

            if (ModelState.IsValid)
            {
                try
                {
                    if (menu.Parent_ID == 0)
                    {
                        menu.Parent_ID = null;
                    }

                    menu.UpdatedBy = userName;
                    menu.UpdatedDate = DateTime.Now;

                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Menu đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Men_ID))
                    {
                        TempData["Error"] = "Không tìm thấy Menu để cập nhật!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Đã xảy ra lỗi khi cập nhật Menu.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";

            // Reload danh sách menu cha
            var menus = _context.Menus
                .Where(m => m.Parent_ID == null || m.Men_ID != id)
                .Select(m => new SelectListItem
                {
                    Value = m.Men_ID.ToString(),
                    Text = m.Title
                }).ToList();

            menus.Insert(0, new SelectListItem { Value = "0", Text = "Không có Menu cha" });

            ViewBag.Men_ID = menus;
            return View(menu);
        }

        // Kiểm tra menu có tồn tại hay không
        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Men_ID == id);
        }

    }
}