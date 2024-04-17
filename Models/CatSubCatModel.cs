using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoutiqueProject.Models
{
    public class CatSubCatModel
    {
        public int catid { get; set; }
        public string catnm { get; set; }
        public string catpic { get; set; }
        public Nullable<int> btid { get; set; }

        public int subcatid { get; set; }
        public string subcatname { get; set; }
        public string subcatpic { get; set; }
       
    }
}