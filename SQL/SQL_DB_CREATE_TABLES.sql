--Create Database FBMDB;

--use master

--SELECT table_name
--FROM INFORMATION_SCHEMA.TABLES
--WHERE table_type = 'BASE TABLE';

--USE FBMDB;

--USE master;

CREATE TABLE [BUSINESSES] (
	[BusinessID] int identity(1, 1) NOT NULL,
	[BusinessName] nvarchar(100) NOT NULL,
	PRIMARY KEY ([BusinessID])
);



CREATE TABLE [CURRENCIES] (
	[CurrencyID] int identity(1, 1) NOT NULL,
	[Country] nvarchar(30) NOT NULL,
	[Code] varchar(3) NOT NULL,
	[Name] nvarchar(40) NOT NULL,
	[Rate] money NOT NULL,
	PRIMARY KEY ([CurrencyID])
);



CREATE TABLE [BRANDS] (
	[BrandID] int identity(1, 1) NOT NULL,
	[BrandName] nvarchar(30) NOT NULL,
	PRIMARY KEY ([BrandID])
);



CREATE TABLE [COLORS] (
	[ColorID] int identity(1, 1) NOT NULL,
	[ColorName] nvarchar(30) NOT NULL,
	PRIMARY KEY ([ColorID])
);



CREATE TABLE [TERMS] (
	[TermID] int identity(1, 1) NOT NULL,
	[Description] nvarchar(500) NOT NULL,
	PRIMARY KEY ([TermID])
);


CREATE TABLE [STORAGES] (
  [StorageID] int identity(1, 1) NOT NULL,
  [StorageCapacity] nvarchar(15),
  PRIMARY KEY ([StorageID])
);


CREATE TABLE [PRODUCTS] (
	[ProductID] int identity(1, 1) NOT NULL,
	[BrandID] int REFERENCES BRANDS(BrandID) NOT NULL,
	[Model] nvarchar(20) NOT NULL,
	[ColorID] int REFERENCES COLORS(ColorID) NOT NULL,
	[StorageID] int REFERENCES STORAGES(StorageID) NOT NULL,
	[Condition] bit NOT NULL,
	[Quality] tinyint NOT NULL,
	[TermID] int REFERENCES TERMS(TermID) NOT NULL,
	[CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
	[PurchaPrice] MONEY NOT NULL,
	[ProductImage] varchar(250) NULL,
	PRIMARY KEY ([ProductID])
);



CREATE TABLE [PEOPLE] (
	[PersonID] int identity(1, 1) NOT NULL,
	[FirstName] nvarchar(30) NOT NULL,
	[SecondName] nvarchar(30) NULL,
	[LastName] nvarchar(30) NOT NULL,
	[Gendor] bit NOT NULL,
	[Address] nvarchar(100) NULL,
	[Email] nvarchar(50) NULL,
	[Phone] nvarchar(20) NULL,
	PRIMARY KEY ([PersonID])
);



CREATE TABLE [CUSTOMERS] (
	[CustomerID] int identity(1, 1) NOT NULL,
	[PersonID] int REFERENCES PEOPLE(PersonID) NOT NULL,
	PRIMARY KEY ([CustomerID])
);



CREATE TABLE [USERS] (
	[UserID] int identity(1, 1) NOT NULL,
	[PersonID] int REFERENCES PEOPLE(PersonID) NOT NULL,
	[Username] nvarchar(25) NOT NULL,
	[Password] nvarchar(25) NOT NULL,
	[IsActive] bit NOT NULL,
	[Permission] int NOT NULL,
	PRIMARY KEY ([UserID])
);



CREATE TABLE [BILLS] (
  [BillID] int identity(1, 1) NOT NULL,
  [FromBusinessID] int REFERENCES BUSINESSES(BusinessID) NOT NULL,
  [ToCustomerID] int REFERENCES CUSTOMERS(CustomerID) NOT NULL,
  [Amount] MONEY NOT NULL,
  [Discount] smallmoney NOT NULL,
  [Date] date NOT NULL,
  [BillStatus] tinyint NOT NULL,
  [CurrencyRate] smallmoney NOT NULL,
  [CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
  [CreatedByUserID] int REFERENCES USERS(UserID) NOT NULL,
  PRIMARY KEY ([BillID])
);



CREATE TABLE [BILLS_BODY] (
  [BBodyID] int identity(1, 1) NOT NULL,
  [BillID] int REFERENCES BILLS(BillID) NOT NULL,
  [ProductID] int REFERENCES PRODUCTS(ProductID) NOT NULL,
  [Quantity] int NOT NULL,
  [SellingPrice] MONEY NOT NULL,
  [CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
  PRIMARY KEY ([BBodyID])
);



CREATE TABLE [METHODS] (
  [MethodID] int identity(1, 1) NOT NULL,
  [MethodName] nvarchar(50) NOT NULL,
  PRIMARY KEY ([MethodID])
);



CREATE TABLE [PAYMENTS] (
  [PaymentID] int identity(1, 1) NOT NULL,
  [CustomerID] int REFERENCES CUSTOMERS(CustomerID) NOT NULL,
  [BillID] int REFERENCES BILLS(BillID) NOT NULL,
  [CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
  [MethodID] int REFERENCES METHODS(MethodID) NOT NULL,
  [CurrencyRate] smallmoney NOT NULL,
  [AmountPaid] MONEY NOT NULL,
  [ReturnAmount] MONEY NOT NULL,
  [PaymentDate] DATE NOT NULL,
  [Note] nvarchar(250) NULL,
  PRIMARY KEY ([PaymentID])
);



--CREATE TABLE [RETURNS] (
--  [ReturnID] int identity(1, 1) NOT NULL,
--  [FromCustomerID] int REFERENCES CUSTOMERS(CustomerID) NOT NULL,
--  [ReturnAmount] smallmoney NOT NULL,
--  [Date] date NOT NULL,
--  [CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
--  [CreatedByUserID] int REFERENCES USERS(UserID) NOT NULL,
--  PRIMARY KEY ([ReturnID])
--);




--CREATE TABLE [RETURNS_BODY] (
--  [RBodyID] int identity(1, 1) NOT NULL,
--  [ReturnID] int REFERENCES RETURNS(ReturnID) NOT NULL,
--  [CurrencyID] int REFERENCES CURRENCIES(CurrencyID) NOT NULL,
--  [ProductID] int REFERENCES PRODUCTS(ProductID) NOT NULL,
--  [Quantity] int NOT NULL,
--  [SellingPrice] MONEY NOT NULL,
--  PRIMARY KEY ([RBodyID])
--);
