using System.Collections.Generic;
using CarsApp.Models;
using System.Threading.Tasks;
using CarsApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarsApp.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroupService _groupService;
        public CarService(ApplicationDbContext context,
                          IGroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        public async Task AddCar(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsAsync(IdentityUser user)
        {
            return await _context.Cars.Where(x => x.User.Id == user.Id).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByGroup(int group)
        {
            var users = await _groupService.FindAllUsersInGroup(group);
            var usrIds = users.Select(x => x.Id);
            var cars = await _context.Cars.Where(x => usrIds.Contains(x.User.Id)).ToListAsync();
            return cars;
        }
    }
}