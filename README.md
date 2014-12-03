#Combis

##Analiza zahtjeva
ASP.NET MVC
 header, footer, body -> gridview, 2 buttons (Load, Save)
 
button "Load" =>
- učitava podatke iz datoteke - folder u kojem se nalazi datoteka je definiran u configu; 1 datoteka => podaci.csv
- prikaži podatke u gridviewu -> označiti podatke koji imaju pogrešan poštanski broj -> kako prepoznajem grešku?

button "Save" =>
- napraviti (if not exists) tablicu dbo.Podaci: Ime, Prezime, Poštanski br, Grad, Telefon
- napraviti proceduru za snimanje podataka za gore kreiranu tablicu
- snima podatke iz gridviewa u tablicu dbo.Podaci koristeži proceduru -> samo retci koji nemaju grešku i nisu duplikati (PK ? ) -> onemogućiti snimanje duplikata
-- implementirati vraćanje greške iz procedure
-- implementirati logiranje u sql server log (?)

- kreiraj skripte za kreiranje tablice i procedure
- postavi aplikaciju i skripte na GitHub

##Oblikovanje
###Baza podataka
Zadana tablica nije normalizirana na 3NF (pBr -> Grad).
Zadana tablica ne poštuje naming convention (upotrebljava množinu imenice).

Name | Type | Description
--- | --- | ---
Id | Integer | Primary Key, Identity
Ime| String| First name
Prezime| String | Last name
PostanskiBroj | Integer | City postal code
Grad | String | City name
Telefon | String | Contact phone number

Odlučio sam preimenovati tablicu u dbo.Podatak.
Što se tiče normalizacije, nisam je obavio (jer bi to previše odudaralo od korisničkih zahtjeva).

##Input(.csv)
problem brzine učitavanja
algoritam koji 300.000 redaka učitava i prikazuje u 0.8s:
- loads all the data up front
- places it into System.Data.DataTable in the loop
- binds the table to the grid upon completion
Prilagodio sam ga svojoj prilici (MVC, ne WebForms), zadržavajući više manje perfomanse.

##Podjela taskova
- definiraj direktorij u configu
- load data from directory + "podaci.csv" defined in config
- display loaded data in gridview
error row if invalid pBr (is not int)
- create table
- create Podatak_Insert stored procedure
-- disable duplicates
-- implement error return from procedure
-- implement loggingto sql server log
- save displayed data (from gridview) using procedure Podatak_Insert
