﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using vproker.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace vproker.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        [FromServices]
        public ApplicationDbContext AppContext { get; set; }

        [FromServices]
        public ILogger<OrderController> Logger { get; set; }

        [Authorize(Roles = "User")]
        public IActionResult Index(string sortOrder, string searchString, string filter)
        {
            var orders = new Order[0];

            if (AppContext.Orders.Count() > 0)
            {
                orders = AppContext.Orders.Include(o => o.Tool).ToArray();

                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ToolSortParm = sortOrder == "Tool" ? "tool_desc" : "Tool";
                ViewBag.FilterParm = String.IsNullOrEmpty(filter) ? "active" : filter;

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o => o.ClientName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.ClientName).ToArray();
                        break;
                    case "Tool":
                        orders = orders.OrderBy(o => o.Tool.Name).ToArray();
                        break;
                    case "tool_desc":
                        orders = orders.OrderByDescending(o => o.Tool.Name).ToArray();
                        break;
                    default: //name ascending
                        orders = orders.OrderBy(s => s.ClientName).ToArray();
                        break;
                }
                switch((string)ViewBag.FilterParm)
                {
                    case "active":
                        orders = orders.Where(o => !o.IsClosed).ToArray();
                        break;

                    case "closed":
                        orders = orders.Where(o => o.IsClosed).ToArray();
                        break;
                }
            }

            return View(orders);
        }

        public async Task<ActionResult> Details(string id)
        {
            Order order = await AppContext.Orders
                /*.Include(b => b.Client)*/.Include(b => b.Tool)
                .SingleOrDefaultAsync(b => b.ID == id);
            if (order == null)
            {
                Logger.LogInformation("Details: Item not found {0}", id);
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            ViewBag.Clients = GetClientsListItems();
            ViewBag.Tools = GetToolsListItems();
            return View();
        }

        private IEnumerable<SelectListItem> GetClientsListItems(string selectedId = null)
        {
            var tmp = AppContext.Clients.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

            // Create authors list for <select> dropdown
            return tmp
                .OrderBy(client => client.LastName)
                .Select(client => new SelectListItem
                {
                    Text = client.FullName,
                    Value = client.ID.ToString(),
                    Selected = client.ID == selectedId
                });
        }

        private IEnumerable<SelectListItem> GetToolsListItems(string selectedId = null)
        {
            var tmp = AppContext.Tools.ToList();  // Workaround for https://github.com/aspnet/EntityFramework/issues/2246

            return tmp
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.ID,
                    Selected = t.ID == selectedId
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppContext.Orders.Add(order);
                    await AppContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            Order order = await FindOrderAsync(id);
            if (order == null)
            {
                Logger.LogInformation("Edit: Item not found {0}", id);
                return HttpNotFound();
            }

            //ViewBag.Clients = GetClientsListItems(order.ClientID);
            ViewBag.Tools = GetToolsListItems(order.ToolID);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(string id, Order order)
        {
            try
            {
                order.ID = id;
                AppContext.Orders.Attach(order);
                AppContext.Entry(order).State = EntityState.Modified;
                await AppContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(order);
        }

        private Task<Order> FindOrderAsync(string id)
        {
            return AppContext.Orders.Include(o => o.Tool).SingleOrDefaultAsync(order => order.ID == id);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(string id, bool? retry)
        {
            Order order = await FindOrderAsync(id);
            if (order == null)
            {
                Logger.LogInformation("Delete: Item not found {0}", id);
                return HttpNotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(order);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Order order = await FindOrderAsync(id);
                AppContext.Orders.Remove(order);
                await AppContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, retry = true });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Close")]
        public async Task<ActionResult> ConfirmClose(string id, bool? retry)
        {
            Order order = await FindOrderAsync(id);
            if (order.EndDate != null)
            {
                throw new Exception("Заказ не может быть закрыт дважды. Дата закрытия заказа уже указана - " + order.EndDate);
            }
            if (order == null)
            {
                Logger.LogInformation("Close: Item not found {0}", id);
                return HttpNotFound();
            }
            ViewBag.Retry = retry ?? false;
            return View(order);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(string id)
        {
            try
            {
                Order order = await FindOrderAsync(id);
                if(order.EndDate != null)
                {
                    throw new Exception("Заказ не может быть закрыт дважды. Дата закрытия заказа уже указана - " + order.EndDate);
                }
                CloseOrder(order);
                await AppContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Close", new { id = id, retry = true });
            }
            return RedirectToAction("Index");
        }

        private void CloseOrder(Order order)
        {
            order.EndDate = DateTime.Now;
            order.Price = CalculatePaymentForDays(order.EndDate.Value, order.StartDate, order.Price);
        }

        public static Decimal CalculatePaymentForDays(DateTime start, DateTime end, Decimal dayPrice)
        {
            TimeSpan period = end.Subtract(start);

            return (int)period.TotalDays * dayPrice;
        }
    }
}
