using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Payment;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;

namespace BMData
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public int BillID { get; set; }
        public int CarrencyID { get; set; }
        public int MethodID { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RefindAmount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public PaymentDTO(int paymentID, int customerID, int billID, int carrencyID, int methodID, decimal currencyRate, decimal amountPaid, decimal refindAmount, DateTime date, string note)
        {
            PaymentID = paymentID;
            CustomerID = customerID;
            BillID = billID;
            CarrencyID = carrencyID;
            MethodID = methodID;
            CurrencyRate = currencyRate;
            AmountPaid = amountPaid;
            RefindAmount = refindAmount;
            Date = date;
            Note = note;
        }
    }

    public class CustomerPaymentDTO
    {
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPayments { get; set; }
        public string Currency { get; set; }

        public CustomerPaymentDTO(string customer, DateTime date, decimal totalPayments, string currency)
        {
            Customer = customer;
            Date = date;
            TotalPayments = totalPayments;
            Currency = currency;
        }
    }

    public class PaymentDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);


        public static int AddNewPayment(PaymentDTO paymentDTO)
        {
            int insrtedID = -1;


            string query = $@"INSERT INTO {PAYMENTS}
                              (
                              {PAYMENT_COLUMN_CUSTOMER_ID},
                              {PAYMENT_COLUMN_BILL_ID},
                              {PAYMENT_COLUMN_CURRENCY_ID},
                              {PAYMENT_COLUMN_METHOD_ID},
                              {PAYMENT_COLUMN_RATE},
                              {PAYMENT_COLUMN_AMOUNT},
                              {PAYMENT_COLUMN_REFIND},
                              {PAYMENT_COLUMN_DATE},
                              {PAYMENT_COLUMN_NOTE}
                              )
                              VALUES
                              (@customerID, @billID, @currencyID, @methodID, @currencyRate, @amountPaid, @returnAmount, @date, @note);
                              {BMContract.SCOPE_IDENTITY};";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@customerID", paymentDTO.CustomerID);
            command.Parameters.AddWithValue("@billID", paymentDTO.BillID);
            command.Parameters.AddWithValue("@currencyID", paymentDTO.CarrencyID);
            command.Parameters.AddWithValue("@methodID", paymentDTO.MethodID);
            command.Parameters.AddWithValue("@currencyRate", paymentDTO.CurrencyRate);
            command.Parameters.AddWithValue("@amountPaid", paymentDTO.AmountPaid);
            command.Parameters.AddWithValue("@returnAmount", paymentDTO.RefindAmount);
            command.Parameters.AddWithValue("@date", paymentDTO.Date);
            DBLib.HandleInsertDBNull(command, "@note", paymentDTO.Note);

            try
            {
                connection.Open();
                DBLib.ExecAndGetInsertedID(ref insrtedID, command);
            }
            catch
            {
                insrtedID = -1;
            }
            finally
            {
                connection.Close();
            }

            return insrtedID;
        }
        public static bool UpdatePayment(PaymentDTO paymentDTO)
        {
            int rowEffected = -1;

            string query = $@"UPDATE {PAYMENTS} 
                              SET {PAYMENT_COLUMN_CUSTOMER_ID} = @customerID,
                                  {PAYMENT_COLUMN_BILL_ID} = @billID,
                                  {PAYMENT_COLUMN_CURRENCY_ID} = @currencyID,
                                  {PAYMENT_COLUMN_METHOD_ID} = @methodID,
                                  {PAYMENT_COLUMN_RATE} = @currencyRate,
                                  {PAYMENT_COLUMN_AMOUNT} = @amountPaid,
                                  {PAYMENT_COLUMN_REFIND} = @returnAmount,
                                  {PAYMENT_COLUMN_DATE} = @paymentDate,
                                  {PAYMENT_COLUMN_NOTE} = @note
                              WHERE {PAYMENT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@customerID", paymentDTO.CustomerID);
            command.Parameters.AddWithValue("@billID", paymentDTO.BillID);
            command.Parameters.AddWithValue("@currencyID", paymentDTO.CustomerID);
            command.Parameters.AddWithValue("@methodID", paymentDTO.MethodID);
            command.Parameters.AddWithValue("@currencyRate", paymentDTO.CurrencyRate);
            command.Parameters.AddWithValue("@amountPaid", paymentDTO.AmountPaid);
            command.Parameters.AddWithValue("@returnAmount", paymentDTO.RefindAmount);
            command.Parameters.AddWithValue("@paymentDate", paymentDTO.Date);
            DBLib.HandleInsertDBNull(command, "@note", paymentDTO.Note);
            command.Parameters.AddWithValue("@id", paymentDTO.PaymentID);

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
        public static bool Delete(int paymentID)
        {
            int rowEffected = -1;

            string query = $@"DELETE {PAYMENTS} WHERE {PAYMENT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", paymentID);

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
        public static PaymentDTO? GetPaymentByID(int paymentID)
        { 
            PaymentDTO? paymentDTO = null;

            string query = $@"SELECT * FROM {PAYMENTS} WHERE {PAYMENT_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", paymentID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    paymentDTO = new PaymentDTO
                        (
                        (int)reader[PAYMENT_COLUMN_PK],
                        (int)reader[PAYMENT_COLUMN_CUSTOMER_ID],
                        (int)reader[PAYMENT_COLUMN_BILL_ID],
                        (int)reader[PAYMENT_COLUMN_CURRENCY_ID],
                        (int)reader[PAYMENT_COLUMN_METHOD_ID],
                        (decimal)reader[PAYMENT_COLUMN_RATE],
                        (decimal)reader[PAYMENT_COLUMN_AMOUNT],
                        (decimal)reader[PAYMENT_COLUMN_REFIND],
                        (DateTime)reader[PAYMENT_COLUMN_DATE],
                        (string)reader[PAYMENT_COLUMN_NOTE]
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


            return paymentDTO;
        }
    }
}
