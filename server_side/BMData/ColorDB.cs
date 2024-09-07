using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Colors;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;

namespace BMData
{

    public class ColorDTO
    {
        public int ColorID { get; set; }
        public string ColorName { get; set; }

        public ColorDTO(int colorID, string colorName)
        {
            ColorID = colorID;
            ColorName = colorName;
        }
    }

    public class ColorDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetColor(SqlDataReader reader, ref ColorDTO? colorDTO)
        {
            colorDTO = new ColorDTO
                        (
                        (int)reader[COLOR_COLUMN_PK],
                        (string)reader[COLOR_COLUMN_NAME]
                        );
        }

        public static List<ColorDTO>? GetAllColors()
        {
            var colorDTOs = new List<ColorDTO>();

            string query = $"SELECT * FROM {COLORS}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    colorDTOs.Add(new ColorDTO((int)reader[COLOR_COLUMN_PK], (string)reader[COLOR_COLUMN_NAME]));
            }
            catch
            {
                colorDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return colorDTOs;
        }

        public static int AddNewColor(ColorDTO colorDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {COLORS} ({COLOR_COLUMN_NAME}) VALUES (@color); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@color", colorDTO.ColorName);

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

        public static ColorDTO? GetColorByID(int colorID)
        {
            ColorDTO? colorDTO = null;

            string query = $"SELECT * FROM {COLORS} WHERE {COLOR_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", colorID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetColor(reader, ref colorDTO);
            }
            catch
            {
                colorDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return colorDTO;
        }

        public static ColorDTO? GetColorByName(string colorName)
        {
            ColorDTO? colorDTO = null;
            string query = $"SELECT * FROM {COLORS} WHERE {COLOR_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", colorName);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetColor(reader, ref colorDTO);
            }
            catch
            {
                colorDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return colorDTO;
        }

        public static bool UpdateColor(ColorDTO colorDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {COLORS} 
                            SET 
                            {COLOR_COLUMN_NAME} = @Name
                            WHERE {COLOR_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", colorDTO.ColorID);
            command.Parameters.AddWithValue("@Name", colorDTO.ColorName);

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

        public static bool DeleteColor(int colorID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {COLORS} WHERE {COLOR_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", colorID);

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

        public static bool IsExists(string colorName)
        {
            bool isFound = false;
            string query = $"SELECT 1 {COLOR_COLUMN_NAME} FROM {BRANDS} WHERE {COLOR_COLUMN_NAME} = @name";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", colorName);

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