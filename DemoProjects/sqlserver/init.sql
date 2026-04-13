IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TestDb')
BEGIN
    CREATE DATABASE TestDb COLLATE Chinese_Taiwan_Stroke_CI_AS;
END
GO

USE TestDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserCredit')
BEGIN
    CREATE TABLE UserCredit (
        Id          INT IDENTITY(1,1) NOT NULL,
        HostId      INT NOT NULL,
        UserId      INT NOT NULL,
        SmsName     NVARCHAR(20) NULL,
        SmsPassword NVARCHAR(50) NULL,
        Amount      FLOAT NOT NULL DEFAULT 0,
        Deleted     BIT NOT NULL DEFAULT 0,
        CreateTime  DATETIME NOT NULL,
        CreateUser  INT NOT NULL,
        ModifyTime  DATETIME NULL,
        ModifyUser  INT NULL,
        CONSTRAINT PK_dbo_UserCredit PRIMARY KEY (Id)
    );

    CREATE INDEX IX_UserId ON UserCredit (UserId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserTopUp')
BEGIN
    CREATE TABLE UserTopUp (
        Id              INT IDENTITY(1,1) NOT NULL,
        HostId          INT NOT NULL,
        UserId          INT NOT NULL,
        Product         NVARCHAR(100) NULL,
        Type            INT NOT NULL DEFAULT 0,
        Amount          FLOAT NOT NULL DEFAULT 0,
        Quantity        INT NOT NULL DEFAULT 0,
        Note            NVARCHAR(500) NULL,
        PayType         NVARCHAR(50) NULL,
        TradeNo         NVARCHAR(20) NULL,
        Status          INT NOT NULL DEFAULT 0,
        Message         NVARCHAR(1000) NULL,
        TPSTradeNo      NVARCHAR(50) NULL,
        InvoiceNumber   NVARCHAR(20) NULL,
        InvoiceNote     NVARCHAR(200) NULL,
        CarruerNum      NVARCHAR(20) NULL,
        CarruerType     NVARCHAR(10) NULL,
        CompAddr        NVARCHAR(200) NULL,
        CompIdNumber    NVARCHAR(20) NULL,
        CompName        NVARCHAR(100) NULL,
        Email           NVARCHAR(100) NULL,
        InvoiceType     NVARCHAR(20) NULL,
        LoveCode        NVARCHAR(30) NULL,
        Phone           NVARCHAR(20) NULL,
        UserIdNumber    NVARCHAR(20) NULL,
        SendParams      NVARCHAR(MAX) NULL,
        ResponseParams  NVARCHAR(MAX) NULL,
        Deleted         BIT NOT NULL DEFAULT 0,
        CreateTime      DATETIME NOT NULL,
        CreateUser      INT NOT NULL,
        ModifyTime      DATETIME NULL,
        ModifyUser      INT NULL,
        CONSTRAINT PK_dbo_UserTopUp PRIMARY KEY (Id)
    );

    CREATE INDEX IX_UserTopUp_UserId ON UserTopUp (UserId);
END
GO

-- 預設第一筆 UserCredit
IF NOT EXISTS (SELECT * FROM UserCredit WHERE UserId = 1)
BEGIN
    INSERT INTO UserCredit (HostId, UserId, SmsName, SmsPassword, Amount, Deleted, CreateTime, CreateUser)
    VALUES (1, 1, N'測試使用者', '1234', 0, 0, GETDATE(), 1);
END
GO
