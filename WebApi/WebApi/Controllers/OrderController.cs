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
    public class OrderController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Order
        public object GetOrders()
        {
            var result = (from order in db.Orders
                          join cust in db.CustomerMasters on order.CustomerId equals cust.CustomerId

                          select new
                          {
                              order.OrderId,
                              order.OrderNo,
                              Customer = cust.Name,
                              order.PaymentMethod,
                              order.GarndTotal
                          }).ToList();

            return result;
        }

        // GET: api/Order/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(long id)
        {
            var order = (from o in db.Orders
                         where o.OrderId == id

                         select new
                         {
                             o.OrderId,
                             o.OrderNo,
                             o.PaymentMethod,
                             o.GarndTotal,
                             o.CustomerId,
                             DeletedItemIds = ""
                         }).FirstOrDefault();

            var orderItems = (from orderItem in db.OrderItems
                              join item in db.ItemMasters on orderItem.ItemId equals item.ItemId
                              where orderItem.OrderId == id

                              select new
                              {
                                  orderItem.OrderId,
                                  orderItem.OrderItemId,
                                  item.ItemId,
                                  ItemName = item.Name,
                                  item.Price,
                                  orderItem.Quantity,
                                  Total = orderItem.Quantity * item.Price
                              }).ToList();
            return Ok(new { order, orderItems });
        }

        // PUT: api/Order/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(long id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Order
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {

            try
            {
                // Order table 
                if (order.OrderId == 0)
                {
                    db.Orders.Add(order);
                }
                else
                {
                    db.Entry(order).State = EntityState.Modified;
                }


                // Order Items 
                foreach (var item in order.OrderItems)
                {
                    if (item.OrderItemId == 0)
                    {
                        db.OrderItems.Add(item);
                    }
                    else
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                }

                //Delete Oreder Items 
                if (!string.IsNullOrEmpty(order.DeletedItemIds))
                {
                    var deletedIds = order.DeletedItemIds.Split(',').Where(t => t != "");
                    foreach (var id in deletedIds)
                    {
                        OrderItem item = db.OrderItems.Find(Convert.ToInt64(id));
                        db.OrderItems.Remove(item);
                    }
                }


                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        // DELETE: api/Order/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(long id)
        {
            Order order = db.Orders.Include(y => y.OrderItems).SingleOrDefault(o => o.OrderId == id);

            foreach (var item in order.OrderItems.ToList())
            {
                db.OrderItems.Remove(item);
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(long id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }
}