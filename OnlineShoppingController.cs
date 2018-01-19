using angulajs_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace angulajs_project.Controllers
{
    public class OnlineShoppingController : Controller
    {
        // GET: OnlineShopping
        ProductContext ctx;
        public OnlineShoppingController()
        {
            ctx = new ProductContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct2(Products p)
        {
            if (ModelState.IsValid)
            {

            }
            return View(new Products());
        }

        public ActionResult AddProduct2()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(HttpPostedFileBase photo, Models.Products prd)
        {
            if (!ModelState.IsValid)
            {
                string physicalpath = HostingEnvironment.MapPath("~//image");
                string[] files = System.IO.Directory.GetFiles(physicalpath);
                string y = null;
                angulajs_project.Models.Products pr1 = new Models.Products();
                foreach (var x in files)
                {
                    string strTemp = x.Split(new string[] { "image\\" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (strTemp == photo.FileName)
                    {
                        string strTemp1 = strTemp.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        var add2 = ctx.products.ToList();
                        var add = add2.LastOrDefault();
                        y = (strTemp1 + "" + add.Pid + ".jpg");

                        break;
                    }
                    else
                    {
                        y = photo.FileName;
                    }
                }
                photo.SaveAs(physicalpath + "/" + y);
                prd.Image = y;
                string[] file = System.IO.Directory.GetFiles(physicalpath);

                pr1.Pname = prd.Pname;
                pr1.Desc = prd.Desc;
                pr1.Price = prd.Price;
                pr1.Image = prd.Image;

                ctx.products.Add(pr1);
                ctx.SaveChanges();
                ModelState.Clear();
                return Json(prd, JsonRequestBehavior.AllowGet); ;
            }
            else
            {
                return Json(prd, JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult ListProducts()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListProducts(Models.Products prd)
        {
            string physicalpath = HostingEnvironment.MapPath("~//image");
            string[] files = System.IO.Directory.GetFiles(physicalpath);
            List<data> list = new List<data>();

            foreach (var image in files)
            {
                string strTemp = image.Split(new string[] { "image\\" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                data values = (from p in ctx.products.Where(pro => pro.Image == strTemp)
                               select new data()
                               {
                                   Pid = p.Pid,
                                   Pname = p.Pname,
                                   Price = p.Price,
                                   Desc = p.Desc,
                                   Image = strTemp

                               }).FirstOrDefault();
                list.Add(values);
            }
            //int items = ctx.Carts.Count(c => c.UserId == 100 && c.Status == 0);
            //ViewBag.count = items;
            //return View(list);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}