//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_repos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Repos
    {
        public int Id_repos { get; set; }
        public int Id { get; set; }
        public int num_sem { get; set; }
        public string jour { get; set; }
        public int shift { get; set; }
        public Nullable<int> congé { get; set; }
        public string Type_congé { get; set; }
    }
}
