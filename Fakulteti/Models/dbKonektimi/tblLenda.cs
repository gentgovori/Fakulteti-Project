//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fakulteti.Models.dbKonektimi
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblLenda
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblLenda()
        {
            this.tblLendaKlasa = new HashSet<tblLendaKlasa>();
            this.tblProfesoriLenda = new HashSet<tblProfesoriLenda>();
        }
    
        public int LendaID { get; set; }
        public string Emri { get; set; }
        public string Pershkrimi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLendaKlasa> tblLendaKlasa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProfesoriLenda> tblProfesoriLenda { get; set; }
    }
}
