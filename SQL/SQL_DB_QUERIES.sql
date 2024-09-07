USE FBMDB;


--INSERT INTO CURRENCIES
--SELECT * FROM CurrenciesData;

--EXEC sp_rename 'PRODUCTS.ProductName', 'Model', 'Column';

ALTER TABLE PRODUCTS
ALTER COLUMN PurchaPrice MONEY;


SELECT * FROM CURRENCIES
SELECT * FROM BRANDS
SELECT * FROM COLORS
SELECT * FROM STORAGES
SELECT * FROM TERMS
SELECT * FROM PRODUCTS


SELECT 1 BrandName FROM BRANDS WHERE BrandName = 'Apple'

SELECT 
ProductID,
BRANDS.BrandName, 
PRODUCTS.Model, 
COLORS.ColorName,
STORAGES.StorageCapacity, 
PRODUCTS.Condition,
TERMS.Description, 
CURRENCIES.Code,
PRODUCTS.PurchaPrice, 
PRODUCTS.ProductImage,
PRODUCTS.Quality
FROM PRODUCTS
JOIN BRANDS ON PRODUCTS.BrandID = BRANDS.BrandID
JOIN COLORS ON PRODUCTS.ColorID= COLORS.ColorID
JOIN STORAGES ON PRODUCTS.StorageID = STORAGES.StorageID
JOIN TERMS ON PRODUCTS.TermID = TERMS.TermID
JOIN CURRENCIES ON PRODUCTS.CurrencyID = CURRENCIES.CurrencyID