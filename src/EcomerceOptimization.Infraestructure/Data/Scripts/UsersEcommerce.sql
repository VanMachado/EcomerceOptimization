USE Ecommerce;
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE name = 'UsersEcommerce')
BEGIN
    CREATE TABLE UsersEcommerce (
        Id              INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
        RoleId          INT not null,
        NomeCompleto    VARCHAR(40) NOT NULL,
        Password        VARCHAR(60) NOT NULL              
    );    

    alter table UsersEcommerce add Constraint FK_RoleId foreign key (RoleId) references dbo.RoleNameEcommerce(Id)
END