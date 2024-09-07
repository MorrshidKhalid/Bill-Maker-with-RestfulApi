using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Terms;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;

namespace BMData
{

    public class TermDTO
    {
        public int TermID { get; set; }
        public string Description { get; set; }

        public TermDTO(int termID, string description)
        {
            TermID = termID;
            Description = description;
        }
    }


    public class TermDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetTerm(SqlDataReader reader, ref TermDTO? termDTO)
        {
            termDTO = new TermDTO
                        (
                        (int)reader[TERM_COLUMN_PK],
                        (string)reader[TERM_COLUMN_DESCRIPTION]
                        );
        }



        public static List<TermDTO>? GetAllTerms()
        {
            var termDTOs = new List<TermDTO>();

            string query = $"SELECT * FROM {TERMS}";
            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    termDTOs.Add(new TermDTO((int)reader[TERM_COLUMN_PK], (string)reader[TERM_COLUMN_DESCRIPTION]));
            }
            catch
            {
                termDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return termDTOs;
        }


        public static int AddNewTerm(TermDTO termDTO)
        {
            int insertedID = -1;
            string query = $"INSERT INTO {TERMS} ({TERM_COLUMN_DESCRIPTION}) VALUES (@des); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@des", termDTO.Description);

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

        public static TermDTO? GetTermByID(int termID)
        {
            TermDTO? termDTO = null;

            string query = $"SELECT * FROM {TERMS} WHERE {TERM_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", termID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    GetTerm(reader, ref termDTO);
            }
            catch
            {
                termDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return termDTO;
        }


        public static bool UpdateTerm(TermDTO termDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {TERMS} 
                            SET 
                            {TERM_COLUMN_DESCRIPTION} = @des
                            WHERE {TERM_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", termDTO.TermID);
            command.Parameters.AddWithValue("@des", termDTO.Description);

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

        public static bool DeleteTerm(int termID)
        {
            int rowEffected = -1;
            string query = $"DELETE FROM {TERMS} WHERE {TERM_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", termID);

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
