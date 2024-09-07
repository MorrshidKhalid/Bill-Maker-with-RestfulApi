using BMData;

namespace BMBusiness
{
    public class Product
    {

        private Mode mode = Mode.Add;

        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public string Model { get; set; }
        public int ColorID { get; set; }
        public int StorageID { get; set; }
        public bool Condition { get; set; }
        public int TermID { get; set; }
        public int CurrencyID { get; set; }
        public decimal PurchaPrice { get; set; }
        public string ImagePath { get; set; }
        public byte Quality { get; set; }

        public Brand? brand = null;
        public Color? color = null;
        public Term? term = null;
        public Storage? storage = null;
        public Currency? currency = null;

        public ProductDTO PDTO { get { return new ProductDTO(
            ProductID, BrandID, Model, ColorID,
            StorageID, Condition, TermID, CurrencyID,
            PurchaPrice, ImagePath, Quality); } }


        //  --- Constructor.
        public Product(ProductDTO productDTO, Mode mode = Mode.Add)
        {
            ProductID = productDTO.ProductID;
            BrandID = productDTO.BrandID;
            Model = productDTO.Model;
            ColorID = productDTO.ColorID;
            StorageID = productDTO.StorageID;
            Condition = productDTO.Condition;
            TermID = productDTO.TermID;
            CurrencyID = productDTO.CurrencyID;
            PurchaPrice = productDTO.PurchaPrice;
            ImagePath = productDTO.ImagePath;
            Quality = productDTO.Quality;

            brand = Brand.Find(productDTO.BrandID);
            color = Color.Find(productDTO.ColorID);
            term = Term.Find(productDTO.TermID);
            storage = Storage.Find(productDTO.StorageID);
            currency = Currency.Find(productDTO.CurrencyID);

            this.mode = mode;
        }



        public static List<ProductDTO>? Producs() => ProductDB.GetAllProductsNormal();

        public static List<ProductDetailsDTO>? AllProducsDetails() => ProductDB.GetAllProductsWithDetails();

        public static List<ProductDetailsDTO>? ProducsByFilter(bool condition, string currencyCode)
            => ProductDB.GetProductsByFilter(condition, currencyCode);

        public static List<ProductDetailsDTO>? ProductsBetweenPrices(decimal firstPrice, decimal secondPrice)
            => ProductDB.GetProductsBetweenPrices(firstPrice, secondPrice);

        public static List<ProductDetailsDTO>? ProductsByBrand(string brand)
            => ProductDB.GetProductsByBrand(brand);

        public static Product? Find(int productID)
            => ProductDB.GetProductByID(productID) is ProductDTO productDTO ? new Product(productDTO, Mode.Update) : null;

        public bool Delete() => ProductDB.DeleteProduct(ProductID);


        private bool AddNew()
        {
            ProductID = ProductDB.AddNewProduct(PDTO);
            return ProductID != -1;
        }
        private bool Update() => ProductDB.UpdateProduct(PDTO);
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
