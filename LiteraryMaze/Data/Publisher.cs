namespace LiteraryMaze.Data
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Informacion { get; set; }
        public string ImageURL { get; set; }
        public DateTime DateRegister { get; set; }  = DateTime.Now; 
    }
}
