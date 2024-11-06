USE Ecommerce;
GO

CREATE TABLE ControleAtualizacao (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Tabela VARCHAR(50) NOT NULL,
    DataCriacao DATETIME DEFAULT GETDATE(),
    DataAtualizacao DATETIME DEFAULT GETDATE()
);

INSERT INTO ControleAtualizacao (Tabela, DataCriacao, DataAtualizacao)
VALUES 
    ('ClientsEcommerce', GETDATE(), GETDATE()),
    ('OrdersEcommerce', GETDATE(), GETDATE()),
    ('UsersEcommerce', GETDATE(), GETDATE());
