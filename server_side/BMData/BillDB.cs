using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Bill;
using static BMDataContract.BMContract.BillBody;
using static BMDataContract.BMContract.Business;
using static BMDataContract.BMContract.Customer;
using static BMDataContract.BMContract.Person;
using static BMDataContract.BMContract.Users;
using static BMDataContract.BMContract.Currencies;
using static BMDataContract.BMContract.Product;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;

namespace BMData
{
    public struct BillBody
    {
        public int BBillID { get; set; }
        public int BillID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public decimal SellingPrice { get; set; }
        public int CurrencyID { get; set; }

        public BillBody(int bBillID, int billID, int productID, int qty, decimal sellingPrice, int currencyID)
        {
            BBillID = bBillID;
            BillID = billID;
            ProductID = productID;
            Qty = qty;
            SellingPrice = sellingPrice;
            CurrencyID = currencyID;
        }
    }
    public class BillDTO
    {
        public int BillID { get; set; }
        public int FromBusinessID { get; set; }
        public int ToCustomerID { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public DateTime DateCreated { get; set; }
        public byte BillStatus { get; set; }
        public decimal CurrencyRate { get; set; }
        public int CurrencyID { get; set; }
        public int CreatedByUserID { get; set; }
        public List<BillBody> BodyList { get; set; } 


        public BillDTO
            (
            int billID, int fromBusinessID, int toCustomerID,
            decimal amount, decimal discount, DateTime dateCreated,
            byte billStatus, decimal currencyRate, int currencyID, int createdByUserID,
            List<BillBody> bodyList
            )
        {
            BillID = billID;
            FromBusinessID = fromBusinessID;
            ToCustomerID = toCustomerID;
            Amount = amount;
            Discount = discount;
            DateCreated = dateCreated;
            BillStatus = billStatus;
            CurrencyRate = currencyRate;
            CurrencyID = currencyID;
            CreatedByUserID = createdByUserID;
            BodyList = bodyList;
        }
    }

    public struct BillBodyDetails
    {
        public string Product { get; set; }
        public int Qty { get; set; }
        public decimal SellingPrice { get; set; }
        public string Currency { get; set; }

        public BillBodyDetails(string product, int qty, decimal sellingPrice, string currency)
        {
            Product = product;
            Qty = qty;
            SellingPrice = sellingPrice;
            Currency = currency;
        }
    }
    public class BillDetailsDTO
    {
        public int BillID { get; set; }
        public string Business{ get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public DateTime DateCreated { get; set; }
        public byte BillStatus { get; set; }
        public decimal CurrencyRate { get; set; }
        public string Currency { get; set; }
        public string User { get; set; }
        public List<BillBodyDetails>? BodyList { get; set; }

        public BillDetailsDTO(int billID, string business, string customer, decimal amount, decimal discount, DateTime dateCreated, byte billStatus, decimal currencyRate, string currency, string user, List<BillBodyDetails>? bodyList)
        {
            BillID = billID;
            Business = business;
            Customer = customer;
            Amount = amount;
            Discount = discount;
            DateCreated = dateCreated;
            BillStatus = billStatus;
            CurrencyRate = currencyRate;
            Currency = currency;
            User = user;
            BodyList = bodyList;
        }
    }


    public class BillDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        public static int AddNewBill(BillDTO billDTO)
        {
            // Check if bill body is empty.
            if (billDTO.BodyList.Count == 0)
                return -1;

            int insertedID = -1;

