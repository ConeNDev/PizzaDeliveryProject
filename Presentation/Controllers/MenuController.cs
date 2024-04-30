using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PizzaApp.Infrastructure.Persistence;
using PizzaApp.Domain.Entities;
using PizzaApp.Presentation.ViewModels;
using PizzaApp.Domain.Entities.API;

namespace PizzaApp.Presentation.Controllers
{
    public class MenuController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PizzaAppDBContext context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;

        public MenuController(UserManager<ApplicationUser> userManager,
            PizzaAppDBContext dBcontext, IMemoryCache memoryCache,
            IHttpClientFactory clientFactory)
        {
            _userManager = userManager;
            context = dBcontext;
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Provera da li su podaci već keširani u IMemoryCache
            if (!_memoryCache.TryGetValue("Pizzas", out List<Pizza> pizzas)
                || searchTerm != null)
            {
                pizzas = await GetPizzasFromAPIAsync();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var filteredPizzas = pizzas.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                    _memoryCache.Set("Pizzas", pizzas, TimeSpan.FromMinutes(10));
                    return View(filteredPizzas);
                }

                // Keširaj podatke u IMemoryCache sa odgovarajućim ključem ("Pizzas")
                _memoryCache.Set("Pizzas", pizzas, TimeSpan.FromMinutes(10));
            }

            return View(pizzas);
        }
        private async Task<List<Pizza>> GetPizzasFromAPIAsync()
        {
            var client = _clientFactory.CreateClient();

            var apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
            var apiKey = "6ea9cc64-7f18-4d42-860a-4b4b3e23b172";
            var menuName = "pizza";

            try
            {
                var response = await client.GetAsync($"{apiURL}?search={menuName}&key={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    PizzaLoadAPI pizzaResponse = await response.Content.ReadFromJsonAsync<PizzaLoadAPI>();
                    List<Pizza> pizzas = pizzaResponse?.Data?.Recipes;
                    return pizzas;
                }
                else
                {
                    throw new Exception("Failed to retrieve pizzas from API. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching pizzas from API.", ex);
            }
        }


        [HttpPost]
        public IActionResult GetMenuCard([FromBody] MenuSearchViewModel model)
        {
            var allPizzas = model.MenuCard;
            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                var filteredPizzas = allPizzas.Where(p => p.Title.ToLower().Contains(model.SearchTerm.ToLower()));
                return PartialView("_MenuCard", filteredPizzas);
            }

            return PartialView("_MenuCard", allPizzas);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order([FromForm] Order order)
        {
            order.OrderTime = DateTime.Now;
            if (ModelState.IsValid)
            {

                context.Orders.Add(order);
                context.SaveChanges();
                return RedirectToAction("Index", "Menu");
            }
            return RedirectToAction("Order", "Menu", new { id = order.Id });
        }
        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> ShowOrder(OrderedPizza orderedPizzaDetails)
        {

            Random random = new Random();
            ViewBag.Price = random.Next(2, 14);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;
            return PartialView("_ShowOrder", orderedPizzaDetails);
        }


    }
}
