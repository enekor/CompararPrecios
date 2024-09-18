namespace CompararPrecios.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public string? Shop {get;set;}
        public string? Url { get; set;}
    }
}
