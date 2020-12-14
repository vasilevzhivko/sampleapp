using Microsoft.AspNetCore.Identity;

namespace CarsApp.Models
{
    public class GroupUsers
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public IdentityUser User { get; set; }
    }
}