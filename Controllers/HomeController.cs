using BoutiqueProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoutiqueProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Database1Entities obj = new Database1Entities();
        // GET: Home
        public ActionResult Index()
        {
            var q = (from m in obj.VendorTbs select m).ToList();
            return View(q);
        }

        public ActionResult SignIn()
        {


            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            var q = (from m in obj.VendorTbs where m.email.Equals(email) && m.password.Equals(password) select m).SingleOrDefault();
            if (q != null)
            {
                Session["vendor_id"] = q.Id;
                Session["vendor_name"] = q.bname;
                return RedirectToAction("Index", "Vendor");
            }
            else
            {
                var q1 = (from m in obj.clientds where m.email.Equals(email) && m.password.Equals(password) select m).SingleOrDefault();
                if (q1 != null)
                {
                    Session["user_id"] = q1.Id;
                    Session["name"] = q1.fname;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Invalid User or Password";
                }

            }
            return View();
        }



        public ActionResult ShowSub(int id)
        {
            int id1 = Convert.ToInt32(Session["v_id"]);
            var q = (from m in obj.SubCategories where m.catid == id && m.btid == id1 select m).ToList();
            return View(q);
        }

        public ActionResult ShowProd(int id)
        {
            int id1 = Convert.ToInt32(Session["v_id"]);
            var q = (from m in obj.Products where m.subcatid == id && m.btid == id1 select m).ToList();
            return View(q);
        }

        public ActionResult ViewProd(int id)
        {
            int id1 = Convert.ToInt32(Session["v_id"]);
            var q = (from m in obj.Products where m.pid == id && m.btid == id1 select m).Single();
            return View(q);
        }
        [HttpPost]
        public ActionResult ViewProd(Product P)
        {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("buynow", P);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult buynow(Product p)
        {

            return View();
        }
        [HttpPost]
        public ActionResult buynow(Product p, int quantity, string address)
        {

            ordertb o = new ordertb();
            o.pid = p.pid;
            o.btid = Convert.ToInt32(Session["v_id"]);
            o.uid = Convert.ToInt32(Session["user_id"]);
            o.quantity = quantity;
            o.odate = System.DateTime.Now.ToShortDateString();
            o.address = address;

            obj.ordertbs.Add(o);
            obj.SaveChanges();

            return RedirectToAction("success");
        }

        public ActionResult Logout()
        {
            Session["user_id"] = null;

            return RedirectToAction("Index");
        }

        public ActionResult success()
        {

            return View();
        }

        public ActionResult uorder()
        {
            //int id = Convert.ToInt32(Session["v_id"]);
            int uid = Convert.ToInt32(Session["user_id"]);

            List<OrderTbModel> lst = new List<OrderTbModel>();

            var q = (from m in obj.ordertbs
                     join k in obj.Products
                     on m.pid equals k.pid
                     join l in obj.clientds
                     on m.uid equals l.Id
                     join b in obj.VendorTbs
                     on m.btid equals b.Id
                     where m.uid == uid
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
        }


        public ActionResult SignUp()
        {
            ViewBag.country = (from m in obj.Countries select m).ToList();
            ViewBag.state = (from m in obj.States select m).ToList();
            ViewBag.city = (from m in obj.Cities select m).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(clientd c)
        {
            ViewBag.country = (from m in obj.Countries select m).ToList();
            ViewBag.state = (from m in obj.States select m).ToList();
            ViewBag.city = (from m in obj.Cities select m).ToList();

            obj.clientds.Add(c);
            obj.SaveChanges();

            return RedirectToAction("SignIn");
        }
        public ActionResult Vendor_SignUp()
        {
            ViewBag.country = (from m in obj.Countries select m).ToList();
            ViewBag.state = (from m in obj.States select m).ToList();
            ViewBag.city = (from m in obj.Cities select m).ToList();
            ViewBag.area = (from m in obj.Areas select m).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Vendor_SignUp(VendorTb v)
        {
            ViewBag.country = (from m in obj.Countries select m).ToList();
            ViewBag.state = (from m in obj.States select m).ToList();
            ViewBag.city = (from m in obj.Cities select m).ToList();
            ViewBag.area = (from m in obj.Areas select m).ToList();

            HttpPostedFileBase f = Request.Files[0];
            string rpath = Server.MapPath("../BoutiqueImages") + "/";
            f.SaveAs(rpath + f.FileName);

            // now insert into db

            v.bpic = f.FileName;

            obj.VendorTbs.Add(v);

            obj.SaveChanges();

            return RedirectToAction("SignIn");
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ShowCat(int id)
        {
            Session["v_id"] = id;
            var q = (from m in obj.Categories where m.btid == id select m).ToList();
            return View(q);
        }

    }
}