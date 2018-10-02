using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.Afatet
{
    public class AfatiLista
    {
        public string AfatiID { get; set; }
        public string Afati { get; set; }
        public string DataFillimit { get; set; }
        public string DataMbarimit { get; set; }
        public Boolean Aktiv { get; set; }
        public string NumriProvimeve { get; set; }
    }
}