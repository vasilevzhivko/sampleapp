using Microsoft.AspNetCore.Identity;

namespace CarsApp.Models
{
    public class Car
    {
        public int Id { get; set; }
        public CarCondition Condition { get; set; }
        public CarColour Color { get; set; }
        public decimal Price { get; set; }
        public IdentityUser User { get; set; }
    }
}