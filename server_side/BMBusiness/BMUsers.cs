using BMData;

namespace BMBusiness
{
    public class BMUser
    {
        private Mode mode = Mode.Add;

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int Permission { get; set; }
        public int PersonID { get; set; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public bool Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public UserDTO UDTO { get { return new UserDTO(UserID, UserName, Password, IsActive, Permission, PersonID, FirstName, SecondName, LastName, Email, Gender, Address, Phone); } }

        public BMUser(UserDTO userDTO, Mode mode = Mode.Add)
        {
            UserID = userDTO.UserID;
            UserName = userDTO.UserName;
            Password = userDTO.Password;
            IsActive = userDTO.IsActive;
            Permission = userDTO.Permission;
            PersonID = userDTO.PersonID;
            FirstName = userDTO.FirstName;
            SecondName = userDTO.SecondName;
            LastName = userDTO.LastName;
            Email = userDTO.Email;
            Gender = userDTO.Gender;
            Address = userDTO.Address;
            Phone = userDTO.Phone;

            this.mode = mode;
        }

        public static List<UserDTO> Users() => UserDB.GetAllUsers();
        
        public static BMUser? Find(int id)
            => UserDB.GetUserByID(id) is UserDTO userDTO ? new BMUser(userDTO, Mode.Update) : null;

        public static BMUser? Find(string username)
            => UserDB.GetUserByUsername(username) is UserDTO userDTO ? new BMUser(userDTO, Mode.Update) : null;


        private bool AddNew()
        {
            // Insert a person first (Parent First). Perform Upcasting to CDTO.
            int personID = PersonDB.AddNewPerson(UDTO);
            if (personID == -1)
                return false;



            UserID = UserDB.AddNewUser(UDTO, personID);
            return UserID != -1;
        }
        private bool Update() => UserDB.Update(UDTO);
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


                               // Delete child first(User)         then -> delete parent(Person).
        public bool Delete() => UserDB.Delete(UserID) && PersonDB.DeletePerson(PersonID);

        public static bool IsExists(string username) => UserDB.IsUserExists(username);
    }
}
