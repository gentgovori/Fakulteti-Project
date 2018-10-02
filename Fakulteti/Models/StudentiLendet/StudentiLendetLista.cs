using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.StudentiLendet
{
    public class StudentiLendetLista
    {
        public int StudentiLendaID { get; set; }
        public int ProfesoriLendaID { get; set; }
        public string Lenda { get; set; }
        public string  Profesori { get; set; }

    }
}