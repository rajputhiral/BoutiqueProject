using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BoutiqueProject.Models
{
    public class OrderTbModel
    {        
        public int Id { get; set; }
        
        // product
        public string pname { get; set; }
        public string price { get; set; }
        public string prodpic { get; set; }

        // user
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public Nullable<int> cnid { get; set; }
        public Nullable<int> stid { get; set; }
        public Nullable<int> cityid { get; set; }

        // quantity
        public string quantity { get; set; }
        public string address { get; set; }
        public string odate { get; set; }
        
        // boutique
        public int btid { get; set; }
        public string bname { get; set; }
    
    }
}