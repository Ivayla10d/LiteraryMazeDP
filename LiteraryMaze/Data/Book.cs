namespace LiteraryMaze.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string CatNumber { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public Author Authors { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publishers { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public int GenreId { get; set; }
        public Genre Genres { get; set; }
        public string Plot { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Year { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}