            string query = $@"INSERT INTO {BILLS} 
                              (
                              {BILL_COLUMN_BUSINESS_ID},
                              {BILL_COLUMN_CUSTOMER_ID},
                              {BILL_COLUMN_AMOUNT},
                              {BILL_COLUMN_DISCOUNT},
                              {BILL_COLUMN_DATE},
                              {BILL_COLUMN_STATUS},
                              {BILL_COLUMN_CURRENCY_RATE},
                              {BILL_COLUMN_CURRENCY_ID},
                              {BILL_COLUMN_USER_ID}
                              )
                              VALUES 
                              (@FromBusinessID, @ToCustomerID, 
                              @Amount, @Discount, @Date, @BillStatus, @CurrencyRate, 
                              @CurrencyID, @CreatedByUserID); 
                              {BMContract.SCOPE_IDENTITY}";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@FromBusinessID", billDTO.FromBusinessID);
            command.Parameters.AddWithValue("@ToCustomerID", billDTO.ToCustomerID);
            command.Parameters.AddWithValue("@Amount", billDTO.Amount);
            command.Parameters.AddWithValue("@Discount", billDTO.Discount);
            command.Parameters.AddWithValue("@Date", billDTO.DateCreated);
            command.Parameters.AddWithValue("@BillStatus", billDTO.BillStatus);
            command.Parameters.AddWithValue("@CurrencyRate", billDTO.CurrencyRate);
            command.Parameters.AddWithValue("@CurrencyID", billDTO.CurrencyID);
            command.Parameters.AddWithValue("@CreatedByUserID", billDTO.CreatedByUserID);

            try
            {
                connection.Open();
                DBLib.ExecAndGetInsertedID(ref insertedID, command);
                // If Error Occur while adding bill body delete the bill.
                if (!AddBillBody(billDTO.BodyList, insertedID))
                    Delete(insertedID);
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
        private static bool AddBillBody(List<BillBody> billBodyDTOs, int billID, int index = 0)
        {

            int rowEffected;

            string query = $@"INSERT INTO {BILLS_BODY}
                              ({BBILL_COLUMN_BILL_ID}, {BBILL_COLUMN_PRODUCT_ID}, {BBILL_COLUMN_QUANTITY}, {BBILL_COLUMN_PRICE}, {BBILL_COLUMN_CURRENCY_ID})
                              VALUES
                              (@billID, @productID, @qty, @price, @currency)";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@billID", billID);
            command.Parameters.AddWithValue("@productID", billBodyDTOs[index].ProductID);
            command.Parameters.AddWithValue("@qty", billBodyDTOs[index].Qty);
            command.Parameters.AddWithValue("@price", billBodyDTOs[index].SellingPrice);
            command.Parameters.AddWithValue("@currency", billBodyDTOs[index].CurrencyID);

            try
            {
                rowEffected = command.ExecuteNonQuery(); // Execute Query.

                if (index != billBodyDTOs.Count - 1)
                {
                    // Here we perfom Recursion in order to increase the index,
                    // and add all products to the bill.
                    AddBillBody(billBodyDTOs, billID, index + 1);
                }
            }
            catch
            {
                rowEffected = -1;
            }


            return rowEffected != -1;
        }

