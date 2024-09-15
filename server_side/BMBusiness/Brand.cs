using BMData;

namespace BMBusiness
{
    public class Brand
    {
        private Mode mode = Mode.Add;

        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public BrandDTO BDTO { get { return new BrandDTO(BrandID, BrandName); } }

        //  --- Constructor.
        public Brand(BrandDTO brandDTO, Mode mode = Mode.Add)
        {
            BrandID = brandDTO.BrandID;
            BrandName = brandDTO.BrandName;

            this.mode = mode;
        }


        public static List<BrandDTO>? Brands() => BrandDB.GetAllBrands();

        public static Brand? Find(int brandID)
            => BrandDB.GetBrandByID(brandID) is BrandDTO brandDTO ? new Brand(brandDTO, Mode.Update) : null;
        
        public static Brand? Find(string brandName)
            => BrandDB.GetBrandByName(brandName) is BrandDTO brandDTO ? new Brand(brandDTO, Mode.Update) : null;

        public static bool IsExists(string brandName) => BrandDB.IsExists(brandName);

        public bool Delete() => BrandDB.DeleteBrand(BrandID);

        private bool AddNew()
        {
            BrandID = BrandDB.AddNewBrand(BDTO);
            return BrandID != -1;
        }
        private bool Update() => BrandDB.UpdateBrand(BDTO);
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
