using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakulteti.Models
{
    public class RegjistroLenden
    {
        public string Emri { get; set; }
        public string Pershkrimi { get; set; }
        public string PerdoruesiID { get; set; }
    }
}