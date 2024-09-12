namespace ElectroDetali.Models
{
    public class Good
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public Category Category { get; set; }

        public string Image { get; set; }
    }
}
