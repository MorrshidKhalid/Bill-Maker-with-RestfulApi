using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Business;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;

namespace BMData
{
    public class BusinessDTO
    {
        public int BusinessID { get; set; }
        public string Name { get; set; }

        public BusinessDTO(int businessID, string name)
        {
            BusinessID = businessID;
            Name = name;
        }
    }

    public class BusinessDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetBusiness(SqlDataReader reader, ref BusinessDTO? businessDTO)
        {
            businessDTO = new BusinessDTO
                        (
                        (int)reader[BUSINESS_COLUMN_PK],
                        (string)reader[BUSINESS_COLUMN_NAME]
                        );
        }



        public static List<BusinessDTO>? GetAllBusinesses()
        {
            var businessDTOs = new List<BusinessDTO>();

            string query = $"SELECT * FROM {BUSINESSES}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    businessDTOs.Add(new BusinessDTO((int)reader[BUSINESS_COLUMN_PK], (string)reader[BUSINESS_COLUMN_NAME]));
            }
            catch
            {
                businessDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return businessDTOs;
        }

        public static int AddNewBusiness(BusinessDTO businessDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {BUSINESSES} ({BUSINESS_COLUMN_NAME}) VALUES (@business); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@business", businessDTO.Name);

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

        public static BusinessDTO? GetBusinessByID(int businessID)
        {
            BusinessDTO? businessDTO = null;

            string query = $"SELECT * FROM {BUSINESSES} WHERE {BUSINESS_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", businessID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetBusiness(reader, ref businessDTO);
            }
            catch
            {
                businessDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return businessDTO;
        }

        public static BusinessDTO? GetBusinessByName(string businessName)
        {
            BusinessDTO? businessDTO = null;
            string query = $"SELECT * FROM {BUSINESSES} WHERE {BUSINESS_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", businessName);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetBusiness(reader, ref businessDTO);
            }
            catch
            {
                businessDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return businessDTO;
        }

        public static bool UpdateBusiness(BusinessDTO businessDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {BUSINESSES} 
                            SET 
                            {BUSINESS_COLUMN_NAME} = @Name
                            WHERE {BUSINESS_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", businessDTO.BusinessID);
            command.Parameters.AddWithValue("@Name", businessDTO.Name);

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

        public static bool DeleteBusiness(int businessID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {BUSINESSES} WHERE {BUSINESS_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", businessID);

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

        public static bool IsExists(string businessName)
        {
            bool isFound = false;
            string query = $"SELECT 1 {BUSINESS_COLUMN_NAME} FROM {BUSINESSES} WHERE {BUSINESS_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", businessName);

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
