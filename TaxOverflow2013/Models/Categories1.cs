//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxOverflow2013.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Categories1
    {
        public Categories1()
        {
            this.Questions1 = new HashSet<Questions1>();
        }
    
        public int CategoryID { get; set; }
        public string Category { get; set; }
    
        public virtual ICollection<Questions1> Questions1 { get; set; }
    }
}