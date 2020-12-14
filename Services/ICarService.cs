using CarsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CarsApp.Services
{
    public interface ICarService
    {
        Task AddCar(Car car);

        Task<IEnumerable<Car>> GetCarsAsync(IdentityUser user);
        Task<IEnumerable<Car>> GetCarsByGroup(int group);
    }
}