using System;

namespace Web.Models
{
    /// <summary>
    /// Same as Podatak model, but holds PostalCode as string instead of integer
    /// for easier interaction with view.
    /// </summary>
    public class PodatakViewModel
    {
        // used when saving data to database to skid entries with invalid postal code
        // TODO: can this be done better?
        const string INVALID_POSTAL_CODE = "INVALID";

        //private Podatak _podatak; -- we don't need it
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String PostanskiBroj { get; set; }
        public String Grad { get; set; }
        public String Telefon { get; set; }

        /// <summary>
        /// True if object is valid.
        /// </summary>
        public Boolean isValid
        {
            // u novoj verziji C#a ovo puno bolje izgleda :P
            get
            {
                return PostanskiBroj != INVALID_POSTAL_CODE;
            }
        }

        /// <summary>
        /// Tries to save data object using stored procedure, 
        /// and returns value based on operation success.
        /// </summary>
        /// <param name="db">database</param>
        /// <param name="podatak">data object</param>
        /// <returns>Boolean</returns>
        public Boolean Save(DatabaseEntities db, PodatakViewModel podatak)
        {
            if (!podatak.isValid)
                return false;

            try
            {
                // pokušaj snimiti podatke
                db.Podatak_Insert(
                    podatak.Ime,
                    podatak.Prezime,
                    Int32.Parse(podatak.PostanskiBroj),
                    podatak.Grad,
                    podatak.Telefon);
            }
            catch (Exception e)
            {
                return false; // dogodila se greška
                // TODO: ukoliko bude potrebno, možeš dodati neki ispis ili nešto
            }

            return true; // sve u redu
        }

        public PodatakViewModel(String[] splitLine)
        {
            this.Ime = splitLine[0];
            this.Prezime = splitLine[1];
            // pokušaj parsirati poštanski broj kao integer
            // ukoliko ne uspije, proglasi ga nevaljanim
            int _pom;
            this.PostanskiBroj = Int32.TryParse(splitLine[2], out _pom) ? splitLine[2] : INVALID_POSTAL_CODE;
            this.Grad = splitLine[3];
            this.Telefon = splitLine[4];
        }
    }
}