        public static bool Update(BillDTO billDTO)
        {
            int rowEffected = -1;

            string query = $@"UPDATE {BILLS} 
                              SET {BILL_COLUMN_BUSINESS_ID} = @FromBusinessID,
                                  {BILL_COLUMN_CUSTOMER_ID} = @ToCustomerID,
                                  {BILL_COLUMN_AMOUNT} = @Amount,
                                  {BILL_COLUMN_DISCOUNT} = @Discount,
                                  {BILL_COLUMN_DATE} = @Date,
                                  {BILL_COLUMN_STATUS} = @BillStatus,
                                  {BILL_COLUMN_CURRENCY_RATE} = @CurrencyRate,
                                  {BILL_COLUMN_CURRENCY_ID} = @CurrencyID
                              WHERE {BILL_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", billDTO.BillID);
            command.Parameters.AddWithValue("@FromBusinessID", billDTO.FromBusinessID);
            command.Parameters.AddWithValue("@ToCustomerID", billDTO.ToCustomerID);
            command.Parameters.AddWithValue("@Amount", billDTO.Amount);
            command.Parameters.AddWithValue("@Discount", billDTO.Discount);
            command.Parameters.AddWithValue("@Date", billDTO.DateCreated);
            command.Parameters.AddWithValue("@BillStatus", billDTO.BillStatus);
            command.Parameters.AddWithValue("@CurrencyRate", billDTO.CurrencyRate);
            command.Parameters.AddWithValue("@CurrencyID", billDTO.CurrencyID);


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

        public static bool Delete(int id)
        {

            // Delete body first.
            if (!DeleteBillBody(id))
                return false;

            int rowEffected = -1;

            string query = $@"DELETE {BILLS} WHERE {BILL_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);

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
        private static bool DeleteBillBody(int billID)
        {
            int rowEffected;

            string query = $@"DELETE {BILLS_BODY} WHERE {BBILL_COLUMN_BILL_ID} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", billID);

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

        public static BillDTO? GetBillByID(int billID)
        {
            BillDTO? billDTO = null;

            string query = $@"SELECT * FROM {BILLS} WHERE {BILL_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", billID);

            try
            {
                connection.Open();
                List<BillBody> billBody = FillBillBody(billID);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    billDTO = new
                        (
                        (int)reader[BILL_COLUMN_PK], 
                        (int)reader[BILL_COLUMN_BUSINESS_ID], 
                        (int)reader[BILL_COLUMN_CUSTOMER_ID], 
                        (decimal)reader[BILL_COLUMN_AMOUNT], 
                        (decimal)reader[BILL_COLUMN_AMOUNT], 
                        (DateTime)reader[BILL_COLUMN_DATE],
                        (byte)reader[BILL_COLUMN_STATUS], 
                        (decimal)reader[BILL_COLUMN_CURRENCY_RATE], 
                        (int)reader[BBILL_COLUMN_CURRENCY_ID], 
                        (int)reader[BILL_COLUMN_USER_ID],
                        billBody
                        );
                }
            }
            catch
            {
                billDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return billDTO;
        }
        private static List<BillBody> FillBillBody(int billID)
        {
            List<BillBody> billBodyList = new();

            string query = $@"SELECT * FROM {BILLS_BODY} WHERE {BBILL_COLUMN_BILL_ID} = @billBody";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@billBody", billID);

            try
            {

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    billBodyList.Add(new BillBody(
                        (int)reader[BBILL_COLUMN_PK],
                        (int)reader[BBILL_COLUMN_BILL_ID],
                        (int)reader[BBILL_COLUMN_PRODUCT_ID],
                        (int)reader[BBILL_COLUMN_QUANTITY],
                        (decimal)reader[BBILL_COLUMN_PRICE],
                        (int)reader[BBILL_COLUMN_CURRENCY_ID]
                        ));
                }
            }
            catch
            {
                
            }

            return billBodyList;
        }

        private static void FillBillBody(ref List<BillDetailsDTO> billDetailsDTOs, int index = 0)
        {

            List<BillBodyDetails> details = new();

            string query = $@"SELECT {PRODUCT_COLUMN_MODEL}, {BBILL_COLUMN_QUANTITY}, {BBILL_COLUMN_PRICE}, {CURRENCIES_COLUMN_CODE}
                              FROM {BILLS_BODY} 
                              JOIN {PRODUCTS} ON {BILLS_BODY}.{BBILL_COLUMN_PRODUCT_ID} = {PRODUCTS}.{PRODUCT_COLUMN_PK}
                              JOIN {CURRENCIES} ON {BILLS_BODY}.{BBILL_COLUMN_CURRENCY_ID} = {CURRENCIES}.{CURRENCIES_COLUMN_PK}
                              WHERE {BILLS_BODY}.{BBILL_COLUMN_BILL_ID} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", billDetailsDTOs[index].BillID);

            try
            {
                if (index != billDetailsDTOs.Count)
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        details.Add(new BillBodyDetails
                            (
                            (string)reader[PRODUCT_COLUMN_MODEL],
                            (int)reader[BBILL_COLUMN_QUANTITY],
                            (decimal)reader[BBILL_COLUMN_PRICE],
                            (string)reader[CURRENCIES_COLUMN_CODE]
                            ));

                        // Add list of details to the bill
                        billDetailsDTOs[index].BodyList = details;
                    }
                    reader.Close();

                    // Perform Recursion in order to fill each bill with it's body.
                    FillBillBody(ref billDetailsDTOs, index + 1);
                }
            }
            catch
            {

            }
        }
        public static List<BillDetailsDTO> GetAllBills()
        {
            List<BillDetailsDTO> billDetailsDTOs = new();

            string query = $@"
                            SELECT {BILLS}.*,
                            From_Business = {BUSINESS_COLUMN_NAME},
                            To_Customer = {PERSON_COLUMN_FIRST_NAME} + ' ' + {PERSON_COLUMN_SECOND_NAME} + ' ' + {PERSON_COLUMN_LAST_NAME},
                            {CURRENCIES_COLUMN_CODE},
                            Created_By_User = {USER_COLUMN_USERNAME}
                            FROM {BILLS}
                            JOIN {BUSINESSES} ON {BILLS}.{BILL_COLUMN_BUSINESS_ID} = {BUSINESSES}.{BUSINESS_COLUMN_PK}
                            JOIN {CURRENCIES} ON {BILLS}.{BILL_COLUMN_CURRENCY_ID} = {CURRENCIES}.{CURRENCIES_COLUMN_PK}
                            JOIN {USERS} ON {BILLS}.{BILL_COLUMN_USER_ID} = {USERS}.{USER_COLUMN_PERSON_ID} 
                            JOIN {CUSTOMERS} ON {BILLS}.{BILL_COLUMN_CUSTOMER_ID} = {CUSTOMERS}.{CUSTOMER_COLUMN_PK}
                            JOIN {PEOPLE} ON {CUSTOMERS}.{CUSTOMER_COLUMN_PERSON_ID} = {PEOPLE}.PersonID";

            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {      
                    billDetailsDTOs.Add(new BillDetailsDTO
                        (
                        (int)reader[BILL_COLUMN_PK],
                        (string)reader["From_Business"],
                        (string)reader["To_Customer"],
                        (decimal)reader[BILL_COLUMN_AMOUNT],
                        (decimal)reader[BILL_COLUMN_DISCOUNT],
                        (DateTime)reader[BILL_COLUMN_DATE],
                        (byte)reader[BILL_COLUMN_STATUS],
                        (decimal)reader[BILL_COLUMN_CURRENCY_RATE],
                        (string)reader[CURRENCIES_COLUMN_CODE],
                        (string)reader["Created_By_User"],
                        null
                        ));
                }
                reader.Close();

                FillBillBody(ref billDetailsDTOs); // Here, Fill the bill with the products list.
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return billDetailsDTOs;
        }
        public static List<BillDetailsDTO> GetAllBillByBusiness(int businessID)
        {
            List<BillDetailsDTO> billDetailsDTOs = new();

            string query = $@"
                            SELECT {BILLS}.*,
                            From_Business = {BUSINESS_COLUMN_NAME},
                            To_Customer = {PERSON_COLUMN_FIRST_NAME} + ' ' + {PERSON_COLUMN_SECOND_NAME} + ' ' + {PERSON_COLUMN_LAST_NAME},
                            {CURRENCIES_COLUMN_CODE},
                            Created_By_User = {USER_COLUMN_USERNAME}
                            FROM {BILLS}
                            JOIN {BUSINESSES} ON {BILLS}.{BILL_COLUMN_BUSINESS_ID} = {BUSINESSES}.{BUSINESS_COLUMN_PK}
                            JOIN {CURRENCIES} ON {BILLS}.{BILL_COLUMN_CURRENCY_ID} = {CURRENCIES}.{CURRENCIES_COLUMN_PK}
                            JOIN {USERS} ON {BILLS}.{BILL_COLUMN_USER_ID} = {USERS}.{USER_COLUMN_PERSON_ID} 
                            JOIN {CUSTOMERS} ON {BILLS}.{BILL_COLUMN_CUSTOMER_ID} = {CUSTOMERS}.{CUSTOMER_COLUMN_PK}
                            JOIN {PEOPLE} ON {CUSTOMERS}.{CUSTOMER_COLUMN_PERSON_ID} = {PEOPLE}.PersonID
                            WHERE {BUSINESS_COLUMN_PK} = @businessID";


            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@businessID", businessID);                

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    billDetailsDTOs.Add(new BillDetailsDTO
                        (
                        (int)reader[BILL_COLUMN_PK],
                        (string)reader["From_Business"],
                        (string)reader["To_Customer"],
                        (decimal)reader[BILL_COLUMN_AMOUNT],
                        (decimal)reader[BILL_COLUMN_DISCOUNT],
                        (DateTime)reader[BILL_COLUMN_DATE],
                        (byte)reader[BILL_COLUMN_STATUS],
                        (decimal)reader[BILL_COLUMN_CURRENCY_RATE],
                        (string)reader[CURRENCIES_COLUMN_CODE],
                        (string)reader["Created_By_User"],
                        null
                        ));
                }
                reader.Close();

