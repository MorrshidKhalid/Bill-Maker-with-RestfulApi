using BMData;

namespace BMBusiness
{
    public class Storage
    {
        private Mode mode = Mode.Add;

        public int StorageID { get; set; }
        public string Capacity { get; set; }
        public StorageDTO SDTO { get { return new StorageDTO(StorageID, Capacity); } }

        //  --- Constructor.
        public Storage(StorageDTO storageDTO, Mode mode = Mode.Add)
        {
            StorageID = storageDTO.StorageID;
            Capacity = storageDTO.Capacity;

            this.mode = mode;
        }


        public static List<StorageDTO>? Storages() => StorageDB.GetAllStorages();

        public static Storage? Find(int storageID)
            => StorageDB.GetStorageByID(storageID) is StorageDTO storageDTO ? new Storage(storageDTO, Mode.Update) : null;

        public static Storage? Find(string capacity)
            => StorageDB.GetStorageByCapacity(capacity) is StorageDTO storageDTO ? new Storage(storageDTO, Mode.Update) : null;

        public static bool IsExists(string capacity) => StorageDB.IsExists(capacity);

        public bool Delete() => StorageDB.DeleteStorage(StorageID);

        private bool AddNew()
        {
            StorageID = StorageDB.AddNewStorage(SDTO);
            return StorageID != -1;
        }
        private bool Update() => StorageDB.UpdateStorage(SDTO);
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
