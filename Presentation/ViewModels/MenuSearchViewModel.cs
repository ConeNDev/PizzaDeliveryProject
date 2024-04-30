using PizzaApp.Domain.Entities;

namespace PizzaApp.Presentation.ViewModels
{
    public class MenuSearchViewModel
    {
        public List<Pizza> MenuCard { get; set; }
        public string SearchTerm { get; set; }
    }
}