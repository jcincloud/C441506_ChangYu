//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcCore.Business.DB0
{
    using System;
    using System.Collections.Generic;
    
    using Newtonsoft.Json;
    public partial class Product_Category_L1 : BaseEntityTable
    {
        public Product_Category_L1()
        {
            this.Product_Category_L2 = new HashSet<Product_Category_L2>();
            this.Product = new HashSet<Product>();
        }
    
        public int product_category_l1_id { get; set; }
        public string l1_name { get; set; }
        public Nullable<int> l1_sort { get; set; }
        public string memo { get; set; }
        public bool i_Hide { get; set; }
        public string i_InsertUserID { get; set; }
        public Nullable<int> i_InsertDeptID { get; set; }
        public Nullable<System.DateTime> i_InsertDateTime { get; set; }
        public string i_UpdateUserID { get; set; }
        public Nullable<int> i_UpdateDeptID { get; set; }
        public Nullable<System.DateTime> i_UpdateDateTime { get; set; }
        public string i_Lang { get; set; }
    
    	[JsonIgnore]
        public virtual ICollection<Product_Category_L2> Product_Category_L2 { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Product> Product { get; set; }
    }
}
