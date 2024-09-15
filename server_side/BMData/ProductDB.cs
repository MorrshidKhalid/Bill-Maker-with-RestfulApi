using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Product;
using static BMDataContract.BMContract.Brands;
using static BMDataContract.BMContract.Colors;
using static BMDataContract.BMContract.Terms;
using static BMDataContract.BMContract.Storages;
using static BMDataContract.BMContract.Currencies;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;

namespace BMData
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public string Model { get; set; }
        public int ColorID { get; set; }
        public int StorageID { get; set; }
        public bool Condition { get; set; }
        public int TermID { get; set; }
        public int CurrencyID { get; set; }
        public decimal PurchaPrice { get; set; }
        public string ImagePath { get; set; }
        public byte Quality { get; set; }

        public ProductDTO
            (
            int productID, int brandID, string model, int colorID,
            int storageID, bool condition, int termID, int currencyID,
            decimal purchaPrice, string imagePath, byte quality
            )
        {
            ProductID = productID;
            BrandID = brandID;
            Model = model;
            ColorID = colorID;
            StorageID = storageID;
            Condition = condition;
            TermID = termID;
            CurrencyID = currencyID;
            PurchaPrice = purchaPrice;
            ImagePath = imagePath;
            Quality = quality;
        }
    }

    public class ProductDetailsDTO
    {
        public int ProductID { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Capacity { get; set; }
        public bool Condition { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal PurchaPrice { get; set; }
        public string ImagePath { get; set; }
        public byte Quality { get; set; }

        public ProductDetailsDTO
            (
            int productID, string brandName, string model, string color, 
            string capacity, bool condition, string description, string code, 
            decimal purchaPrice, string imagePath, byte quality
            )
        {
            ProductID = productID;
            BrandName = brandName;
            Model = model;
            Color = color;
            Capacity = capacity;
            Condition = condition;
            Description = description;
            Code = code;
            PurchaPrice = purchaPrice;
            ImagePath = imagePath;
            Quality = quality;
        }
    }

    public class ProductDB
    {

        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void AddToProductDTOList(ref List<ProductDTO> list, SqlDataReader reader)
        {
            string imagePath = string.Empty;

            while (reader.Read()) 
            {
                DBLib.HandleDBNull(reader, PRODUCT_COLUMN_IMAGE_PATH, ref imagePath);
                list.Add(new ProductDTO
                    (
                    (int)reader[PRODUCT_COLUMN_PK],
                    (int)reader[PRODUCT_COLUMN_BRAND_ID],
                    (string)reader[PRODUCT_COLUMN_MODEL],
                    (int)reader[PRODUCT_COLUMN_COLOR_ID],
                    (int)reader[PRODUCT_COLUMN_STORAGE_ID],
                    (bool)reader[PRODUCT_COLUMN_CONDITION],
                    (int)reader[PRODUCT_COLUMN_TERM_ID],
                    (int)reader[PRODUCT_COLUMN_CURRENCY_ID],
                    (decimal)reader[PRODUCT_COLUMN_PURCHA_PRICE],
                    imagePath,
                    (byte)reader[PRODUCT_COLUMN_QUALITY]
                    ));
            }
        }

        private static void AddToProductDetailsDTOList(ref List<ProductDetailsDTO> list, SqlDataReader reader)
        {
            string imagePath = string.Empty;

            while (reader.Read())
            {
                DBLib.HandleDBNull(reader, PRODUCT_COLUMN_IMAGE_PATH, ref imagePath);
                list.Add(new ProductDetailsDTO
                    (
                    (int)reader[PRODUCT_COLUMN_PK],
                    (string)reader[BRANDS_COLUMN_NAME],
                    (string)reader[PRODUCT_COLUMN_MODEL],
                    (string)reader[COLOR_COLUMN_NAME],
                    (string)reader[STORAGE_COLUMN_CAPACITY],
                    (bool)reader[PRODUCT_COLUMN_CONDITION],
                    (string)reader[TERM_COLUMN_DESCRIPTION],
                    (string)reader[CURRENCIES_COLUMN_CODE],
                    (decimal)reader[PRODUCT_COLUMN_PURCHA_PRICE],
                    imagePath,
                    (byte)reader[PRODUCT_COLUMN_QUALITY]
                    ));
            }
        }


        public static List<ProductDTO>? GetAllProductsNormal()
        {
            var productDTOs = new List<ProductDTO>();

            string query = $"SELECT * FROM {PRODUCTS}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                AddToProductDTOList(ref productDTOs, reader);
            }
            catch
            {
                productDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return productDTOs;
        }

        public static List<ProductDetailsDTO>? GetAllProductsWithDetails()
        {
            var productDetailsDTOs = new List<ProductDetailsDTO>();

            string query = $@"{BMContract.SQL_SELECT_ALL_PRODUCTS_WITH_JOIN}";

            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                AddToProductDetailsDTOList(ref productDetailsDTOs, reader);
            }
            catch
            {
                productDetailsDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return productDetailsDTOs;
        }

        public static List<ProductDetailsDTO>? GetProductsByFilter(bool condition, string currencyCode)
        {
            var productDetailsDTOs = new List<ProductDetailsDTO>();
            

            string query = $@"{BMContract.SQL_SELECT_ALL_PRODUCTS_WITH_JOIN}
                              WHERE {PRODUCT_COLUMN_CONDITION} = @condition
                              AND {CURRENCIES_COLUMN_CODE} = @currencyCode";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@condition", condition);
            command.Parameters.AddWithValue("@currencyCode", currencyCode);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                AddToProductDetailsDTOList(ref productDetailsDTOs, reader);
            }
            catch
            {
                productDetailsDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return productDetailsDTOs;
        }

        public static List<ProductDetailsDTO>? GetProductsBetweenPrices(decimal firstPrice, decimal secondPrice)
        {
            var productDetailDTOs = new List<ProductDetailsDTO>();


            string query = $@"{BMContract.SQL_SELECT_ALL_PRODUCTS_WITH_JOIN}
                              WHERE {PRODUCT_COLUMN_PURCHA_PRICE} BETWEEN @firstPrice AND @secondPrice";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@firstPrice", firstPrice);
            command.Parameters.AddWithValue("@secondPrice", secondPrice);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                AddToProductDetailsDTOList(ref productDetailDTOs, reader);
            }
            catch
            {
                productDetailDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return productDetailDTOs;
        }

        public static List<ProductDetailsDTO>? GetProductsByBrand(string brand)
        {
            var productDetailsDTOs = new List<ProductDetailsDTO>();


            string query = $@"{BMContract.SQL_SELECT_ALL_PRODUCTS_WITH_JOIN}
                              WHERE {BRANDS_COLUMN_NAME} = @brand";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@brand", brand);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                AddToProductDetailsDTOList(ref productDetailsDTOs, reader);
            }
            catch
            {
                productDetailsDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return productDetailsDTOs;
        }

        public static int AddNewProduct(ProductDTO productDTO)
        {
            int insertedID = -1;
            string query = $@"INSERT INTO {PRODUCTS} 
                              (
                              {PRODUCT_COLUMN_BRAND_ID},
                              {PRODUCT_COLUMN_MODEL},
                              {PRODUCT_COLUMN_COLOR_ID},
                              {PRODUCT_COLUMN_STORAGE_ID},
                              {PRODUCT_COLUMN_CONDITION},
                              {PRODUCT_COLUMN_TERM_ID},
                              {PRODUCT_COLUMN_CURRENCY_ID},
                              {PRODUCT_COLUMN_PURCHA_PRICE},
                              {PRODUCT_COLUMN_IMAGE_PATH},
                              {PRODUCT_COLUMN_QUALITY}
                              )
                              VALUES
                              (
                              @brandID, @model, @colorID, @storageID, @condition,
                              @termID, @currencyID, @price, @imagePath, @quality
                              );
                              {BMContract.SCOPE_IDENTITY}";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@brandID", productDTO.BrandID);
            command.Parameters.AddWithValue("@model", productDTO.Model);
            command.Parameters.AddWithValue("@colorID", productDTO.ColorID);
            command.Parameters.AddWithValue("@storageID", productDTO.StorageID);
            command.Parameters.AddWithValue("@condition", productDTO.Condition);
            command.Parameters.AddWithValue("@termID", productDTO.TermID);
            command.Parameters.AddWithValue("@currencyID", productDTO.CurrencyID);
            command.Parameters.AddWithValue("@price", productDTO.PurchaPrice);
            DBLib.HandleInsertDBNull(command, "@imagePath", productDTO.ImagePath);
            command.Parameters.AddWithValue("@quality", productDTO.Quality);


            try
            {
                connection.Open();
                DBLib.ExecAndGetInsertedID(ref insertedID, command);
            }
            catch
            {
                insertedID = -1;
            }
            finally
            {
                connection.Close();
            }

            return insertedID;
        }

        public static ProductDTO? GetProductByID(int productID)
        {
            ProductDTO? productDTO = null;

            string query = $"SELECT * FROM {PRODUCTS} WHERE {PRODUCT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", productID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string imagePath = string.Empty;
                    DBLib.HandleDBNull(reader, PRODUCT_COLUMN_IMAGE_PATH, ref imagePath);
                    productDTO = new ProductDTO
                        (
                        (int)reader[PRODUCT_COLUMN_PK],
                        (int)reader[PRODUCT_COLUMN_BRAND_ID],
                        (string)reader[PRODUCT_COLUMN_MODEL],
                        (int)reader[PRODUCT_COLUMN_COLOR_ID],
                        (int)reader[PRODUCT_COLUMN_STORAGE_ID],
                        (bool)reader[PRODUCT_COLUMN_CONDITION],
                        (int)reader[PRODUCT_COLUMN_TERM_ID],
                        (int)reader[PRODUCT_COLUMN_CURRENCY_ID],
                        (decimal)reader[PRODUCT_COLUMN_PURCHA_PRICE],
                        imagePath,
                        (byte)reader[PRODUCT_COLUMN_QUALITY]
                        );

                }

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return productDTO;
        }

        public static bool UpdateProduct(ProductDTO productDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {PRODUCTS} 
                            SET
                            {PRODUCT_COLUMN_BRAND_ID} = @brandID,
                            {PRODUCT_COLUMN_MODEL} = @model,
                            {PRODUCT_COLUMN_COLOR_ID} = @colorID,
                            {PRODUCT_COLUMN_STORAGE_ID} = @storageID,
                            {PRODUCT_COLUMN_CONDITION} = @condition,
                            {PRODUCT_COLUMN_TERM_ID} = @termID,
                            {PRODUCT_COLUMN_CURRENCY_ID} = @currencyID,
                            {PRODUCT_COLUMN_PURCHA_PRICE} = @price,
                            {PRODUCT_COLUMN_IMAGE_PATH} = @imagePath,
                            {PRODUCT_COLUMN_QUALITY} = @quality
                            WHERE {PRODUCT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@brandID", productDTO.BrandID);
            command.Parameters.AddWithValue("@model", productDTO.Model);
            command.Parameters.AddWithValue("@colorID", productDTO.ColorID);
            command.Parameters.AddWithValue("@storageID", productDTO.StorageID);
            command.Parameters.AddWithValue("@condition", productDTO.Condition);
            command.Parameters.AddWithValue("@termID", productDTO.TermID);
            command.Parameters.AddWithValue("@currencyID", productDTO.CurrencyID);
            command.Parameters.AddWithValue("@price", productDTO.PurchaPrice);
            DBLib.HandleInsertDBNull(command, "@imagePath", productDTO.ImagePath);
            command.Parameters.AddWithValue("@quality", productDTO.Quality);
            command.Parameters.AddWithValue("@id", productDTO.ProductID);


            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowEffected = -1;
            }
            finally
            {
                connection.Close();
            }

            return rowEffected != -1;
        }

        public static bool DeleteProduct(int productID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {PRODUCTS} WHERE {PRODUCT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", productID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowEffected = -1;
            }
            finally
            {
                connection.Close();
            }

            return rowEffected != -1;
        }

    }
}
