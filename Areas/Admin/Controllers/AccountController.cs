using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Areas.Admin.DTOs.request;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly PetShopContext _context;

        public AccountController(PetShopContext context)
        {
            _context = context;
        }

        //================ GET:Login =====================//
        public ActionResult Login()
        {
            var login = Request.Cookies.Get<AdminUser>("UserCredential");
            if (login != null)
            {
                var user = _context.AdminUsers.AsNoTracking()
                    .FirstOrDefault(x => x.Username == login.Username && x.Token == login.Token);

                if (user != null)
                {
                    HttpContext.Session.Set<AdminUser>("userInfo", user);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto login)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.AdminUsers
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Username == login.Username);
                if (result == null || !BCrypt.Net.BCrypt.Verify(login.Password, result.Password))
                {
                    TempData["Error"] = "Tên tài khoản hoặc mật khẩu không chính xác!";
                    return View(login);
                }

                switch (result.Status)
                {
                    case 0:
                        TempData["Error"] = "Tài khoản chưa được kích hoạt!";
                        return View();

                    case 1:
                        TempData["Error"] = "Tài khoản của bạn đã bị khóa!";
                        return View();

                    default:

                        if (login.RememberMe)
                        {
                            var userCredential = new { result.Username, result.Token };
                            Response.Cookies.Append("UserCredential", JsonConvert.SerializeObject(userCredential), new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddHours(7),
                                HttpOnly = true,
                                IsEssential = true
                            });
                        }

                        HttpContext.Session.Set<AdminUser>("userInfo", result);

                        TempData["Success"] = "Đăng nhập thành công!";
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(login);
        }

        //================ Register =====================//
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại!";
                return View(request);
            }

            // Kiểm tra Username hoặc Email đã tồn tại
            var isExistingUser = await _context.AdminUsers
                .AsNoTracking()
                .AnyAsync(x => x.Username == request.Username || x.Email == request.Email);

            if (isExistingUser)
            {
                TempData["Error"] = "Tên tài khoản hoặc email đã tồn tại!";
                return View(request);
            }

            // Tạo tài khoản mới
            try
            {
                // Tạo Token
                var token = Guid.NewGuid().ToString();

                // Băm mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var newUser = new AdminUser
                {
                    Username = request.Username,

                    Password = hashedPassword,
                    Avatar = "default_avatar.jpg",
                    Token = token,
                    Email = request.Email,
                    Status = 0, // Chưa được kích hoạt
                    CreatedBy = request.Username,
                    CreatedDate = DateTime.Now,
                };

                _context.AdminUsers.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký thành công! Vui lòng chờ quản trị viên kích hoạt tài khoản.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View(request);
            }
        }

        //================ Register =====================//

        public IActionResult Logout()
        {
            // Xóa session người dùng
            HttpContext.Session.Remove("userInfo");

            // Xóa cookie UserCredential nếu có
            Response.Cookies.Delete("UserCredential");

            // Redirect về trang đăng nhập hoặc trang nào đó bạn muốn
            TempData["Success"] = "Đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
}