using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakulteti.Models
{
    public class RegjistroProfesorin
    {
        public string Emri { get; set; }
        public string Mbiemri { get; set; }
        public string Datelindja { get; set; }
        public int KomunaID { get; set; }
        public int DepartamentiID { get; set; }
        public string Perdoruesi { get; set; }
        public string Fjalekalimi { get; set; }
        public string Telefoni { get; set; }
    }
}