USE Ecommerce
GO

if not exists(select top 1 1 from sys.objects WHERE name = 'OrdersEcommerce')
begin

    CREATE TABLE OrdersEcommerce (
        Id              INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
        NomeProduto     VARCHAR(100) NOT NULL,
        Preco           DECIMAL(10, 2) NOT NULL,
        ClientId        INT NOT NULL,
    )  
    
    alter table OrdersEcommerce add Constraint FK_ClientsEcommerce foreign key (ClientId) references [dbo].[ClientsEcommerce](Id)

end