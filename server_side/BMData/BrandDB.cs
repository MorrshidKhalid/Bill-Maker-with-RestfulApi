using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Brands;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;


namespace BMData
{

    public class BrandDTO
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

        public BrandDTO(int brandID, string brandName)
        {
            BrandID = brandID;
            BrandName = brandName;
        }
    }

    public class BrandDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetBrand(SqlDataReader reader, ref BrandDTO? brandDTO)
        {
            brandDTO = new BrandDTO
                        (
                        (int)reader[BRANDS_COLUMN_PK],
                        (string)reader[BRANDS_COLUMN_NAME]
                        );
        }



        public static List<BrandDTO>? GetAllBrands()
        {
            var brandDTOs = new List<BrandDTO>();

            string query = $"SELECT * FROM {BRANDS}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    brandDTOs.Add(new BrandDTO((int)reader[BRANDS_COLUMN_PK], (string)reader[BRANDS_COLUMN_NAME]));
            }
            catch
            {
                brandDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return brandDTOs;
        }

        public static int AddNewBrand(BrandDTO brandDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {BRANDS} ({BRANDS_COLUMN_NAME}) VALUES (@brand); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@brand", brandDTO.BrandName);

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

        public static BrandDTO? GetBrandByID(int brandID)
        {
            BrandDTO? brandDTO = null;

            string query = $"SELECT * FROM {BRANDS} WHERE {BRANDS_COLUMN_PK} = @id";
            
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", brandID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetBrand(reader, ref brandDTO);
            } 
            catch
            {
                brandDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return brandDTO;
        }

        public static BrandDTO? GetBrandByName(string brandName)
        {
            BrandDTO? brandDTO = null;
            string query = $"SELECT * FROM {BRANDS} WHERE {BRANDS_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", brandName);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetBrand(reader, ref brandDTO);
            }
            catch
            {
                brandDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return brandDTO;
        }

        public static bool UpdateBrand(BrandDTO brandDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {BRANDS} 
                            SET 
                            {BRANDS_COLUMN_NAME} = @Name
                            WHERE {BRANDS_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", brandDTO.BrandID);
            command.Parameters.AddWithValue("@Name", brandDTO.BrandName);

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

        public static bool DeleteBrand(int brandID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {BRANDS} WHERE {BRANDS_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", brandID);

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

        public static bool IsExists(string brandName)
        {
            bool isFound = false;
            string query = $"SELECT 1 {BRANDS_COLUMN_NAME} FROM {BRANDS} WHERE {BRANDS_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", brandName);

            try
            {
                connection.Open();
                DBLib.HasRow(ref isFound, command);
            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}