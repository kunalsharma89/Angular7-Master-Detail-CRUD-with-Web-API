using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CustomerMasterController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/CustomerMaster
        public IQueryable<CustomerMaster> GetCustomerMasters()
        {
            return db.CustomerMasters;
        }

        // GET: api/CustomerMaster/5
        [ResponseType(typeof(CustomerMaster))]
        public IHttpActionResult GetCustomerMaster(int id)
        {
            CustomerMaster customerMaster = db.CustomerMasters.Find(id);
            if (customerMaster == null)
            {
                return NotFound();
            }

            return Ok(customerMaster);
        }

        // PUT: api/CustomerMaster/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomerMaster(int id, CustomerMaster customerMaster)
        {
            if (id != customerMaster.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(customerMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CustomerMaster
        [ResponseType(typeof(CustomerMaster))]
        public IHttpActionResult PostCustomerMaster(CustomerMaster customerMaster)
        {
            
            db.CustomerMasters.Add(customerMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customerMaster.CustomerId }, customerMaster);
        }

        // DELETE: api/CustomerMaster/5
        [ResponseType(typeof(CustomerMaster))]
        public IHttpActionResult DeleteCustomerMaster(int id)
        {
            CustomerMaster customerMaster = db.CustomerMasters.Find(id);
            if (customerMaster == null)
            {
                return NotFound();
            }

            db.CustomerMasters.Remove(customerMaster);
            db.SaveChanges();

            return Ok(customerMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerMasterExists(int id)
        {
            return db.CustomerMasters.Count(e => e.CustomerId == id) > 0;
        }
    }
}