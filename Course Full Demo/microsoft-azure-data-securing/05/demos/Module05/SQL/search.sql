/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @EMAIL NVARCHAR(9) = 'zaalion@gmail.com'

SELECT TOP (1000) [Id]
      ,[Name]
      ,[Email]
      ,[Phone]
      ,[Address]
      ,[PictureName]
      ,[SIN_Number]
  FROM [dbo].[Contact]
  WHERE Email = @EMAIL