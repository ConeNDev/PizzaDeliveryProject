namespace PizzaApp.Domain.Entities.API
{
    public class PizzaLoadAPI
    {
        public string Status { get; set; }

        public int Results { get; set; }

        public PizzaData Data { get; set; }
    }
}