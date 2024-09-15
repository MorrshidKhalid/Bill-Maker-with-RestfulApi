using BMData;
namespace BMBusiness
{
    public class Business
    {
        private Mode mode = Mode.Add;

        public int BusinessID { get; set; }
        public string Name { get; set; }
        public BusinessDTO BDTO { get { return new BusinessDTO(BusinessID, Name); } }

        //  --- Constructor.
        public Business(BusinessDTO businessDTO, Mode mode = Mode.Add)
        {
            BusinessID = businessDTO.BusinessID;
            Name = businessDTO.Name;

            this.mode = mode;
        }


        public static List<BusinessDTO>? Businesses() => BusinessDB.GetAllBusinesses();
        public static Business? Find(int businessID)
            => BusinessDB.GetBusinessByID(businessID) is BusinessDTO businessDTO ? new Business(businessDTO, Mode.Update) : null;
        public static Business? Find(string name)
            => BusinessDB.GetBusinessByName(name) is BusinessDTO businessDTO ? new Business(businessDTO, Mode.Update) : null;
        public static bool IsExists(string name) => BusinessDB.IsExists(name);

        public bool Delete() => BusinessDB.DeleteBusiness(BusinessID);

        private bool AddNew()
        {
            BusinessID = BusinessDB.AddNewBusiness(BDTO);
            return BusinessID != -1;
        }
        private bool Update() => BusinessDB.UpdateBusiness(BDTO);
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
    }
}
