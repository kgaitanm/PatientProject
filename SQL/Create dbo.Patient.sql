IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PatientDatabase')
BEGIN
    CREATE DATABASE PatientDatabase;
END;

IF DB_ID('PatientDatabase') IS NOT NULL
BEGIN
    USE [PatientDatabase];
    
    /****** Object: Table [dbo].[Patient] Script Date: 27/10/2024 13:12:16 ******/
    SET ANSI_NULLS ON;
    SET QUOTED_IDENTIFIER ON;
    
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patient' AND schema_id = SCHEMA_ID('dbo'))
    BEGIN
        CREATE TABLE [dbo].[Patient] (
            [Id]                    UNIQUEIDENTIFIER NOT NULL,
            [FirstName]             NVARCHAR (50)    NOT NULL,
            [LastName]              NVARCHAR (50)    NOT NULL,
            [DateOfBirth]           DATE             NOT NULL,
            [Address]               NVARCHAR (255)   NULL,
            [Email]                 NVARCHAR (50)    NOT NULL,
            [PhoneNumber]           NVARCHAR (20)    NOT NULL,
            [SocialInsuranceNumber] VARBINARY (MAX)  NOT NULL,
            [EncryptionIV]          VARBINARY (16)   NOT NULL
        );
    END
END;
GO
