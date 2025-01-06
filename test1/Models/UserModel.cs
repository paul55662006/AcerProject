using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text;

namespace test1.Models
{
    public class Product    // 藥品資訊的格式
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }
    public class WebCrawler
    {
        //中文鍵映射到C#屬性
        [JsonPropertyName("產品名稱")]
        public string ProductName { get; set; }
        [JsonPropertyName("價格")]
        public string ProductPrice { get; set; }
        [JsonPropertyName("產品介紹")]
        public string ProductIntroduction { get; set; }
        [JsonPropertyName("圖片連結")]
        public string ProductPictureUrl { get; set; }
    }

    public class ProductManager
    {
        public List<Product> dataList = new List<Product>();
        private readonly string connectString = "Server=tcp:frankdbserver.database.windows.net,1433;Initial Catalog=PetClinicDb;Persist Security Info=False;User ID=frankfan;Password=Ab16881688?;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // 首頁訪問時初始化讀取product表格全部資料
        public ProductManager()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Product", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    dataList.Add(new Product
                    {
                        ProductId = reader.GetString(reader.GetOrdinal("ProductId")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        ProductQuantity = reader.GetInt32(reader.GetOrdinal("ProductQuantity"))
                    });
                }
            }
        }

        // 插入資料
        public void InsertProduct(string productId, string productName, int productQuantity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                var query = "INSERT INTO Product (ProductId, ProductName, ProductQuantity) VALUES (@ProductId, @ProductName, @ProductQuantity)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductId", productId);
                sqlCommand.Parameters.AddWithValue("@ProductName", productName);
                sqlCommand.Parameters.AddWithValue("@ProductQuantity", productQuantity);
                sqlCommand.ExecuteNonQuery();
            }
        }

        // 刪除資料
        public void DeleteProduct(string productId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                var query = "DELETE FROM Product WHERE ProductId = @ProductId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductId", productId);
                sqlCommand.ExecuteNonQuery();
            }
        }

        // 編輯資料
        public void editProduct(string newProductId, string newProductName, int newProductQuantity, string oldProductId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                var query = "UPDATE Product SET ProductId=@newProductId, ProductName=@newProductName, ProductQuantity=@newProductQuantity WHERE ProductId = @oldProductId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@newProductId", newProductId);
                sqlCommand.Parameters.AddWithValue("@newProductName", newProductName);
                sqlCommand.Parameters.AddWithValue("@newProductQuantity", newProductQuantity);
                sqlCommand.Parameters.AddWithValue("@oldProductId", oldProductId);
                sqlCommand.ExecuteNonQuery();
            }
        }

        // 搜尋資料
        public void searchData(string searchWords)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Product WHERE ProductName LIKE '%' + @searchWords + '%'", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@searchWords", searchWords); // 加入參數
                SqlDataReader reader = sqlCommand.ExecuteReader();
                dataList = new List<Product>();
                while (reader.Read())
                {
                    dataList.Add(new Product
                    {
                        ProductId = reader.GetString(reader.GetOrdinal("ProductId")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        ProductQuantity = reader.GetInt32(reader.GetOrdinal("ProductQuantity"))
                    });
                }
            }
        }
    }

    // 呼叫python程式碼
    public class PythonInvoker
    {
        public string CallPythonScript(string scriptName, string args = "")
        {
            // 動態拼接腳本路徑
            string rootPath = Directory.GetCurrentDirectory(); // 專案根目錄
            string scriptPath = Path.Combine(rootPath, "wwwroot", "python", scriptName);

            // 設定 Python 執行參數
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python", // Azure 環境中直接使用 "python"
                Arguments = $"{scriptPath} {args}",

                // Azure必須寫此絕對路徑
                //Arguments = $"/home/site/wwwroot/wwwroot/python/web_crawler.py",
                RedirectStandardOutput = true,
                RedirectStandardError = true, // 捕獲錯誤輸出
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            // 執行 Python 腳本
            using (Process process = Process.Start(psi))
            {
                using (StreamReader outputReader = process.StandardOutput)
                using (StreamReader errorReader = process.StandardError)
                {
                    string result = outputReader.ReadToEnd();
                    string error = errorReader.ReadToEnd(); // 捕獲錯誤輸出
                    if (process.ExitCode != 0)
                    {
                        // 如果 Python 腳本有錯誤，打印錯誤信息
                        throw new Exception($"Python script failed with error: {error}");
                    }
                    return result; // 正常返回結果
                }
            }
        }
    }
}

