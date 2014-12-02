using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entry.Models
{
    public class PodatakViewModel
    {
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
    }
}