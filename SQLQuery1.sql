CREATE PROC [dbo].[DeleteStockByDressId] @dressId INT
AS
BEGIN
DELETE FROM Stocks
WHERE DressId = @dressId
RETURN
END
GO

CREATE PROC [dbo].[spInsertDress] @name NVARCHAR(50),
						 @FirstIntroduceOn DATETIME,
						 @OnSale BIT,
						 @Picture NVARCHAR(MAX),
						 @DressModelId INT,
						 @BrandId INT
AS
BEGIN
	INSERT INTO Dresses(Name,FirstIntroduceOn,OnSale,Picture,DressModelId,BrandId)
	VALUES(@name,@FirstIntroduceOn,@OnSale,@Picture,@DressModelId,@BrandId)
	SELECT SCOPE_IDENTITY() AS BrandId
	RETURN
END
GO

CREATE PROC [dbo].[spInsertStock] @Size INT,
						  @Price DECIMAL(18,0),
						  @Quantity INT,
						  @DressId INT
AS
BEGIN
INSERT INTO [dbo].[Stocks](Size
		,Price
		,Quantity
		,DressId)
	VALUES(@Size,@Price,@Quantity,@DressId)
	SELECT SCOPE_IDENTITY() AS StockId
	RETURN
END
GO

