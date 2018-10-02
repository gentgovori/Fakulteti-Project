using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.StudentiLendet
{
    public class RegjistroLendetStudenti
    {
        [Required]
        public int ProfesoriLendaID { get; set; }
    }
}