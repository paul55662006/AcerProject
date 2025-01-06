using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using test1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace test1.Controllers
{
    [Route("Pet")]
    public class PetController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PetController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("")]
        [HttpGet("Index")]
        // 寵物列表
        public IActionResult Index(int ownerId)
        {
            var petsForView = _context.Pet.Where(p => p.OwnerID == ownerId).ToList();
            ViewBag.OwnerID = ownerId;
            return View(petsForView);
        }




        // 新增寵物
        [HttpGet("Create")]
        public IActionResult Create(int ownerId)
        {
            var owner = _context.Owner.Find(ownerId);
            if (owner == null)
            {
                return NotFound();
            }
            ViewBag.OwnerID = ownerId;
            LoadDropdownOptions();
            return View();
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pet pet, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var owner = _context.Owner.Find(pet.OwnerID);
                if (owner == null)
                {
                    ModelState.AddModelError("Owner", "Invalid OwnerID");
                    LoadDropdownOptions();
                    return View(pet);
                }

                pet.Owner = owner;

                // 圖片上傳
                ProcessImageUpload(imageFile, pet);

                _context.Pet.Add(pet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { ownerId = pet.OwnerID });
            }

            LoadDropdownOptions();
            return View(pet);
        }




        // 編輯寵物
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var pet = _context.Pet
                .Include(p => p.Owner)
                .FirstOrDefault(p => p.PetID == id);

            if (pet == null) return NotFound();

            LoadDropdownOptions();
            return View(pet);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pet pet, IFormFile? imageFile)
        {
            if (id != pet.PetID) return NotFound();

            if (ModelState.IsValid)
            {
                var existingPet = _context.Pet.FirstOrDefault(p => p.PetID == id);
                if (existingPet == null) return NotFound();

                // 更新基本屬性
                existingPet.PetName = pet.PetName;
                existingPet.BirthDate = pet.BirthDate;
                existingPet.Gender = pet.Gender;
                existingPet.Neutering = pet.Neutering;
                existingPet.MicrochipID = pet.MicrochipID;
                existingPet.State = pet.State;
                existingPet.TypeName = pet.TypeName;

                // 圖片上傳/保留原圖片路徑
                if (imageFile != null && imageFile.Length > 0)
                {
                    ProcessImageUpload(imageFile, existingPet);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { ownerId = existingPet.OwnerID });
            }

            LoadDropdownOptions();
            return View(pet);
        }



        // 刪除寵物
        [HttpGet("Delete")]
        public IActionResult Delete(int id)
        {
            var pet = _context.Pet.Find(id);
            if (pet == null)
            {
                return NotFound();
            }
            return View(pet);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int petID, int ownerID)
        {
            var pet = _context.Pet.FirstOrDefault(p => p.PetID == petID);
            if (pet == null) return NotFound();

            // 刪除伺服器上的圖片
            if (!string.IsNullOrEmpty(pet.ImagePath))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, pet.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Pet.Remove(pet);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { ownerId = ownerID });
        }



        //下拉式選單
        private void LoadDropdownOptions()
        {
            ViewBag.TypeNameOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Canine", Text = "Canine (犬)" },
                new SelectListItem { Value = "Feline", Text = "Feline (貓)" },
                new SelectListItem { Value = "Rabbit", Text = "Rabbit (兔)" },
                new SelectListItem { Value = "Rodent", Text = "Rodent (鼠)" },
                new SelectListItem { Value = "Else", Text = "Else (其他)" }
            };

            ViewBag.GenderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Female", Text = "Female (雌性)" },
                new SelectListItem { Value = "Male", Text = "Male (雄性)" },
                new SelectListItem { Value = "Non-Binary", Text = "Non-Binary (不確定性別)" }
            };

            ViewBag.NeuteringOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Intact", Text = "Intact (尚未結紮)" },
                new SelectListItem { Value = "Unknown", Text = "Unknown (不確定)" },
                new SelectListItem { Value = "Neuted", Text = "Neuted (已結紮)" }
            };

            ViewBag.StateOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "遺失", Text = "遺失" },
                new SelectListItem { Value = "送養", Text = "送養" },
                new SelectListItem { Value = "死亡", Text = "死亡" },
                new SelectListItem { Value = "正常", Text = "正常" }
            };
        }

        //上傳圖片
        private void ProcessImageUpload(IFormFile? imageFile, Pet pet)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // 路徑
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 個別文件名(防覆蓋)
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                // 保存圖片到伺服器
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
                //圖片路徑到模型
                pet.ImagePath = "/Uploads/" + fileName;
            }
            else
            {
                // 沒有上傳圖片，使用預設
                pet.ImagePath = "/Uploads/pet-placeholder.jpg";
            }
        }

    }
}
