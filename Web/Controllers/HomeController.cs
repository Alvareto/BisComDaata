using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.Web.Configuration;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        /// <summary>
        /// Retrieve the directory of the file from the app settings in Web.config file.
        /// </summary>
        private String PodaciDirectory
        {
            get
            {
                return WebConfigurationManager.AppSettings["CsvFileDirectory"];
            }
        }

        /// <summary>
        /// Gets, or sets, list of Podatak in session.
        /// </summary>
        private IEnumerable<PodatakViewModel> PodaciSession
        {
            get
            {
                return (List<PodatakViewModel>)Session["podaci"] ?? new List<PodatakViewModel>();
            }
            set
            {
                Session["podaci"] = value;
            }
        }

        // GET: Home
        public ActionResult Index()
        {
            return View(this.PodaciSession);
        }

        // POST: Home/Load
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Load()
        {
            this.PodaciSession = LoadCsvDataFrom(
                filePath: Path.Combine(PodaciDirectory, "podaci.csv"));

            return RedirectToAction("Index");
        }

        // POST: Home/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save()
        {
            if (PodaciSession.Any())
            {
                foreach (var podatak in PodaciSession)
                {
                    if ((int)
                    db.Podatak_Insert(
                        podatak.Ime,
                        podatak.Prezime,
                        podatak.PostanskiBroj,
                        podatak.Grad,
                        podatak.Telefon
                    ) != 0)
                    {
                        // Dogodila se greška
                    }
                }
                // Očisti listu
                PodaciSession.ToList().Clear();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Loads data from csv file into List of PodatakViewModels to show in grid.
        /// </summary>
        /// <param name="filePath">Csv file path (directory + "podaci.csv")</param>
        /// <returns>List</returns>
        private List<PodatakViewModel> LoadCsvDataFrom(string filePath)
        {
            List<PodatakViewModel> pom = new List<PodatakViewModel>();
            if (System.IO.File.Exists(filePath))
            {
                using (System.IO.StreamReader objReader = new System.IO.StreamReader(filePath))
                {
                    var contents = objReader.ReadToEnd();

                    using (System.IO.StringReader strReader = new System.IO.StringReader(contents))
                    {
                        do
                        {
                            var textLine = strReader.ReadLine();

                            if (textLine != string.Empty)
                            {
                                var splitLine = textLine.Split(';');

                                pom.Add(new PodatakViewModel(pBr: splitLine[2])
                                {
                                    Ime = splitLine[0],
                                    Prezime = splitLine[1],
                                    //
                                    Grad = splitLine[3],
                                    Telefon = splitLine[4]
                                });
                            }
                        } while (strReader.Peek() != -1);
                    }
                }
            }

            return pom;
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only
        /// unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
