using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Areas.Admin.DTOs.request;
using PetShop.Models;
using PetShop.Utils;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly PetShopContext _context;

        public OrderController(PetShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var petShopContext = _context.Orders.Include(o => o.Member).OrderBy(x => x.Ord_ID);
            return View(await petShopContext.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID đơn hàng không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }
            var order = await _context.Orders
                .Include(o => o.Member)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Ord_ID == id);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["Mem_ID"] = new SelectList(_context.Members.OrderBy(x => x.Name), "Mem_ID", "Name");
            ViewData["products"] = _context.Products.AsNoTracking().OrderBy(x => x.Name).ToList();
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>()
            };
            return View(order);
        }

        // POST: Admin/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] OrderDto request)
        {
            var userInfo = HttpContext.Session.Get<AdminUser>("userInfo");
            var userName = userInfo?.Username ?? "Unknown";

            if (request.OrderDetails == null || !request.OrderDetails.Any())
            {
                TempData["Error"] = "Bạn phải thêm ít nhất một chi tiết đơn hàng.";
                ViewData["Mem_ID"] = new SelectList(_context.Members.OrderBy(x => x.Name), "Mem_ID", "Name", request.Mem_ID);
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var order = new Order
                    {
                        Ord_ID = request.Ord_ID,
                        Mem_ID = request.Mem_ID,
                        CustomerName = request.CustomerName,
                        Phone = request.Phone,
                        Address = request.Address,
                        Discount = request.Discount,
                        PaymentMethod = request.PaymentMethod,
                        IsPaid = request.IsPaid,
                        Note = request.Note,
                        Status = request.Status,
                        OrderDetails = request.OrderDetails ?? new List<OrderDetail>(),

                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                    };

                    if (request.OrderDetails != null && request.OrderDetails.Any())
                    {
                        order.TotalPrice = request.OrderDetails.Sum(od => od.Price * od.Quantity);
                    }

                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo đơn hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi tạo đơn hàng: {ex.Message}";
                }
            }

            ViewData["Mem_ID"] = new SelectList(_context.Members.OrderBy(x => x.Name), "Mem_ID", "Name", request.Mem_ID);
            return View(request);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID đơn hàng không được cung cấp!";
                return RedirectToAction(nameof(Index));
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Mem_ID"] = new SelectList(_context.Members, "Mem_ID", "Email", order.Mem_ID);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ord_ID,Mem_ID,OrderDate,CustomerName,Phone,Address,TotalPrice,Discount,PaymentMethod,IsPaid,Note,Status,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Order order)
        {
            if (id != order.Ord_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Ord_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Mem_ID"] = new SelectList(_context.Members, "Mem_ID", "Email", order.Mem_ID);
            return View(order);
        }



        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Ord_ID == id);
        }
    }
}