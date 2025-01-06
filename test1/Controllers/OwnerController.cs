using Microsoft.AspNetCore.Mvc;
using test1.Models;
using System.Linq;



namespace test1.Controllers
{
    [Route("Owner")] // 指定控制器 路由前綴
    public class OwnerController : Controller
    {
        private readonly AppDbContext _context;

        public OwnerController(AppDbContext context)
        {
            _context = context;
        }

        // 飼主列表和搜尋
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index(string? phoneNumber)
        {
            var owners = string.IsNullOrEmpty(phoneNumber)
                ? _context.Owner.ToList()
                : _context.Owner.Where(o => o.PhoneNumber.Contains(phoneNumber)).ToList();
            return View(owners);
        }

        //新增飼主
        [HttpGet("Create")]
        public IActionResult Create()
        {
            // 初始化 Owner
            var owner = new Owner();

            return View(owner);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Owner owner)
        {
            if (owner == null)
            {
                ModelState.AddModelError(string.Empty, "提交的數據無效，請重試。");
                return View(new Owner());
            }

            if (ModelState.IsValid)
            {                try
                {
                    _context.Owner.Add(owner);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"新增資料失敗：{ex.Message}";
                    return View("Error");
                }
            }
            return View(owner);
        }
        // 編輯飼主
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var owner = _context.Owner.FirstOrDefault(o => o.OwnerID == id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Owner owner)
        {
            if (id != owner.OwnerID)
            {
                return BadRequest(); // 檢查路由
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Owner.Update(owner);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"更新資料失敗：{ex.Message}";
                    return View("Error");
                }
            }
            return View(owner); // 如果驗證失敗，返回表單
        }


        // 刪除飼主
        [HttpGet("DeleteConfirm/{id}")]
        public IActionResult Delete(int id)
        {
            var owner = _context.Owner.FirstOrDefault(o => o.OwnerID == id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        [HttpPost("DeleteConfirm/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var owner = _context.Owner.Find(id);
            if (owner != null)
            {
                _context.Owner.Remove(owner);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
