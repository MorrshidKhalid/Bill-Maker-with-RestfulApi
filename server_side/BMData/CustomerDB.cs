using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Person;
using static BMDataContract.BMContract.Customer;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;


namespace BMData
{
    public class CustomerDTO : PersonDTO
    {
        public int CustomerID { get; set; }

        public CustomerDTO(int customerID, int personID, string firstName, string secondName, string lastName, string email, bool gender, string address, string phone)
            : base(personID, firstName, secondName, lastName, email, gender, address, phone)
        {
            CustomerID = customerID;
        }
    }


    public class CustomerDB
    {

        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);
        
        public static List<CustomerDTO> GetAllCustomers()
        {
            List<CustomerDTO> customerDTOs = new();
            string query = $@"SELECT 
                              {CUSTOMERS}.*,
                              {BMContract.JOIN_CUSTOMER_PERSON_INFO}";

            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    customerDTOs.Add(new CustomerDTO
                        (
                        (int)reader[CUSTOMER_COLUMN_PK],
                        (int)reader[CUSTOMER_COLUMN_PERSON_ID],
                        (string)reader[PERSON_COLUMN_FIRST_NAME],
                        (string)reader[PERSON_COLUMN_SECOND_NAME],
                        (string)reader[PERSON_COLUMN_LAST_NAME],
                        (string)reader[PERSON_COLUMN_EMAIL],
                        (bool)reader[PERSON_COLUMN_GENDOR],
                        (string)reader[PERSON_COLUMN_ADDRESS],
                        (string)reader[PERSON_COLUMN_PHONE]
                        ));
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return customerDTOs;
        }

        public static CustomerDTO? GetCustomerByID(int customerID)
        {
            CustomerDTO? customerDTO = null;

            string query = $@"SELECT 
                              {CUSTOMERS}.*,
                              {BMContract.JOIN_CUSTOMER_PERSON_INFO}
                              WHERE {CUSTOMER_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", customerID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    customerDTO = new CustomerDTO
                        (
                        (int)reader[CUSTOMER_COLUMN_PK],
                        (int)reader[CUSTOMER_COLUMN_PERSON_ID],
                        (string)reader[PERSON_COLUMN_FIRST_NAME],
                        (string)reader[PERSON_COLUMN_SECOND_NAME],
                        (string)reader[PERSON_COLUMN_LAST_NAME],
                        (string)reader[PERSON_COLUMN_EMAIL],
                        (bool)reader[PERSON_COLUMN_GENDOR],
                        (string)reader[PERSON_COLUMN_ADDRESS],
                        (string)reader[PERSON_COLUMN_PHONE]
                        );
            }
            catch
            {
                customerDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return customerDTO;
        }

        public static int AddNewCustomer(int personID)
        {
            int insertedID = -1;            

            string query = $@"INSERT INTO {CUSTOMERS} ({CUSTOMER_COLUMN_PERSON_ID}) VALUES (@personID) {BMContract.SCOPE_IDENTITY}";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

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

        public static int GetTotalCutomersByGendor(bool gendor)
        {
            int total = 0;

            string query = $@"SELECT COUNT(*) as Total 
                              FROM {CUSTOMERS}
                              JOIN {PEOPLE} ON {CUSTOMERS}.{CUSTOMER_COLUMN_PERSON_ID} = {PEOPLE}.{PERSON_COLUMN_PK}
                              Group by {PERSON_COLUMN_GENDOR} HAVING {PERSON_COLUMN_GENDOR} = @gendor";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@gendor", gendor);

            try
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    total = (int)reader["Total"];
            }
            catch
            {
                total = 0;
            }
            finally
            {
                connection.Close();
            }

            return total;
        }

        public static bool DeleteCustomer(int customerID)
        {
            int rowEffected = -1;

            string query = $"DELETE {CUSTOMERS} WHERE {CUSTOMER_COLUMN_PERSON_ID} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", customerID);

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

            return rowEffected > 0;
        }
    }
}
