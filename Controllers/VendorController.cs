using BoutiqueProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoutiqueProject.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor
        Database1Entities obj = new Database1Entities();
        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult addcategory()
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            var q = (from m in obj.Categories where m.btid == id select m).ToList();
            return View(q);
        }
        [HttpPost]
        public ActionResult addcategory(Category c)
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            c.btid = id;

            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../category") + "/";
            f.SaveAs(rpath + f.FileName);

            c.catpic = f.FileName;

            obj.Categories.Add(c);
            obj.SaveChanges();

            var q = (from m in obj.Categories where m.btid == id select m).ToList();
            return View(q);
        }
        [HttpGet]
        public ActionResult editcategory(int id)
        {
            int id1 = Convert.ToInt32(Session["vendor_id"]);
            var q = (from m in obj.Categories where m.catid == id && m.btid == id1 select m).Single();
            return View(q);
        }
        [HttpPost]
        public ActionResult editcategory(Category c)
        {
            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../category") + "/";
            f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.Categories where m.catid == c.catid select m).Single();

            q.catnm = c.catnm;
            q.catpic = f.FileName;

            obj.SaveChanges();
            return RedirectToAction("addcategory");


        }
        public ActionResult delcat(int id)
        {
            var q = (from m in obj.Categories where m.catid == id select m).Single();
            obj.Categories.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("addcategory");
        }


        #region subcategory module
        public ActionResult addsubcat()
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == id select m).ToList();

            List<CatSubCatModel> sc = new List<CatSubCatModel>();
            var q = (from m in obj.SubCategories
                     join
                     k in obj.Categories
                     on m.catid equals k.catid
                     select new
                     {
                         k.catnm,
                         k.catid,
                         k.catpic,
                         m.subcatpic,
                         m.subcatname,
                         m.subcatid,


                     }).ToList();

            foreach (var item in q)
            {
                CatSubCatModel c = new CatSubCatModel();
                c.catnm = item.catnm;
                c.subcatname = item.subcatname;
                c.subcatid = item.subcatid;
                c.catid = item.catid;
                c.catpic = item.catpic;
                c.subcatpic = item.subcatpic;
                sc.Add(c);
            }
            return View(sc);
        }

        [HttpPost]
        public ActionResult addsubcat(SubCategory s)
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            s.btid = id;

            // for category binding
            ViewBag.cat = (from m in obj.Categories select m).ToList();

            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../subcategory") + "/";
            f.SaveAs(rpath + f.FileName);

            // now insert into db

            s.subcatpic = f.FileName;

            obj.SubCategories.Add(s);
            obj.SaveChanges();

            List<CatSubCatModel> sc = new List<CatSubCatModel>();
            var q = (from m in obj.SubCategories
                     join
                     k in obj.Categories
                     on m.catid equals k.catid
                     where m.btid == id
                     select new
                     {
                         k.catnm,
                         k.catid,
                         k.catpic,
                         m.subcatpic,
                         m.subcatname,
                         m.subcatid,


                     }).ToList();

            foreach (var item in q)
            {
                CatSubCatModel c = new CatSubCatModel();
                c.catnm = item.catnm;
                c.subcatname = item.subcatname;
                c.subcatid = item.subcatid;
                c.catid = item.catid;
                c.catpic = item.catpic;
                c.subcatpic = item.subcatpic;
                sc.Add(c);
            }
            return View(sc);
        }

        public ActionResult editsubcat(int id)
        {
            int id1 = Convert.ToInt32(Session["vendor_id"]);
            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == id1 select m).ToList();

            var q = (from m in obj.SubCategories where m.subcatid == id select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult editsubcat(SubCategory c)
        {


            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../../subcategory") + "/";
            f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.SubCategories where m.catid == c.catid select m).Single();

            q.subcatname = c.subcatname;
            q.subcatpic = f.FileName;
            q.catid = c.catid;

            obj.SaveChanges();
            return RedirectToAction("addsubcat");
        }


        public ActionResult delsubcat(int id)
        {
            var q = (from m in obj.SubCategories where m.subcatid == id select m).Single();
            obj.SubCategories.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("addsubcat");
        }
        #endregion

        #region Product module
        public ActionResult AddProduct()
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == id select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories where m.btid == id select m).ToList();

            var q = (from m in obj.Products where m.btid == id select m).ToList();

            return View(q);
        }

        [HttpPost]
        public ActionResult AddProduct(Product p)
        {
            int id = Convert.ToInt32(Session["vendor_id"]);
            p.btid = id;

            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == id select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories where m.btid == id select m).ToList();

            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../Product") + "/";
            f.SaveAs(rpath + f.FileName);

            // now insert into db

            p.prodpic = f.FileName;

            obj.Products.Add(p);
            obj.SaveChanges();

            var q = (from m in obj.Products select m).ToList();
            return View(q);
        }

        public ActionResult EditProduct(int id1)
        {
            int id = Convert.ToInt32(Session["vendor_id"]);

            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == id select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories where m.btid == id select m).ToList();

            var q = (from m in obj.Products where m.pid == id1 select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult EditProduct(Product p)
        {
            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../../product") + "/";
            f.SaveAs(rpath + f.FileName);
            var q = (from m in obj.Products where m.pid == p.pid select m).Single();

            q.pname = p.pname;
            q.prodpic = f.FileName;
            q.price = p.price;
            q.pdescription = p.pdescription;

            obj.SaveChanges();

            return RedirectToAction("AddProduct");

        }


        public ActionResult DeleteProduct(int id)
        {

            var q = (from m in obj.Products where m.pid == id select m).Single();
            obj.Products.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("AddProduct");

        }
        #endregion

        public ActionResult showorder()
        {
            int id = Convert.ToInt32(Session["vendor_id"]);

            List<OrderTbModel> lst = new List<OrderTbModel>();

            var q = (from m in obj.ordertbs
                     join k in obj.Products
                     on m.pid equals k.pid
                     join l in obj.clientds
                     on m.uid equals l.Id
                     join b in obj.VendorTbs
            on m.btid equals b.Id
                     where k.btid == id
                     select new
                     {
                         m.quantity,
                         m.address,
                         m.odate,
                         k.pname,
                         k.price,
                         k.prodpic,
                         l.contact,
                         l.fname,
                         l.lname,
                         l.email,
                         b.bname

                     });

            foreach (var item in q)
            {
                OrderTbModel o = new OrderTbModel();
                o.quantity = item.quantity.ToString();
                o.address = item.address;
                o.odate = item.odate;
                o.pname = item.pname;
                o.price = item.price;
                o.prodpic = item.prodpic;
                o.contact = item.contact;
                o.fname = item.fname;
                o.lname = item.lname;
                o.email = item.email;
                o.bname = item.bname;

                lst.Add(o);

            }
            return View(lst);
            return View();
        }
    }
}