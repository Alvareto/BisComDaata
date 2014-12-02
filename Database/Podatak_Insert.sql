CREATE PROCEDURE [dbo].[Podatak_Insert]
	@ime 		nvarchar(40),
	@prezime 	nvarchar(60),
	@pbr 		int,
	@grad 		nvarchar(60),
	@telefon 	nvarchar(15),
	@error		int		OUTPUT
	
AS
BEGIN
	SET NOCOUNT ON -- maybe
	SET @error = 0
	
	BEGIN TRANSACTION
	
	INSERT INTO [dbo].[Podatak]
	(
		[Ime],
		[Prezime], 
		[PostanskiBroj], 
		[Grad], 
		[Telefon]
	)
	VALUES 
	(
		@ime,
		@prezime,
		@pbr,
		@grad,
		@telefon
	)
	
	SET @error = @@ERROR
	
	IF @error <> 0
		ROLLBACK TRANSACTION
	
	COMMIT TRANSACTION
	
	RETURN @error
	
	SELECT SCOPE_IDENTITY() AS Id
END
