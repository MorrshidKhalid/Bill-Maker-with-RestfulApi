USE FBMDB;

Insert into BUSINESSES(BusinessName)
VALUES 
('YES Store'),
('Ok Store'),
('Oslo');


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

--INSERT INTO CURRENCIES 
--SELECT * FROM CurrenciesData;

SELECT * FROM CURRENCIES
SELECT * FROM BRANDS
SELECT * FROM COLORS
SELECT * FROM STORAGES
SELECT * FROM TERMS
SELECT * FROM PRODUCTS
SELECT * FROM BUSINESSES
SELECT * FROM BILLS
SELECT * FROM BILLS_BODY
SELECT * FROM CUSTOMERS
SELECT * FROM USERS
SELECT * FROM PEOPLE


INSERT INTO PRODUCTS(BrandID, Model, ColorID, StorageID, Condition, Quality, TermID, CurrencyID, PurchaPrice, ProductImage)
VALUES
(1, 'Iphone 13 pro', 4, 8, 1, 1, 2, 220, 600000, null),
(2, 'Galaxy S21', 1, 7, 2, 3, 2, 220, 75000.0, null),
(2, 'Galaxy S21+', 4, 6, 1, 1, 2, 1, 235.0, null),
(2, 'Galaxy S22', 4, 6, 1, 1, 1, 1, 400, null),
(7, 'Google Pixel 4', 3, 5, 2, 2, 1, 220, 35000, null);


Insert into PEOPLE(FirstName, SecondName, LastName, Email, Gendor, Address, Phone) 
values 
('Khalid', 'Mohammed', 'Ali', 'MorrshidKhalid2@gmail.com', 1, 'Al-Sawad', '+967-775265494'),
('Ali', 'Ahmed', 'Ali', 'Ali21@gmail.com', 1, 'Al-Asbahi', '+967-775332112'),
('Rawan', 'Mansor', 'Domini', 'Ro@gmail.com', 0, 'Al-Huthily', '+967-775772112'),
('Aya', 'Ali', 'Mohammed', 'Ay@gmail.com', 0, 'Hadah', '+967-780112112'),
('Kuther', 'Abdo', 'Salem', 'Keo221@gmail.com', 0, '50, Street', '+967-781773774'),
('Zide', 'Mohammed', 'Al-Refi', 'Zidch@gmail.com', 1, 'Al-Sawad', '+967-777777722'),
('Ahmed', 'Humain', 'Ahmed', 'Ahmed@gmail.com', 1, 'Al-Asbahi', '+967-774265494');


Insert into CUSTOMERS(PersonID) values
(5),
(2),
(3),
(6);


Insert into USERS(PersonID, Username, Password, IsActive, Permission) values
(1, 'Admin', '1234', 1, -1);



SELECT * FROM BILLS
SELECT * FROM BILLS_BODY
SELECT * FROM PAYMENTS


INSERT INTO BILLS
(FromBusinessID, ToCustomerID, Amount, Discount, Date, BillStatus, CurrencyRate,CurrencyID, CreatedByUserID)
VALUES 
(3, 1,
80000, 0, GETDATE(), 1, 530,
220, 1)

INSERT INTO BILLS_BODY
(BillID, ProductID, Quantity, SellingPrice, CurrencyID)
VALUES 
(1, 1, 1, 80000, 220)

SELECT * FROM BILLS;

INSERT INTO PAYMENTS
(CustomerID, BillID, CurrencyID, MethodID, CurrencyRate, AmountPaid, ReturnAmount, PaymentDate, Note)
VALUES
(1, 1, 220, 1, 530, 80000, 0, GETDATE(), 'Demo payment')