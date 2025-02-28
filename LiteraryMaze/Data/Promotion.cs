namespace LiteraryMaze.Data
{
    public class Promotion
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Books { get; set; }
        public double PromotionPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
    }
}