//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolleySportStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductAttribute
    {
        public int AttributeId { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public Nullable<int> ProductId { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
