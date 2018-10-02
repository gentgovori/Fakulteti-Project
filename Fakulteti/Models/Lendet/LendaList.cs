using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models
{
    public class LendaList
    {
       public int LendaID { get; set; }
       public string Emri { get; set; }
       public string Pershkrimi { get; set; }
       public int ProfesoriID { get; set; }
       public int ProfesoriLendaID { get; set; }
       public string Profesori { get; set; }
    }


}