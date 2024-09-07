using Microsoft.Data.SqlClient;
using static DBAccessSettings;
using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Currencies;

namespace BMData
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }

        public CurrencyDTO(int currencyID, string country, string code, string name, decimal rate)
        {
            CurrencyID = currencyID;
            Country = country;
            Code = code;
            Name = name;
            Rate = rate;
        }
    }

    public class CurrencyDB
    {
        readonly static SqlConnection connection = new(ConnectionString);

        private static void GetCurrency(SqlDataReader reader, ref CurrencyDTO? currencyDTO)
        {
            currencyDTO = new CurrencyDTO
                        (
                        (int)reader[CURRENCIES_COLUMN_PK],
                        (string)reader[CURRENCIES_COLUMN_COUNTRY],
                        (string)reader[CURRENCIES_COLUMN_CODE],
                        (string)reader[CURRENCIES_COLUMN_NAME],
                        (decimal)reader[CURRENCIES_COLUMN_RATE]
                        );
        }

        private static void AddToList(SqlDataReader reader, ref List<CurrencyDTO> list)
        {
            while (reader.Read())
            {
                list.Add(new CurrencyDTO
                    (
                    (int)reader[CURRENCIES_COLUMN_PK],
                    (string)reader[CURRENCIES_COLUMN_COUNTRY],
                    (string)reader[CURRENCIES_COLUMN_CODE],
                    (string)reader[CURRENCIES_COLUMN_NAME],
                    (decimal)reader[CURRENCIES_COLUMN_RATE]
                    ));
            }
        }



        public static List<CurrencyDTO> GetAllCurrencies()
        {
            var currinciesList = new List<CurrencyDTO>();

            string query = $"SELECT * FROM {CURRENCIES}";

            SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                AddToList(reader, ref currinciesList);
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return currinciesList;
        }

        public static CurrencyDTO? GetCurrencyByID(int id)
        {
            CurrencyDTO? currencyDTO = null;

            string query = $"SELECT * FROM {CURRENCIES} WHERE {CURRENCIES_COLUMN_PK} = @id";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    GetCurrency(reader, ref currencyDTO);
            }
            catch
            {
                currencyDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return currencyDTO;
        }
        
        public static List<CurrencyDTO>? GetCurrencyByCode(string code)
        {
            var currinciesList = new List<CurrencyDTO>();

            string query = $"SELECT * FROM {CURRENCIES} WHERE {CURRENCIES_COLUMN_CODE} = @code";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@code", code);

            try
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                AddToList(reader, ref currinciesList);
            }
            catch
            {
                currinciesList = null;
            }
            finally
            {
                connection.Close();
            }

            return currinciesList;
        }
      
        public static bool UpdateRate(CurrencyDTO currencyDTO)
        {
            int rowEffected = -1;
            string query = $@"UPDATE {CURRENCIES} 
                            SET 
                            {CURRENCIES_COLUMN_RATE} = @rate
                            WHERE {CURRENCIES_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", currencyDTO.CurrencyID);
            command.Parameters.AddWithValue("@rate", currencyDTO.Rate);

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