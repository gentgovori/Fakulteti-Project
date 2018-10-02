using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models
{
    public class StudentiLista
    {
        public string StudentiID { get; set; }
        public string Emri { get; set; }
        public string Mbiemri { get; set; }
        public string Telefoni { get; set; }
        public string Perdoruesi { get; set; }
        public string Fjalekalimi { get; set; }
        public string Datelindja { get; set; }
        public string Departamenti { get; set; }
        public string Komuna { get; set; }
        public int DepartamentiID { get; set; }
        public int KomunaID { get; set; }

    }
}