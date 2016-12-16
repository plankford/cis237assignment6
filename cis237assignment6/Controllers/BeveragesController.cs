using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    //Protect every route that is taken
    [Authorize]

    public class BeveragesController : Controller
    {
        private BeveragePLankfordEntities db = new BeveragePLankfordEntities();

        // GET: Beverages
        public ActionResult Index()
        {
            //Variable to hold the beverages dataset
            DbSet<Beverage> BeveragesToSearch = db.Beverages;

            //Strings that will hold the data that will be in the session
            //If there is nothing here the variables could still be used
            //as default values
            string filterMinPrice = "";
            string filterMaxPrice = "";

            //Define the default min and max decimal for the Price
            decimal minPrice = 0m;
            decimal maxPrice = 1000m;
           
            //Check the session and see if there is a value
            //Assign them to the variables that are setup to hold the values
            if (Session["minPrice"] != null && !string.IsNullOrWhiteSpace((string)Session["minPrice"]))
            {
                filterMinPrice = (string)Session["minPrice"];
                minPrice = decimal.Parse(filterMinPrice);
            }

            if (Session["maxPrice"] != null && !string.IsNullOrWhiteSpace((string)Session["maxPrice"]))
            {
                filterMaxPrice = (string)Session["maxPrice"];
                maxPrice = decimal.Parse(filterMaxPrice);
            }

            //Do the filter on the dataset. Use the where and send in more lambda expressions
            //to narrow the search down further.
            IEnumerable<Beverage> filtered = BeveragesToSearch.Where(beverage => beverage.price >= minPrice &&
                                                                                 beverage.price <= maxPrice);
                                                                                        
                                                                                                                                                           

            //Place the string representation of the values in the session into the Viewbag
            //so that it can be retrieved and displayed on the view
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

            //Return the view with the filtered selections
            return View(filtered);

        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Filter method that will take in the new data and submit it to the form. This will be stored
        //in the session so we can accces it later.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            String minPrice = Request.Form.Get("minPrice");
            String maxPrice = Request.Form.Get("maxPrice");

            //Store that data into the session
            Session["minPrice"] = minPrice;
            Session["maxPrice"] = maxPrice;

            //Redirect the user back to the index page
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
