using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Data.Common;
using System.Data.SqlClient;
using test1.Models;   

namespace test1.Controllers
{
    public class Button3Controller : Controller
    {
        public IActionResult Index()    
        {
            var data = new ProductManager();
            return View(data.dataList);
        }

        public JsonResult InsertData()
        {
            try
            {
                string productId = Request.Form["productId"].ToString();
                string productName = Request.Form["productName"].ToString();
                int productQuantity = int.Parse(Request.Form["productQuantity"]);

                // 插入資料庫
                var data = new ProductManager();
                data.InsertProduct(productId, productName, productQuantity);
                return Json(new { 
                    success = true, 
                    message = "資料插入成功",                     
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult DeleteData()
        {
            try
            {
                string productId = Request.Form["productId"].ToString();               
                var data = new ProductManager();
                data.DeleteProduct(productId);
                return Json(new
                {
                    success = true,
                    message = "資料刪除成功",                    
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult editData()
        {
            try
            {                
                string newProductId = Request.Form["newProductId"].ToString();
                string newProductName = Request.Form["newProductName"].ToString();
                int newProductQuantity = int.Parse(Request.Form["newProductQuantity"]);
                string oldProductId = Request.Form["oldProductId"].ToString();
                var data = new ProductManager();
                data.editProduct(newProductId, newProductName, newProductQuantity, oldProductId);
                return Json(new
                {
                    success = true,
                    message = "資料更新成功",
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult LoadProductTable()
        {
            var data = new ProductManager();
            return PartialView("~/Views/Button3/PartialViewIndex.cshtml", data.dataList);                                                
        }
        public IActionResult searchData()
        {
            var data = new ProductManager();
            data.searchData(Request.Form["searchWords"].ToString());
            return PartialView("~/Views/Button3/PartialViewIndex.cshtml", data.dataList);
        }
    }

}
