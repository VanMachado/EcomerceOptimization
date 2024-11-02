USE Ecommerce;
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'RoleNameEcommerce') AND type = N'U')
BEGIN    
    CREATE TABLE RoleNameEcommerce (
        Id          INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
        RoleName    VARCHAR(20) NOT NULL
    );
        
    INSERT INTO RoleNameEcommerce (RoleName) VALUES ('admin'), ('user');
END
GO
