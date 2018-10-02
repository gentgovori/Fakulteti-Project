using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.ParaqitjaProvimeveModel
{
    public class ListaProvimeve
    {
        public int ParaqitjaProvimeveID { get; set; }
        public string Lenda { get; set; } 
        public string Profesori { get; set; }
        public string Afati { get; set; }
        public int Viti { get; set; }
    }
}