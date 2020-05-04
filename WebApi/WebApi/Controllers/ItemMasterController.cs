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
    public class ItemMasterController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/ItemMaster
        public IQueryable<ItemMaster> GetItemMasters()
        {
            return db.ItemMasters;
        }

        // GET: api/ItemMaster/5
        [ResponseType(typeof(ItemMaster))]
        public IHttpActionResult GetItemMaster(int id)
        {
            ItemMaster itemMaster = db.ItemMasters.Find(id);
            if (itemMaster == null)
            {
                return NotFound();
            }

            return Ok(itemMaster);
        }

        // PUT: api/ItemMaster/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItemMaster(int id, ItemMaster itemMaster)
        {
            if (id != itemMaster.ItemId)
            {
                return BadRequest();
            }

            db.Entry(itemMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemMasterExists(id))
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

        // POST: api/ItemMaster
        [ResponseType(typeof(ItemMaster))]
        public IHttpActionResult PostItemMaster(ItemMaster itemMaster)
        {
            db.ItemMasters.Add(itemMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = itemMaster.ItemId }, itemMaster);
        }

        // DELETE: api/ItemMaster/5
        [ResponseType(typeof(ItemMaster))]
        public IHttpActionResult DeleteItemMaster(int id)
        {
            ItemMaster itemMaster = db.ItemMasters.Find(id);
            if (itemMaster == null)
            {
                return NotFound();
            }

            db.ItemMasters.Remove(itemMaster);
            db.SaveChanges();

            return Ok(itemMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemMasterExists(int id)
        {
            return db.ItemMasters.Count(e => e.ItemId == id) > 0;
        }
    }
}