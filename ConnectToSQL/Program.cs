using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectToSQL
{
    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["ConnectToSQL"].ConnectionString;
        static string querySelect = "SELECT [Id],[Name],[Article],[ProducerId],[Description],[ProductGroupId] FROM [dbo].[tblProduct]";
        static string queryInsert = "INSERT INTO [dbo].[tblProduct] ([Name],[ProductGroupId]) VALUES (@Name,@ProductGroupId); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        static string queryUpdate = "UPDATE [dbo].[tblProduct] SET [Name] = @Name WHERE [Id] = @id";
        static string queryDelete = "DELETE FROM [dbo].[tblProduct] WHERE [Id] = @id";

        static void Main(string[] args)
        {
            List<Connect> ListProduct = GetAll(connectionString, querySelect);
            // відображення даних таблиці
            Console.WriteLine("Before change:");
            ShowList(ListProduct);
            // ввід даних для додавання запису
            Console.WriteLine("---New Record---");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("ProductGroupId: ");
            int productGroupId = Int32.Parse(Console.ReadLine());
            Connect con = new Connect() { Name = name, ProductGroupId = productGroupId};
            // додати запис
            AddRecord(con);
            
            // ввід даних для редагування запису
            Console.WriteLine("---Edit Record---");
            Console.Write("Id: ");
            int id = Int32.Parse(Console.ReadLine());
            Console.Write("Name: ");
            name = Console.ReadLine();
            // редагувати запис
            UpdateRecord(new Connect() { Id = id, Name = name});
            
            // ввід даних для видалення запису
            Console.WriteLine("---Delete Record---");
            Console.Write("Id: ");
            id = Int32.Parse(Console.ReadLine());
            // видалити запис
            DeleteRecord(id);

            List<Connect> ListProductNew = GetAll(connectionString, querySelect);
            // відображення даних таблиці після проведених змін
            Console.Write("After change:");
            ShowList(ListProductNew);

            Console.Read();
        }

        public static List<Connect> GetAll(string ConnectionString, string Query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Connect> myListProduct = new List<Connect>();
                        while (reader.Read())
                        {
                            myListProduct.Add(new Connect()
                            {
                                Id = (int)reader["id"],
                                Name = (string)reader["Name"],
                                //Article = (string)reader["Article"], 
                                //ProducerId = (int?)reader["ProducerId"], 
                                //Description = (string)reader["Description"],
                                ProductGroupId = (int)reader["ProductGroupId"] 
                            });
                        }
                        return myListProduct;
                    }
                }
            }
        }

        public static Connect AddRecord(Connect myListProduct)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryInsert, connection))
                {
                    //command.Parameters.AddWithValue("Id", myListProduct.Id);
                    command.Parameters.AddWithValue("Name", myListProduct.Name);
                    command.Parameters.AddWithValue("ProductGroupId", myListProduct.ProductGroupId);

                    int connectId = (int)command.ExecuteScalar();
                    myListProduct.Id = connectId;

                    return myListProduct;
                }
            }
        }

        public static void UpdateRecord(Connect myListProduct)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryUpdate, connection))
                {
                    command.Parameters.AddWithValue("Id", myListProduct.Id);
                    command.Parameters.AddWithValue("Name", myListProduct.Name);

                    command.ExecuteNonQuery();

                }
            }
        }

        public static void DeleteRecord(int prodId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDelete, connection))
                {
                    command.Parameters.AddWithValue("id", prodId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void ShowList(List<Connect> listProduct)
        {
            for (int i = 0; i < listProduct.Count; i++)
            {
                Console.WriteLine(String.Format("{0} {1} {2}", listProduct[i].Id, listProduct[i].Name, listProduct[i].ProductGroupId));
                //Console.WriteLine(String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", listProduct[i].Id, listProduct[i].Name, listProduct[i].Article, listProduct[i].ProducerId, listProduct[i].Description, listProduct[i].ProductGroupId));
            }
        }
    }
}
        

    

     

