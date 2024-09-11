using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace PizzaApp.Presentation.Configurations
{
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // Ovde nema rekurzije, samo ispisujemo poruku za proveru poziva
            System.Diagnostics.Debug.WriteLine("PopulateValues pozvan");
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Ispisujemo poruku za proveru poziva
            System.Diagnostics.Debug.WriteLine("ExpandViewLocations pozvan");

            
            var customLocations = new List<string>
            {
                "/Presentation/Views/Shared/_Layout.cshtml"
            };

            return customLocations.Concat(viewLocations);
        }
    }
}