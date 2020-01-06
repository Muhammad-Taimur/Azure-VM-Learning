USE [MyAddressBookPlus]
GO

ALTER TABLE [dbo].[Contact]
ADD [SIN_Number] NVARCHAR(9) NULL -- columns with default constraints are not supported