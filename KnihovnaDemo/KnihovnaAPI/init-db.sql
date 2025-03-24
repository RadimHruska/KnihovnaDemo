-- Vytvoření databáze KnihovnaDb, pokud neexistuje
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'KnihovnaDb')
BEGIN
    CREATE DATABASE [KnihovnaDb]
END
GO

USE [KnihovnaDb]
GO

-- Vytvoření tabulky Users, pokud neexistuje
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [IsAdmin] BIT NOT NULL DEFAULT(0),
        [Password] NVARCHAR(100) NOT NULL
    )
    
    -- Vložení výchozích uživatelů
    INSERT INTO [dbo].[Users] ([Name], [IsAdmin], [Password]) VALUES
    ('Admin', 1, 'admin'),
    ('Radim', 1, 'heslo'),
    ('Uživatel', 0, 'heslo')
END
GO

-- Vytvoření tabulky Books, pokud neexistuje
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Books' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Books] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Author] NVARCHAR(200) NOT NULL,
        [InStock] INT NOT NULL DEFAULT(1)
    )
    
    -- Vložení výchozích knih
    INSERT INTO [dbo].[Books] ([Name], [Author], [InStock]) VALUES
    ('Pán prstenů', 'J.R.R. Tolkien', 3),
    ('Harry Potter', 'J.K. Rowling', 5),
    ('1984', 'George Orwell', 2)
END
GO

-- Vytvoření tabulky Lends, pokud neexistuje
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Lends' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Lends] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [LandedDate] DATETIME NOT NULL,
        [ReturnedDate] DATETIME NULL,
        [IdUser] INT NOT NULL,
        [IdBook] INT NOT NULL,
        CONSTRAINT [FK_Lends_Users] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[Users] ([Id]),
        CONSTRAINT [FK_Lends_Books] FOREIGN KEY ([IdBook]) REFERENCES [dbo].[Books] ([Id])
    )
    
    -- Vložení ukázkových výpůjček
    INSERT INTO [dbo].[Lends] ([LandedDate], [ReturnedDate], [IdUser], [IdBook]) VALUES
    (DATEADD(day, -10, GETDATE()), NULL, 2, 1),
    (DATEADD(day, -15, GETDATE()), DATEADD(day, -5, GETDATE()), 3, 2)
END
GO 