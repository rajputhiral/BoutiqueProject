//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoutiqueProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubCategory
    {
        public int subcatid { get; set; }
        public Nullable<int> catid { get; set; }
        public string subcatname { get; set; }
        public string subcatpic { get; set; }
        public Nullable<int> btid { get; set; }
    }
}