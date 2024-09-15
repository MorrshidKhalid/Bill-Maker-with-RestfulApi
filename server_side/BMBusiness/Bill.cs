using BMData;
namespace BMBusiness
{
    public class Bill
    {
        private Mode mode = Mode.Add;

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
        public List<BillBody> BodyList;
        public Business? business;
        public Customer? customer;
        public Currency? currency;
        public BMUser? user;
        public BillDTO BDTO { get { return new BillDTO(BillID, FromBusinessID, ToCustomerID, Amount, Discount, DateCreated, BillStatus, CurrencyRate, CurrencyID, CreatedByUserID, BodyList); } }


        public Bill(BillDTO billDTO, Mode mode = Mode.Add)
        {
            BillID = billDTO.BillID;
            FromBusinessID = billDTO.FromBusinessID;
            ToCustomerID = billDTO.ToCustomerID;
            Amount = billDTO.Amount;
            Discount = billDTO.Discount;
            DateCreated = billDTO.DateCreated;
            BillStatus = billDTO.BillStatus;
            CurrencyRate = billDTO.CurrencyRate;
            CurrencyID = billDTO.CurrencyID;
            CreatedByUserID = billDTO.CreatedByUserID;
            BodyList = billDTO.BodyList;

            business = Business.Find(FromBusinessID);
            customer = Customer.Find(ToCustomerID);
            currency = Currency.Find(CurrencyID);
            user = BMUser.Find(CreatedByUserID);

            this.mode = mode;
        }


        public static List<BillDetailsDTO> BillsWithDetails() => BillDB.GetAllBills();
        public static List<BillDetailsDTO> BillsByBusiness(int businessID) => BillDB.GetAllBillByBusiness(businessID);
        public static List<BillDetailsDTO> BillsByBusinessBetweenDate(int businessID, DateTime start, DateTime end) => BillDB.GetAllBillByBusinessAndDate(businessID, start, end);
        public static decimal TotalSelles(int businessID, int currencyID, DateTime start, DateTime end)
            => BillDB.GetTotalSelles(businessID, currencyID, start, end);

        public static Bill? Find(int id) 
            => BillDB.GetBillByID(id) is BillDTO billDTO ? new Bill(billDTO, Mode.Update) : null;

        private bool AddNew()
        {
            BillID = BillDB.AddNewBill(BDTO);

            return BillID != -1;
        }
        private bool Update() => BillDB.Update(BDTO);
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

        public bool Delete() => BillDB.Delete(BillID);
    }
}
