//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel11.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Evaluation
    {
        public int EvaluationID { get; set; }
        public Nullable<int> CusID { get; set; }
        public Nullable<int> IDProduct { get; set; }
        public Nullable<System.DateTime> EvaluationDate { get; set; }
        public Nullable<int> PointEvaluation { get; set; }
        public string Comment { get; set; }
        public string NameCus { get; set; }
        public string PhoneCus { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Room Room { get; set; }
    }
}
