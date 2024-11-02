USE Ecommerce;
GO

if not exists(select top 1 1 from sys.objects WHERE name = 'ClientsEcommerce')
begin

    CREATE TABLE ClientsEcommerce (
        Id              INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
        NomeCompleto    VARCHAR(40) NOT NULL,
        Email           VARCHAR(60) NOT NULL              
    );    

end