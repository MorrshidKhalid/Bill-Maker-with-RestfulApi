using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Method;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;

namespace BMData
{
    public class MethodDTO
    {
        public int MethodID { get; set; }
        public string MethodName { get; set; }

        public MethodDTO(int methodID, string methodName)
        {
            MethodID = methodID;
            MethodName = methodName;
        }
    }

    public class MethodDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetMethod(SqlDataReader reader, ref MethodDTO? methodDTO)
        {
            methodDTO = new MethodDTO
                        (
                        (int)reader[PAYMENT_METHOD_COLUMN_PK],
                        (string)reader[PAYMENT_METHOD_COLUMN_METHOD]
                        );
        }

        public static List<MethodDTO>? GetMethods()
        {
            var methodDTOs = new List<MethodDTO>();

            string query = $"SELECT * FROM {PAYMENT_METHODS}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    methodDTOs.Add(new MethodDTO((int)reader[PAYMENT_METHOD_COLUMN_PK], (string)reader[PAYMENT_METHOD_COLUMN_METHOD]));
            }
            catch
            {
                methodDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return methodDTOs;
        }

        public static int AddNewMethod(MethodDTO methodDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {PAYMENT_METHODS} ({PAYMENT_METHOD_COLUMN_METHOD}) VALUES (@method); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@method", methodDTO.MethodName);

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

        public static MethodDTO? GetMethodByID(int methodID)
        {
            MethodDTO? methodDTO = null;

            string query = $"SELECT * FROM {PAYMENT_METHODS} WHERE {PAYMENT_METHOD_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", methodID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetMethod(reader, ref methodDTO);
            }
            catch
            {
                methodDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return methodDTO;
        }

        public static bool UpdateMethod(MethodDTO methodDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {PAYMENT_METHODS} 
                            SET 
                            {PAYMENT_METHOD_COLUMN_METHOD} = @Name
                            WHERE {PAYMENT_METHOD_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", methodDTO.MethodID);
            command.Parameters.AddWithValue("@Name", methodDTO.MethodName);

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

        public static bool DeleteMethod(int methodID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {PAYMENT_METHODS} WHERE {PAYMENT_METHOD_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", methodID);

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

        public static bool IsExists(string method)
        {
            bool isFound = false;
            string query = $"SELECT 1 {PAYMENT_METHOD_COLUMN_METHOD} FROM {PAYMENT_METHODS} WHERE {PAYMENT_METHOD_COLUMN_METHOD} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", method);

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
