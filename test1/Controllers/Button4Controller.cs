using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json; // 引入內建的 JSON 套件
using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using test1.Models;


namespace WebApplication2.Controllers
{
    public class Button4Controller : Controller
    {
        public IActionResult Index()
        {
            string json = "[\n" +
                "{\"產品名稱\": \"熟齡膳 - 貓保健罐頭\", \"價格\": \"建議售價:1入$80 6入$450 $24入$1680\", \"產品介紹\": \"主要原料: 雞胸肉、鮪魚、雞油、魚油、水\\n營養添加: 絲蘭、維他命、魚水解蛋白、礦物質、碳酸鈣、蛋黃粉、洋車前子、蔬菜粉、蒟蒻粉、離胺酸\\n功效: 腦神經保健、心臟保健、腎臟保健、泌尿道保健、關節保健、免疫力保健、熟齡貓照護、貓熟齡全方位營養補充\\n使用方法: 直接食用，貓每 5-10 公斤體重一包，早晚各一次，或依獸醫師指示使用\", \"圖片連結\": \"/img/product_1558397.jpg\"},\n" +
                "{\"產品名稱\": \"腎泌膳 - 貓保健罐頭\", \"價格\": \"建議售價:1入$80 6入$450 24入$1680\", \"產品介紹\": \"主要原料: 雞胸肉、鮭魚、雞油、魚油、水\\n營養添加: 絲蘭、維他命、魚水解蛋白、礦物質、碳酸鈣、蛋黃粉、洋車前子、蔬菜粉、蒟蒻粉、離胺酸\\n功效: 貓腎臟保健、膀胱保健、結石保健、泌尿道保健、貓腎臟泌尿道營養補充\\n使用方法: 直接食用，貓每 5-10 公斤體重一包，早晚各一次，或依獸醫師指示使用\", \"圖片連結\": \"/img/product_1558393.jpg\"},\n" +
                "{\"產品名稱\": \"關膝膳 - 保健肉泥\", \"價格\": \"建議售價:1入$45 6入$240 50入$1750\", \"產品介紹\": \"主要原料: 雞胸肉、雞肝、雞油\\n營養添加: 魚水解蛋白、牛磺酸、蛋黃粉、洋車前子、酵母粉\\n使用方法: 直接食用，狗貓每 10 公斤體重一包，早晚各一次，或依獸醫師指示使用\\n規格: 每份18g/每包含一份\", \"圖片連結\": \"/img/product_1557261.jpg\"},\n" +
                "{\"產品名稱\": \"癌疫膳 - 保健肉泥\", \"價格\": \"建議售價:1入$45 6入$240 50入$1750\", \"產品介紹\": \"主要原料: 雞胸肉、雞肝、雞油\\n營養添加: 魚水解蛋白、牛磺酸、蛋黃粉、洋車前子、酵母粉、B-glucane、DHA、EPA\\n使用方法: 直接食用，狗貓每 10 公斤體重一包，早晚各一次，或依獸醫師指示使用\\n規格: 每條18g\", \"圖片連結\": \"/img/product_1557260.jpg\"},\n" +
                "{\"產品名稱\": \"腎泌膳 - 保健肉泥\", \"價格\": \"建議售價:1入$45 6入$240 50入$1750\", \"產品介紹\": \"主要原料: 雞胸肉、雞肝、雞油\\n營養添加: 魚水解蛋白、牛磺酸、蛋黃粉、洋車前子、酵母粉\\n使用方法: 直接食用，狗貓每 10 公斤體重一包，早晚各一次，或依獸醫師指示使用\\n規格: 每份18g/每包含一份\", \"圖片連結\": \"/img/product_1557255.jpg\"}\n" +
            "]";

            // 反序列化 JSON 字串為模型列表
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<List<WebCrawler>>(json, options);

            return View(data);
        }
        

        public IActionResult pythonInvoker()
        {
            var pythonInvoker = new PythonInvoker();
            string result = pythonInvoker.CallPythonScript("web_crawler.py");
            Console.WriteLine(result);
            try
            {
                // 使用內建的 System.Text.Json 反序列化
                var data = JsonSerializer.Deserialize<List<WebCrawler>>(result);
                Console.WriteLine(data);
                // 返回序列化的 JSON 給前端
                return PartialView("~/Views/Button4/PartialViewIndex.cshtml", data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Controller錯誤訊息 JSON Deserialization Error: " + ex.Message);
                return Content("Failed to deserialize JSON.");
            }
        }
    }
}
