using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoutiqueProject.Models;

namespace BoutiqueProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Database1Entities obj = new Database1Entities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdminDetail a)
        {
            var q = (from m in obj.AdminDetails where m.email == a.email && m.password == a.password select m).SingleOrDefault();
            if (q != null)
            {
                Session["user"] = "admin";
                return RedirectToAction("Inner");
            }
            else
            {
                ViewBag.msg = "Invalid User or Password";
            }
            return View();
        }

        public ActionResult Inner()
        {

            return View();
        }


        #region category module
        public ActionResult addcat()
        {
            var q = (from m in obj.Categories select m).ToList();

            return View(q);
        }

        [HttpPost]
        public ActionResult addcat(Category c)
        {
            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../category") + "/";
            f.SaveAs(rpath + f.FileName);

            // now insert into db

            c.catpic = f.FileName;

            obj.Categories.Add(c);
            obj.SaveChanges();

            var q = (from m in obj.Categories select m).ToList();
            return View(q);
        }

        public ActionResult editcat(int id)
        {
            var q = (from m in obj.Categories where m.catid == id select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult editcat(Category c)
        {

            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../../category") + "/";
            f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.Categories where m.catid == c.catid select m).Single();

            q.catnm = c.catnm;
            q.catpic = f.FileName;

            obj.SaveChanges();
            return RedirectToAction("addcat");
        }


        public ActionResult delcat(int id)
        {
            var q = (from m in obj.Categories where m.catid == id select m).Single();
            obj.Categories.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("addcat");
        }
        #endregion

        #region subcategory module
        public ActionResult addsubcat()
        {
            // for category binding
            ViewBag.cat = (from m in obj.Categories  select m).ToList();

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
            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.catid == id select m).Single();

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
            // for category binding
            ViewBag.cat = (from m in obj.Categories select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories  select m).ToList();

            var q = (from m in obj.Products select m).ToList();

            return View(q);
        }

        [HttpPost]
        public ActionResult AddProduct(Product p)
        {

            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == 0 select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories where m.btid == 0 select m).ToList();

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

        public ActionResult EditProduct(int id)
        {
            // for category binding
            ViewBag.cat = (from m in obj.Categories where m.btid == 0 select m).ToList();

            // for subcategory binding
            ViewBag.subcat = (from m in obj.SubCategories where m.btid == 0 select m).ToList();

            var q = (from m in obj.Products where m.pid == id select m).Single();
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

        #region Country module
        public ActionResult AddCountry()
        {
            // for category binding
            ViewBag.country = (from m in obj.Countries select m).ToList();

            //// for subcategory binding
            //ViewBag.subcat = (from m in obj.SubCategories select m).ToList();

            var q = (from m in obj.Countries select m).ToList();

            return View(q);
        }

        [HttpPost]
        public ActionResult AddCountry(Country c)
        {

            // for category binding
            ViewBag.country = (from m in obj.Countries select m).ToList();

            //// for subcategory binding
            //ViewBag.subcat = (from m in obj.SubCategories select m).ToList();

            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../Product") + "/";
            //f.SaveAs(rpath + f.FileName);

            // now insert into db

            //p.prodpic = f.FileName;

            obj.Countries.Add(c);
            obj.SaveChanges();

            var q = (from m in obj.Countries select m).ToList();
            return View(q);
        }

        public ActionResult EditCountry(int id)
        {
            // for category binding
            ViewBag.cat = (from m in obj.Countries select m).ToList();

            //// for subcategory binding
            //ViewBag.subcat = (from m in obj.SubCategories select m).ToList();

            var q = (from m in obj.Countries where m.cid == id select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult EditCountry(Country c)
        {
            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../../product") + "/";
            //f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.Countries where m.cid == c.cid select m).Single();


            q.cntrynm = c.cntrynm;


            obj.SaveChanges();

            return RedirectToAction("AddCountry");

        }


        public ActionResult DeleteCountry(int id)
        {

            var q = (from m in obj.Countries where m.cid == id select m).Single();
            obj.Countries.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("AddCountry");

        }
        #endregion

        #region State module
        public ActionResult AddState()
        {
            ViewBag.state = (from m in obj.Countries select m).ToList();
            List<CountryStateModel> cs = new List<CountryStateModel>();

            var q = (from m in obj.Countries
                     join k in obj.States
                     on m.cid equals k.cid
                     select new
                     {

                         k.stid,
                         k.stname,
                         m.cntrynm,
                         m.cid
                     }
                     ).ToList();
            foreach (var item in q)
            {
                CountryStateModel c = new CountryStateModel();
                c.stid = item.stid;
                c.stname = item.stname;
                c.cntrynm = item.cntrynm;
                cs.Add(c);

            }

            return View(cs);
        }

        [HttpPost]
        public ActionResult AddState(State s)
        {
            ViewBag.state = (from m in obj.Countries select m).ToList();

            obj.States.Add(s);
            obj.SaveChanges();



            List<CountryStateModel> sc = new List<CountryStateModel>();
            var q = (from m in obj.Countries
                     join
                     k in obj.States
                     on m.cid equals k.cid
                     select new
                     {
                         m.cid,
                         m.cntrynm,
                         k.stid,
                         k.stname

                     }).ToList();

            foreach (var item in q)
            {
                CountryStateModel c = new CountryStateModel();
                c.cid = item.cid;
                c.cntrynm = item.cntrynm;
                c.stid = item.stid;
                c.stname = item.stname;
                sc.Add(c);
            }
            return View(sc);
            //var q = (from m in obj.States select m).ToList();
            //return View(q);
        }

        public ActionResult EditState(int id)
        {
            var q = (from m in obj.States where m.stid == id select m).Single();
            return View(q);
        }



        [HttpPost]
        public ActionResult EditState(State s)
        {


            var q = (from m in obj.States where m.stid == s.stid select m).Single();

            q.stname = s.stname;
            q.cid = s.cid;

            obj.SaveChanges();
            return RedirectToAction("AddState");
        }


        public ActionResult DeleteState(int id)
        {
            var q = (from m in obj.States where m.stid == id select m).Single();
            obj.States.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("AddState");
        }
        #endregion


        #region City module
        public ActionResult AddCity()
        {
            // for Country binding
            ViewBag.Country = (from m in obj.Countries select m).ToList();

            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();

            List<csca> ca = new List<csca>();


            var q = (from m in obj.Cities
                     join
                     k in obj.States
                     on m.cid equals k.cid
                     join
                     c1 in obj.Countries on
                     k.cid equals c1.cid
                     select new
                     {
                         k.stid,
                         k.stname,
                         m.cid,
                         m.cityid,
                         m.citynm,
                         c1.cntrynm,

                     }
                     ).ToList();
            foreach (var item in q)
            {
                csca c = new csca();
                c.stid = item.stid;
                c.stname = item.stname;
                c.cityid = item.cityid;
                c.citynm = item.citynm;
                c.cntryname = item.cntrynm;

                ca.Add(c);
            }

            return View(ca);
        }

        [HttpPost]
        public ActionResult AddCity(City c)
        {

            // for Country binding
            ViewBag.Country = (from m in obj.Countries select m).ToList();

            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();


            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../Product") + "/";
            //f.SaveAs(rpath + f.FileName);

            // now insert into db

            //p.prodpic = f.FileName;

            obj.Cities.Add(c);
            obj.SaveChanges();

            List<csca> ca = new List<csca>();

            var q = (from m in obj.Cities
                     join
                     k in obj.States
                     on m.cid equals k.cid
                     join
                   c1 in obj.Countries on
                   k.cid equals c1.cid
                     select new
                     {
                         m.cityid,
                         m.cntryid,
                         m.citynm,
                         k.stid,
                         k.stname,
                         c1.cntrynm

                     }
                     ).ToList();

            foreach (var item in q)
            {
                csca s = new csca();
                s.cityid = item.cityid;
                s.citynm = item.citynm;
                s.stid = item.stid;
                s.stname = item.stname;
                s.cntryname = item.cntrynm;


                ca.Add(s);

            }
            return View(ca);
        }

        public ActionResult EditCity(int id)
        {
            // for country binding
            ViewBag.country = (from m in obj.Countries select m).ToList();

            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();

            var q = (from m in obj.Cities where m.cityid == id select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult EditCity(City c)
        {
            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../../product") + "/";
            //f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.Cities where m.cityid == c.cityid select m).Single();


            q.citynm = c.citynm;
            //q.pname = p.pname;
            //q.prodpic = f.FileName;
            //q.price = p.price;
            //q.pdescription = p.pdescription;

            obj.SaveChanges();

            return RedirectToAction("AddCity");

        }

        public ActionResult DeleteCity(int id)
        {

            var q = (from m in obj.Cities where m.cityid == id select m).Single();
            obj.Cities.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("AddCity");

        }
        #endregion

        #region Area module
        public ActionResult AddArea()
        {
            // for Coutry binding
            ViewBag.Country = (from m in obj.Countries select m).ToList();
            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();

            // for city binding
            ViewBag.city = (from m in obj.Cities select m).ToList();



            List<csca> ca = new List<csca>();

            var q = (from m in obj.Countries
                     join k in obj.States on
                     m.cid equals k.cid
                     join k1 in obj.Cities on
                     k.stid equals k1.stid
                     join k2 in obj.Areas on
                     k1.cityid equals k2.cityid
                     select new
                     {
                         m.cntrynm,
                         k.stname,
                         k1.citynm,
                         k2.areanm,
                         k2.areaid
                     }).ToList();

            foreach (var item in q)
            {
                csca cs = new csca();
                cs.stname = item.stname;
                cs.citynm = item.citynm;
                cs.areanm = item.areanm;
                cs.cntryname = item.cntrynm;
                cs.areaid = item.areaid;
                ca.Add(cs);
            }
            return View(ca);
        }

        [HttpPost]
        public ActionResult AddArea(Area a)
        {

            // for Country binding
            ViewBag.country = (from m in obj.Countries select m).ToList();

            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();
            // for city binding
            ViewBag.city = (from m in obj.Cities select m).ToList();

            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../Product") + "/";
            //f.SaveAs(rpath + f.FileName);

            //// now insert into db

            //p.prodpic = f.FileName;

            obj.Areas.Add(a);
            obj.SaveChanges();

            List<csca> ca = new List<csca>();

            var q = (from m in obj.Countries
                     join k in obj.States on
                     m.cid equals k.cid
                     join k1 in obj.Cities on
                     k.stid equals k1.stid
                     join k2 in obj.Areas on
                     k1.cityid equals k2.cityid
                     select new
                     {
                         m.cntrynm,
                         k.stname,
                         k1.citynm,
                         k2.areanm,
                         k2.areaid
                     }).ToList();

            foreach (var item in q)
            {
                csca cs = new csca();
                cs.stname = item.stname;
                cs.citynm = item.citynm;
                cs.areanm = item.areanm;
                cs.cntryname = item.cntrynm;
                cs.areaid = item.areaid;
                ca.Add(cs);
            }



            return View(ca);
        }

        public ActionResult EditArea(int id)
        {
            // for country binding
            ViewBag.country = (from m in obj.Countries select m).ToList();

            // for state binding
            ViewBag.state = (from m in obj.States select m).ToList();
            // for city binding
            ViewBag.city = (from m in obj.Cities select m).ToList();

            var q = (from m in obj.Areas where m.areaid == id select m).Single();
            return View(q);
        }

        [HttpPost]
        public ActionResult EditArea(Area a)
        {
            //HttpPostedFileBase f = Request.Files[0];
            //string rpath = Server.MapPath("../../product") + "/";
            //f.SaveAs(rpath + f.FileName);

            var q = (from m in obj.Areas where m.areaid == a.areaid select m).Single();

            q.areaid = a.areaid;
            //q.pname = p.pname;
            //q.prodpic = f.FileName;
            //q.price = p.price;
            //q.pdescription = p.pdescription;

            obj.SaveChanges();

            return RedirectToAction("AddArea");

        }


        public ActionResult DeleteArea(int id)
        {

            var q = (from m in obj.Areas where m.areaid == id select m).Single();
            obj.Areas.Remove(q);
            obj.SaveChanges();
            return RedirectToAction("AddArea");

        }
        #endregion

    }
}