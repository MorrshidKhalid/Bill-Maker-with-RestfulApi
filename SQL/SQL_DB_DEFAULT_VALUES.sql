USE FBMDB;


Insert into METHODS(MethodName)
VALUES 
('CASH'),
('Jawali'),
('Kuraimi'),
('One Cash'),
('YKB');

Insert into COLORS(ColorName)
VALUES 
('Black'),
('White'),
('Red'),
('Blue'),
('Green');

Insert into BRANDS(BrandName)
VALUES 
('Apple'),
('Samsung'),
('Xiaomi'),
('Oppo'),
('Vivo'),
('Realme'),
('Google'),
('Huawei'),
('OnePlus'),
('Motorola'),
('Sony'),
('LG'),
('Tecno'),
('Infinix'),
('Itel');


Insert into STORAGES(StorageCapacity)
VALUES 
('4GB'),
('8GB'),
('16GB'),
('32GB'),
('64GB'),
('128GB'),
('256GB'),
('512GB'),
('1T');

INSERT INTO TERMS(Description)
VALUES 
('By accessing or using our website or purchasing products from our store, you agree to be bound by these Terms and Conditions.'),
('We make every effort to provide accurate product information. However, we cannot guarantee that all descriptions are error-free. Images are for illustrative purposes only.'),
('Prices are subject to change without notice. All payments must be made in full at the time of purchase. We accept [list of payment methods');


SELECT * FROM CURRENCIES
SELECT * FROM BRANDS
SELECT * FROM COLORS
SELECT * FROM STORAGES
SELECT * FROM TERMS
SELECT * FROM PRODUCTS


INSERT INTO PRODUCTS(BrandID, Model, ColorID, StorageID, Condition, Quality, TermID, CurrencyID, PurchaPrice, ProductImage)
VALUES
(1, 'DEMO', 4, 8, 1, 1, 2, 220, 100, null),
(2, 'Galaxy S21', 1, 7, 2, 3, 2, 220, 75000.0, null),
(2, 'Galaxy S21+', 4, 6, 1, 1, 2, 1, 235.0, null),
(2, 'Galaxy S21+', 4, 6, 1, 1, 1, 1, 235.0, null),
(7, 'Google Pixel 4', 3, 5, 2, 2, 1, 220.0, 35000, null);