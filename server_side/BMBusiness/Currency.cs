using BMData;
namespace BMBusiness
{
    public class Currency
    {
        public int CurrencyID { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }

        public CurrencyDTO CDTO { get { return new CurrencyDTO(CurrencyID, Country, Code, Name, Rate); } }

        public Currency(CurrencyDTO currencyDTO)
        {
            CurrencyID = currencyDTO.CurrencyID;
            Country = currencyDTO.Country;
            Code = currencyDTO.Code;
            Name = currencyDTO.Name;
            Rate = currencyDTO.Rate;
        }

        public static List<CurrencyDTO> CurrencyList() => CurrencyDB.GetAllCurrencies();

        public static Currency? Find(int id)
            => CurrencyDB.GetCurrencyByID(id) is CurrencyDTO currencyDTO ? new Currency(currencyDTO) : null;

        public static List<CurrencyDTO>? Find(string code) => CurrencyDB.GetCurrencyByCode(code);

        public bool UpdateRate() => CurrencyDB.UpdateRate(CDTO);

        public static bool UpdateRate(int id)
        {
            var currencyDTO = CurrencyDB.GetCurrencyByID(id);
            if (currencyDTO != null)
                return CurrencyDB.UpdateRate(new Currency(currencyDTO).CDTO);


            return false;
        }
    }
}