using Microsoft.AspNetCore.Identity;

namespace LiteraryMaze.Data
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}