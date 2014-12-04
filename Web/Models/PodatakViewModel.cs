using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public Boolean Save(DatabaseEntities db, PodatakViewModel podatak)
        {
            if (podatak.PostanskiBroj == INVALID_POSTAL_CODE)
                return false;

            try
            {
                // poku�aj snimiti podatke
                db.Podatak_Insert(
                    podatak.Ime,
                    podatak.Prezime,
                    Int32.Parse(podatak.PostanskiBroj),
                    podatak.Grad,
                    podatak.Telefon);
            }
            catch (Exception e)
            {
                return false; // dogodila se gre�ka
                // TODO: ukoliko bude potrebno, mo�e� dodati neki ispis ili ne�to
            }

            return true; // sve u redu
        }

        public PodatakViewModel(String pBr)
        {
            // poku�aj parsirati po�tanski broj kao integer
            // ukoliko ne uspije, proglasi ga nevaljanim
            int _pom;
            PostanskiBroj = Int32.TryParse(pBr, out _pom) ? pBr : INVALID_POSTAL_CODE;
        }
    }
}
