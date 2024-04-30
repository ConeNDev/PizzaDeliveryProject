using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaApp.Infrastructure.Persistence;
using PizzaApp.Domain.Entities;
using System.Security.Claims;

namespace PizzaApp.Presentation.Controllers
{
    public class CartController : Controller

    {

        private readonly PizzaAppDBContext _dbContext;

        public CartController(PizzaAppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //kupi objekat Cart ako postoji ako ne postoji onda pravi novi objekat
            var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return View(cart.Orders);
        }

        [HttpPost]
        public IActionResult AddToCart(Order order)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            cart.AddOrder(order);

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int orderId)
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            cart.RemoveOrder(orderId);

            HttpContext.Session.SetObject("Cart", cart);

            return Json(new { success = true });
        }


        // Metod za Checkout nije definisan, ali možete dodati logiku za naplatu ovde
        [HttpGet]
        public IActionResult Checkout()
        {
            // Checkout logika
            return View();
        }
        public IActionResult CartListPartial()
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return PartialView("_CartList", cart.Orders);
        }

        [HttpPost]
        public IActionResult OrderAll()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            cart.OrderTime = DateTime.Now;
            _dbContext.Carts.Add(cart);
            foreach (var order in cart.Orders)
            {

                order.CartId = cart.Id;
                order.OrderTime = DateTime.Now;
                _dbContext.Orders.Add(order);
            }

            _dbContext.SaveChanges();
            cart.Clear();

            HttpContext.Session.SetObject("Cart", new Cart());

            return PartialView("_CartList", cart.Orders);
        }
    }
}
