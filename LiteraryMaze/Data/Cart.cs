namespace LiteraryMaze.Data
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User Users { get; set; }
        public int BookId { get; set; }
        public Book Books { get; set; }
        public decimal FinalPrice { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
    }
}