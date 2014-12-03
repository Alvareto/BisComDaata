CREATE TABLE [dbo].[Podatak] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Ime]           NVARCHAR (40) NOT NULL,
    [Prezime]       NVARCHAR (60) NOT NULL,
    [PostanskiBroj] INT           NOT NULL,
    [Grad]          NVARCHAR (60) NOT NULL,
    [Telefon]       NVARCHAR (15) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT uc_person UNIQUE(Ime, Prezime, Telefon)
);
