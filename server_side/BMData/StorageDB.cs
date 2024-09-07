using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Storages;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;

namespace BMData
{
    public class StorageDTO
    {
        public int StorageID { get; set; }
        public string Capacity { get; set; }

        public StorageDTO(int storageID, string capacity)
        {
            StorageID = storageID;
            Capacity = capacity;
        }
    }

    public class StorageDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetStorege(SqlDataReader reader, ref StorageDTO? storageDTO)
        {
            storageDTO = new StorageDTO
                        (
                        (int)reader[STORAGE_COLUMN_PK],
                        (string)reader[STORAGE_COLUMN_CAPACITY]
                        );
        }



        public static List<StorageDTO>? GetAllStorages()
        {
            var storagDTOs = new List<StorageDTO>();

            string query = $"SELECT * FROM {STORAGES}";
            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    storagDTOs.Add(new StorageDTO((int)reader[STORAGE_COLUMN_PK], (string)reader[STORAGE_COLUMN_CAPACITY]));
            }
            catch
            {
                storagDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return storagDTOs;
        }


        public static int AddNewStorage(StorageDTO storageDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {STORAGES} ({STORAGE_COLUMN_CAPACITY}) VALUES (@capcity); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@capcity", storageDTO.Capacity);

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

        public static StorageDTO? GetStorageByID(int storageID)
        {
            StorageDTO? storageDTO = null;

            string query = $"SELECT * FROM {STORAGES} WHERE {STORAGE_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", storageID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetStorege(reader, ref storageDTO);
            }
            catch
            {
                storageDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return storageDTO;
        }

        public static StorageDTO? GetStorageByCapacity(string capacity)
        {
            StorageDTO? storageDTO = null;
            string query = $"SELECT * FROM {STORAGES} WHERE {STORAGE_COLUMN_CAPACITY} = @capacity";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@capacity", capacity);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetStorege(reader, ref storageDTO);
            }
            catch
            {
                storageDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return storageDTO;
        }

        public static bool UpdateStorage(StorageDTO storageDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {STORAGES} 
                            SET 
                            {STORAGE_COLUMN_CAPACITY} = @Capacity
                            WHERE {STORAGE_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", storageDTO.StorageID);
            command.Parameters.AddWithValue("@Capacity", storageDTO.Capacity);

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

        public static bool DeleteStorage(int storageID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {STORAGES} WHERE {STORAGE_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", storageID);

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

        public static bool IsExists(string capacity)
        {
            bool isFound = false;
            string query = $"SELECT 1 {STORAGE_COLUMN_CAPACITY} FROM {STORAGES} WHERE {STORAGE_COLUMN_CAPACITY} = @capacity";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@capacity", capacity);

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
