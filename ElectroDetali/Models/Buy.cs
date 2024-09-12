namespace ElectroDetali.Models
{
    public class Buy
    {
        public int Id { get; set; }

        public Good Good { get; set; }

        public int? Userid { get; set; }

        public string Email { get; set; }

        public DateTime? Datecreate { get; set; }

        public DateTime? Datedelivery { get; set; }

        public bool? Isbasket { get; set; }

        public Point Point { get; set; }
    }
}
