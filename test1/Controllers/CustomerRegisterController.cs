using Microsoft.AspNetCore.Mvc;
using test1.Models;
using System.IO;

namespace test1.Controllers
{
    public class CustomerRegisterController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerRegisterController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; // 獲取 wwwroot 路徑
        }

        public IActionResult Create()
        {
            var viewModel = new RegisterViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterViewModel viewModel, IFormFile? petPhoto)
        {
            if (ModelState.IsValid)
            {
                // 預設密碼
                if (string.IsNullOrWhiteSpace(viewModel.Owner.Password))
                {
                    viewModel.Owner.Password = "default_password"; // 預設值
                }

                // 儲存 Owner
                _context.Owner.Add(viewModel.Owner);
                _context.SaveChanges();

                // 圖片上傳
                ProcessImageUpload(petPhoto, viewModel.Pet);

                // 儲存 Pet
                viewModel.Pet.OwnerID = viewModel.Owner.OwnerID;
                _context.Pet.Add(viewModel.Pet);
                _context.SaveChanges();

                return RedirectToAction("Index", "CustomerLogIn");
            }

            return View(viewModel);
        }

        // 圖片上傳
        private void ProcessImageUpload(IFormFile? imageFile, Pet pet)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // 圖片儲存邏輯
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                pet.ImagePath = "/Uploads/" + fileName;
            }
            else
            {
                // 預設圖片
                pet.ImagePath = "/Uploads/pet-placeholder.jpg"; 
            }
        }

    }
}
