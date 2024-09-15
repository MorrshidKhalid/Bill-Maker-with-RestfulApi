using BMData;

namespace BMBusiness
{
    public class Method
    {
        private Mode mode = Mode.Add;

        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public MethodDTO MDTO { get { return new MethodDTO(MethodID, MethodName); } }


        //  --- Constructor.
        public Method(MethodDTO methodDTO, Mode mode = Mode.Add)
        {
            MethodID = methodDTO.MethodID;
            MethodName = methodDTO.MethodName;

            this.mode = mode;
        }


        public static List<MethodDTO>? Methods() => MethodDB.GetMethods();

        public static Method? Find(int methodID)
            => MethodDB.GetMethodByID(methodID) is MethodDTO methodDTO ? new Method(methodDTO, Mode.Update) : null;

        public static bool IsExists(string method) => MethodDB.IsExists(method);

        public bool Delete() => MethodDB.DeleteMethod(MethodID);

        private bool AddNew()
        {
            MethodID = MethodDB.AddNewMethod(MDTO);
            return MethodID != -1;
        }
        private bool Update() => MethodDB.UpdateMethod(MDTO);
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
