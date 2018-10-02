using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakulteti.Models.Sallat
{
    public class RegjistroSallen
    {
        [Required]
        public string Numri { get; set; }
    }
}