                FillBillBody(ref billDetailsDTOs); // Here, Fill the bill with the products list.
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return billDetailsDTOs;

        }
        public static List<BillDetailsDTO> GetAllBillByBusinessAndDate(int businessID, DateTime start, DateTime end)
        {
            List<BillDetailsDTO> billDetailsDTOs = new();

            string query = $@"
                            SELECT {BILLS}.*,
                            From_Business = {BUSINESS_COLUMN_NAME},
                            To_Customer = {PERSON_COLUMN_FIRST_NAME} + ' ' + {PERSON_COLUMN_SECOND_NAME} + ' ' + {PERSON_COLUMN_LAST_NAME},
                            {CURRENCIES_COLUMN_CODE},
                            Created_By_User = {USER_COLUMN_USERNAME}
                            FROM {BILLS}
                            JOIN {BUSINESSES} ON {BILLS}.{BILL_COLUMN_BUSINESS_ID} = {BUSINESSES}.{BUSINESS_COLUMN_PK}
                            JOIN {CURRENCIES} ON {BILLS}.{BILL_COLUMN_CURRENCY_ID} = {CURRENCIES}.{CURRENCIES_COLUMN_PK}
                            JOIN {USERS} ON {BILLS}.{BILL_COLUMN_USER_ID} = {USERS}.{USER_COLUMN_PERSON_ID} 
                            JOIN {CUSTOMERS} ON {BILLS}.{BILL_COLUMN_CUSTOMER_ID} = {CUSTOMERS}.{CUSTOMER_COLUMN_PK}
                            JOIN {PEOPLE} ON {CUSTOMERS}.{CUSTOMER_COLUMN_PERSON_ID} = {PEOPLE}.PersonID
                            WHERE {BUSINESS_COLUMN_PK} = @businessID
                            AND {BILL_COLUMN_DATE} BETWEEN @start and @end";


            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@businessID", businessID);
            command.Parameters.AddWithValue("@start", start);
            command.Parameters.AddWithValue("@end", end);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    billDetailsDTOs.Add(new BillDetailsDTO
                        (
                        (int)reader[BILL_COLUMN_PK],
                        (string)reader["From_Business"],
                        (string)reader["To_Customer"],
                        (decimal)reader[BILL_COLUMN_AMOUNT],
                        (decimal)reader[BILL_COLUMN_DISCOUNT],
                        (DateTime)reader[BILL_COLUMN_DATE],
                        (byte)reader[BILL_COLUMN_STATUS],
                        (decimal)reader[BILL_COLUMN_CURRENCY_RATE],
                        (string)reader[CURRENCIES_COLUMN_CODE],
                        (string)reader["Created_By_User"],
                        null
                        ));
                }
                reader.Close();

                FillBillBody(ref billDetailsDTOs); // Here, Fill the bill with the products list.
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return billDetailsDTOs;

        }
        public static decimal GetTotalSelles(int businessID, int currencyID, DateTime start, DateTime end)
        {
            decimal total = 0;

            string query = $@"SELECT SUM({BILLS}.{BILL_COLUMN_AMOUNT}) as total FROM {BILLS}
                            WHERE {BILL_COLUMN_BUSINESS_ID} = @businessID AND {BILL_COLUMN_CURRENCY_ID} = @currencyID 
                            AND {BILL_COLUMN_DATE} BETWEEN @start and @end";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@currencyID", currencyID);
            command.Parameters.AddWithValue("@businessID", businessID);
            command.Parameters.AddWithValue("@start", start);
            command.Parameters.AddWithValue("@end", end);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    total = (decimal)reader["total"];
                
            }
            catch
            {
                total = -1;
            }
            finally
            {
                connection.Close();
            }


            return total;
        }

    }
} 