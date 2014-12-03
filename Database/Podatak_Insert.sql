CREATE PROCEDURE [dbo].[Podatak_Insert]
	@ime 		nvarchar(40),
	@prezime 	nvarchar(60),
	@pbr 		int,
	@grad 		nvarchar(60),
	@telefon 	nvarchar(15)
AS
BEGIN
	BEGIN TRY
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
		);
		
		RETURN @@ERROR
		--SELECT SCOPE_IDENTITY() AS Id;
	END TRY
	BEGIN CATCH
		RETURN  @@ERROR -- #9 implementirati vraćanje greške iz procedure
	END CATCH
	
END
