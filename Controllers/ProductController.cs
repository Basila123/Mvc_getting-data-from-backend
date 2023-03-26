using MVCWebapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace MVCWebapp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            using (BasilaEntities db = new BasilaEntities())
            {
                List<Product> Productlist = (from data in db.Products
                                             select data).ToList();
                return View(Productlist);
            }

        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase postedFile)
        //HttpPostedFileBase is for adding image and postedfile is the name of file increate.cshtml
        {
            try
            {
                // TODO: Add insert logic here

                //string extension = postedFile.FileName;
                //or
                string extension = Path.GetExtension(postedFile.FileName);
                if (extension.Equals(".jpg") || extension.Equals(".png"))
                {
                    string filename = "IMG-" + DateTime.Now.ToString("yyyyMMddhhssffff") + extension;
                    string savepath = Server.MapPath("~/Content/Image/");
                    postedFile.SaveAs(savepath + filename);
                    product.prod_pic = filename;
                    using (BasilaEntities db = new BasilaEntities())
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                }
                else
                {
                    return Content("<H1>you can only jpg or ong file</H1>");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            using (BasilaEntities db = new BasilaEntities())
            {
                //Product product= (from item in db.Products
                //                  where item.prod_id == id
                //                  select item).Single();

                //another way

                Product product = db.Products.Find(id);

                return View(product);
            }

        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product,HttpPostedFileBase postedFile)
        {
            try
            {
                // TODO: Add update logic here
                string filename = "";
                //if we are updating an image
                if (postedFile != null)
                {
                    string extension = Path.GetExtension (postedFile.FileName);
                    if (extension.Equals(".jpg") || extension.Equals(".png"))
                    {
                        filename = "IMG-" + DateTime.Now.ToString("yyyyMMddhhssffff") + extension;
                        string savepath = Server.MapPath("~/Content/Image/");
                        postedFile.SaveAs(savepath + filename);
                           
                        
                    }
                }
                //if we are not changing the image
           
                using(BasilaEntities db=new BasilaEntities())
                {
                    Product item = db.Products.Find(product.prod_id);
                    item.prod_name= product.prod_name;
                    item.prod_qty=product.prod_qty;
                    item.prod_price=product.prod_price;
                    db.Products.AddOrUpdate(product);
                    // if we  add new file
                    if(!filename.Equals(""))
                    {
                        item.prod_pic= filename;
                    } 
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/ 5
        public ActionResult Delete(int id)
        {
            using(BasilaEntities db=new BasilaEntities())
            {
                db.Products.Remove(db.Products.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
