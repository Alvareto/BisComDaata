
CREATE PROCEDURE [dbo].[Podatak_Insert]
	@ime 		nvarchar(40),
	@prezime 	nvarchar(60),
	@pbr 		int, -- this is enforced on application level (before passing data to this stored procedure)
	@grad 		nvarchar(60),
	@telefon 	nvarchar(15)
AS
BEGIN
	SET XACT_ABORT OFF -- to get statement-level rollbacks
	SET NOCOUNT ON
	DECLARE @error TABLE -- to collect all the errors
	(Error_ID INT IDENTITY(1,1) PRIMARY KEY,
	ErrorCode INT,
	Name NVARCHAR(40),
	Surname NVARCHAR(60),
	PostalCode INT,
	Phone NVARCHAR(15),
	TransactionState INT,
	ErrorMessage VARCHAR(255)
	)

	BEGIN TRANSACTION -- start a transaction
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
	END TRY
	BEGIN CATCH -- record the error
		INSERT INTO @error(ErrorCode, Name, Surname, PostalCode, Phone, TransactionState, ErrorMessage)
			SELECT ERROR_NUMBER(), @ime, @prezime, @pbr, @telefon, XACT_STATE(), ERROR_MESSAGE()
	END CATCH

	IF EXISTS (SELECT * FROM @error)
		BEGIN
			ROLLBACK TRANSACTION
			SELECT * FROM @error; -- return the error #9
		END
	ELSE
		COMMIT TRANSACTION
END