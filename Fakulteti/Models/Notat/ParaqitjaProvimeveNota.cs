using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.Notat
{
    public class ParaqitjaProvimeveNota
    {
        public int ParaqitjaProvimeveID { get; set; }
        public string Studenti { get; set; }
        public string Lenda { get; set; }
        public string Afati { get; set; }
        public int Viti { get; set; }
        public string Statusi { get; set; }
        public string Nota { get; set; }
        public string Profesori { get; set; }
    }
}