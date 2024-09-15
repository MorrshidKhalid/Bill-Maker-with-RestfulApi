using BMData;

namespace BMBusiness
{
    public class Payment
    {
        private Mode mode = Mode.Add;

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

        public PaymentDTO PDTO { get { return new PaymentDTO(PaymentID, CustomerID, BillID, CarrencyID, MethodID, CurrencyRate, AmountPaid, RefindAmount, Date, Note); } }


        //  --- Constructor.
        public Payment(PaymentDTO paymentDTO, Mode mode = Mode.Add)
        {
            PaymentID = paymentDTO.PaymentID;
            CustomerID = paymentDTO.CustomerID;
            BillID = paymentDTO.BillID;
            CarrencyID = paymentDTO.CarrencyID;
            MethodID = paymentDTO.MethodID;
            CurrencyRate = paymentDTO.CurrencyRate;
            AmountPaid = paymentDTO.AmountPaid;
            RefindAmount = paymentDTO.RefindAmount;
            Date = paymentDTO.Date;
            Note = paymentDTO.Note;

            this.mode = mode;
        }

        private bool AddNew()
        {
            PaymentID = PaymentDB.AddNewPayment(PDTO);
            return PaymentID != -1;
        }
        private bool Update() => PaymentDB.UpdatePayment(PDTO);
        public bool Save()
        {
            switch (mode)
            {
                case Mode.Add:
                    if (AddNew())
                    {
                        mode = Mode.Update;
                        return true;
                    }
                    else return false;

                case Mode.Update:
                    return Update();
            }

            return false;
        }

        public bool Delete() => PaymentDB.Delete(PaymentID);


        public static Payment? Find(int paymentID)
            => PaymentDB.GetPaymentByID(paymentID) is PaymentDTO paymentDTO ? new Payment(paymentDTO, Mode.Update) : null;
    }
}
