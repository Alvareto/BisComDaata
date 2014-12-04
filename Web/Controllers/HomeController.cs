using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using Web.Models;
using System.Threading.Tasks;

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
            if (TempData.ContainsKey("error"))
                ViewBag.ErrorMessage = TempData["error"];
            return View(this.PodaciSession);
        }

        // POST: Home/Load
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Load()
        {
            this.PodaciSession = await LoadCsvDataFrom(
                filePath: Path.Combine(PodaciDirectory, "podaci.csv"));

            return RedirectToAction("Index");
        }

        // POST: Home/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save()
        {
            if (PodaciSession.Any())
            { // ako postoji bar jedan podatak
                foreach (var podatak in PodaciSession)
                {
                    if (!podatak.Save(db, podatak))
                        TempData["error"] = "One or more records can not be saved because they are duplicates or contain invalid data.";
                }

                // Očisti listu podataka
                PodaciSession = null;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Loads data from csv file into List of PodatakViewModels to show in grid.
        /// </summary>
        /// <param name="filePath">Csv file path (directory + "podaci.csv")</param>
        /// <returns>List</returns>
        private async Task<List<PodatakViewModel>> LoadCsvDataFrom(string filePath)
        {
            List<PodatakViewModel> pom = new List<PodatakViewModel>();
            if (System.IO.File.Exists(filePath))
            {
                using (System.IO.StreamReader objReader = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8))
                {
                    var contents = await objReader.ReadToEndAsync();

                    using (System.IO.StringReader strReader = new System.IO.StringReader(contents))
                    {
                        do
                        {
                            var textLine = await strReader.ReadLineAsync();

                            if (textLine != string.Empty)
                            {
                                pom.Add(new PodatakViewModel(
                                    textLine.Split(';')
                                ));
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
