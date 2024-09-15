using BMData;

namespace BMBusiness
{
    public class Customer
    {
        private Mode mode = Mode.Add;

        public int CustomerID { get; set; }
        public int PersonID { get; set; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public bool Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public CustomerDTO CDTO { get { return new CustomerDTO(CustomerID, PersonID, FirstName, SecondName, LastName, Email, Gender, Address, Phone); } }

        public Customer(CustomerDTO customerDTO, Mode mode = Mode.Add)
        {
            CustomerID = customerDTO.CustomerID;
            PersonID = customerDTO.PersonID;
            FirstName = customerDTO.FirstName;
            SecondName = customerDTO.SecondName;
            LastName = customerDTO.LastName;
            Email = customerDTO.Email;
            Gender = customerDTO.Gender;
            Address = customerDTO.Address;
            Phone = customerDTO.Phone;

            this.mode = mode;
        }


        public static List<CustomerDTO> AllCustomers() => CustomerDB.GetAllCustomers();

        public static Customer? Find(int customerID) 
            => CustomerDB.GetCustomerByID(customerID) is CustomerDTO customerDTO ? new Customer(customerDTO, Mode.Update) : null;

        public static int CountCustomersByGender(bool gendor) => CustomerDB.GetTotalCutomersByGendor(gendor);


        private bool AddNew()
        {
            // Insert a person first (Parent First). Perform Upcasting to CDTO.
            int personID = PersonDB.AddNewPerson(CDTO);
            if (personID == -1)
                return false;

            

            CustomerID = CustomerDB.AddNewCustomer(personID);
            return CustomerID != -1;
        }
        private bool Update() => PersonDB.UpdatePerson(CDTO);
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


        // Delete child first(Customer)         then -> delete parent(Person).
        public bool Delete() => CustomerDB.DeleteCustomer(CustomerID) && PersonDB.DeletePerson(PersonID);
    }
}
