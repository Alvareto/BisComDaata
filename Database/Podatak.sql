-- Due to naming conventions, I renamed table from Podaci to Podatak.
CREATE TABLE [dbo].[Podatak]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Ime] NVARCHAR(40) NOT NULL,
	[Prezime] NVARCHAR(60) NOT NULL,
	[PostanskiBroj] INTEGER NOT NULL,
	[Grad] NVARCHAR(60) NOT NULL,
	[Telefon] NVARCHAR(15) NOT NULL
)
