namespace LiteraryMaze.Data
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
        public ICollection<Book> Books { get; set; }
    }
}
