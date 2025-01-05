using System;
using System.Data.SqlClient;

namespace test1.Models
{
    public class Product    // 藥品資訊的格式
    {
        public string ProductId;
        public string ProductName;
        public int ProductQuantity;
    }

    public class ProductManager
    {   
        public List<Product> dataList = new List<Product>();
        //private readonly string connectString = "Data Source=(localdb)\\MSSQLLocalDB;Database=Product;User ID=frankfan;Password=123456;Trusted_Connection=True";
        private readonly string connectString = "Server=tcp:frankdbserver.database.windows.net,1433;Initial Catalog=PetClinicDb;Persist Security Info=False;User ID=frankfan;Password=Ab16881688?;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public ProductManager()     
        {            
            using (SqlConnection sqlConnection = new SqlConnection(connectString))      
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Product", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read()) {     
                    dataList.Add(new Product
                    { 
                        ProductId = reader.GetString(reader.GetOrdinal("ProductId")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        ProductQuantity = reader.GetInt32(reader.GetOrdinal("ProductQuantity"))
                    });
                }
            }            
        }
        public void InsertProduct(string productId, string productName, int productQuantity) {
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
        public void DeleteProduct(string productId) {
            using (SqlConnection sqlConnection = new SqlConnection(connectString)) {
                sqlConnection.Open();
                var query = "DELETE FROM Product WHERE ProductId = @ProductId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductId", productId);
                sqlCommand.ExecuteNonQuery();
            }
        }

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
                // 測試是否正確搜尋
                //for (int i =0;i<dataList.Count; i++)
                //{
                //    Console.WriteLine(dataList[i].ProductId);
                //    Console.WriteLine(dataList[i].ProductName);
                //    Console.WriteLine(dataList[i].ProductQuantity);
                //}        
            }
        }
    }
}
