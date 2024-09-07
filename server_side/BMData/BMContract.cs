namespace BMDataContract
{
    public static class BMContract
    {
        /* Inner class that defines the tables names */
        public static class Tables
        {
            public const string PEOPLE = "PEOPLE";
            public const string USERS = "USERS";
            public const string CURRENCIES = "CURRENCIES";
            public const string BRANDS = "BRANDS";
            public const string COLORS = "COLORS";
            public const string PHONES = "PHONES";
            public const string PAYMENTS = "PAYMENTS";
            public const string PAYMENT_METHODS = "METHODS";
            public const string STORAGES = "STORAGES";
            public const string TERMS = "TERMS";
            public const string PRODUCTS = "PRODUCTS";
            public const string BUSINESSES = "BUSINESSES";
            public const string CUSTOMERS = "CUSTOMERS";
        }



        /* Inner class that defines the (PERSONS) table contents */
        public static class Person
        {
            public const string PERSON_COLUMN_PK = "PersonID";
            public const string PERSON_COLUMN_FIRST_NAME = "FirstName";
            public const string PERSON_COLUMN_SECOND_NAME = "SecondName";
            public const string PERSON_COLUMN_LAST_NAME = "LastName";
            public const string PERSON_COLUMN_GENDOR = "Gendor";
            public const string PERSON_COLUMN_ADDRESS = "Address";
            public const string PERSON_COLUMN_EMAIL = "Email";
            public const string PERSON_COLUMN_PHONE = "Phone";
        }


        /* Inner class that defines the (USERS) table contents */
        public static class Users
        {
            public const string USER_COLUMN_PK = "UserID";
            public const string USER_COLUMN_PERSON_ID = "PersonID";
            public const string USER_COLUMN_IS_ACTIVE = "IsActive";
            public const string USER_COLUMN_USERNAME = "Username";
            public const string USER_COLUMN_PASSWORD = "Password";
        }


        /* Inner class that defines the (PHONES) table contents */
        public static class Phone
        {
            public const string PHONE_COLUMN_PK = "PhoneID";
            public const string PHONE_COLUMN_PERSON_ID = "PersonID";
            public const string PHONE_COLUMN_NUMBER = "Number";
        }




        /* Inner class that defines the (PAYMENT) table contents */
        public static class Payment
        {
            public const string PAYMENT_COLUMN_PK = "PaymentID";
        }



        /* Inner class that defines the (METHODS) table contents */
        public static class Method
        {
            public const string PAYMENT_METHOD_COLUMN_PK = "MethodID";
            public const string PAYMENT_METHOD_COLUMN_METHOD = "MethodName";
        }



        /* Inner class that defines the (CURRENCIES) table contents */
        public static class Currencies
        {
            public const string CURRENCIES_COLUMN_PK = "CurrencyID";
            public const string CURRENCIES_COLUMN_COUNTRY = "Country";
            public const string CURRENCIES_COLUMN_CODE = "Code";
            public const string CURRENCIES_COLUMN_NAME = "Name";
            public const string CURRENCIES_COLUMN_RATE = "Rate";
        }



        /* Inner class that defines the (BRANDS) table contents */
        public static class Brands
        {
            public const string BRANDS_COLUMN_PK = "BrandID";
            public const string BRANDS_COLUMN_NAME = "BrandName";
        }



        /* Inner class that defines the (COLORS) table contents */
        public static class Colors
        {
            public const string COLOR_COLUMN_PK = "ColorID";
            public const string COLOR_COLUMN_NAME = "ColorName";
        }



        /* Inner class that defines the (STORAGES) table contents */
        public static class Storages
        {
            public const string STORAGE_COLUMN_PK = "StorageID";
            public const string STORAGE_COLUMN_CAPACITY = "StorageCapacity";
        }


        /* Inner class that defines the (TERMS) table contents */
        public static class Terms
        {
            public const string TERM_COLUMN_PK = "TermID";
            public const string TERM_COLUMN_DESCRIPTION = "Description";
        }



        /* Inner class that defines the (PRODUCTS) table contents */
        public static class Product
        {
            public const string PRODUCT_COLUMN_PK = "ProductID";
            public const string PRODUCT_COLUMN_BRAND_ID = "BrandID";
            public const string PRODUCT_COLUMN_MODEL = "Model";
            public const string PRODUCT_COLUMN_COLOR_ID = "ColorID";
            public const string PRODUCT_COLUMN_STORAGE_ID = "StorageID";
            public const string PRODUCT_COLUMN_CONDITION = "Condition";
            public const string PRODUCT_COLUMN_TERM_ID = "TermID";
            public const string PRODUCT_COLUMN_CURRENCY_ID = "CurrencyID";
            public const string PRODUCT_COLUMN_PURCHA_PRICE = "PurchaPrice";
            public const string PRODUCT_COLUMN_IMAGE_PATH = "ProductImage";
            public const string PRODUCT_COLUMN_QUALITY = "Quality";
        }

        public static string SQL_SELECT_ALL_PRODUCTS_WITH_JOIN = $@"
                             SELECT 
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_PK},
                             {Tables.BRANDS}.{Brands.BRANDS_COLUMN_NAME},
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_MODEL},
                             {Tables.COLORS}.{Colors.COLOR_COLUMN_NAME},
                             {Tables.STORAGES}.{Storages.STORAGE_COLUMN_CAPACITY},
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_CONDITION},
                             {Tables.TERMS}.{Terms.TERM_COLUMN_DESCRIPTION},
                             {Tables.CURRENCIES}.{Currencies.CURRENCIES_COLUMN_CODE},
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_PURCHA_PRICE},
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_IMAGE_PATH},
                             {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_QUALITY}
                             FROM {Tables.PRODUCTS}
                             JOIN {Tables.BRANDS} ON {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_BRAND_ID} = {Tables.BRANDS}.{Brands.BRANDS_COLUMN_PK}
                             JOIN {Tables.COLORS} ON {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_COLOR_ID} = {Tables.COLORS}.{Colors.COLOR_COLUMN_PK}
                             JOIN {Tables.STORAGES} ON {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_STORAGE_ID} = {Tables.STORAGES}.{Storages.STORAGE_COLUMN_PK}
                             JOIN {Tables.TERMS} ON {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_TERM_ID} = {Tables.TERMS}.{Terms.TERM_COLUMN_PK}
                             JOIN {Tables.CURRENCIES} ON {Tables.PRODUCTS}.{Product.PRODUCT_COLUMN_CURRENCY_ID} = {Tables.CURRENCIES}.{Currencies.CURRENCIES_COLUMN_PK}";

        /* Inner class that defines the (BUSINESSES) table contents */
        public static class Business
        {
            public const string BUSINESS_COLUMN_PK = "BusinessID";
            public const string BUSINESS_COLUMN_NAME = "BusinessName";
            public const string BUSINESS_COLUMN_SIG = "Signatuer";
            public const string BUSINESS_COLUMN_LOGO = "Logo";
        }



        /* Inner class that defines the (CUSTOMERS) table contents */
        public static class Customer
        {
            public const string CUSTOMER_COLUMN_PK = "CustomerID";
            public const string CUSTOMER_COLUMN_PERSON_ID = "PersonID";
        }

        public const string SCOPE_IDENTITY = "SELECT SCOPE_IDENTITY();";
    }
}