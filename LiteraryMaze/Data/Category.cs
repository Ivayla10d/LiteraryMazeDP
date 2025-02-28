namespace LiteraryMaze.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
        public ICollection<Book> Books { get; set; }
    }
}
