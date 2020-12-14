using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarsApp.Models;
using Microsoft.AspNetCore.Authorization;
using CarsApp.Data;
using Microsoft.AspNetCore.Identity;
using CarsApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApp.Controllers
{        
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICarService _carService;
        private readonly IGroupService _groupService;
        public CarsController(UserManager<IdentityUser> userManager,
                              ApplicationDbContext context,
                              ICarService carService,
                              IGroupService groupService)
        {
            _context = context;
            _userManager = userManager;
            _carService = carService;
            _groupService = groupService;
        }

        private async Task<CreateCarViewModel> CreateVM(string id = null) 
        {
            IEnumerable<Car> cars;
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var vm = new CreateCarViewModel();
            var colours = await _context.Colours.ToListAsync();
            var conditions = await _context.Conditions.ToListAsync();
            if (id != null)
            {
                cars = await _carService.GetCarsByGroup(int.Parse(id));
            }
            else
            {
                cars = await _carService.GetCarsAsync(user);
            }
            vm.Chart = new List<ChartVM>();
            vm.Colours = new List<SelectListItem>();
            vm.Conditions = new List<SelectListItem>();
            vm.Groups = new List<SelectListItem>();
            vm.Cars = new List<Car>();
            vm.Cars = cars.ToList();
            var userGroups = await _groupService.FindGroupsByUser(user);

            var newCars = cars.Where(x => x.Condition.Condition == "New").Count();
            var totalCars = cars.Count();

            foreach (var c in colours)
            {
                vm.Colours.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.ColorName });
                vm.Chart.Add(new ChartVM { Colour = c.ColorName, Quantity = cars.Where(x => x.Color.ColorName == c.ColorName).Count() });
            }

            foreach (var cond in conditions)
            {
                vm.Conditions.Add(new SelectListItem { Value = cond.Id.ToString(), Text = cond.Condition });
            }

            foreach (var usrGroup in userGroups) 
            {
                vm.Groups.Add(new SelectListItem { Value = usrGroup.Id.ToString(), Text = usrGroup.Name });
            }

            try 
            {
                vm.AveragePrice = cars.Sum(x => x.Price) / totalCars;
                vm.PercentageOfNewCars = ((decimal)newCars / (decimal)totalCars) * 100;
            }
            catch
            {
                //no cars, no price
                vm.AveragePrice = 0;
                vm.PercentageOfNewCars = 0;
            }
            
            
            return vm;
        }

        [HttpGet]
        public async Task<IActionResult> Cars()
        {
            var vm = await CreateVM();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> FilterByGroup(CreateCarViewModel model)
        {
            var vm = await CreateVM(model.SelectedGroup);
            return View("Cars", vm);
        }
        

        [HttpPost]
        public async Task<IActionResult> Cars(CreateCarViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var addRecord = new Car
            {
                Color = _context.Colours.Where(x => x.Id == int.Parse(model.SelectedColour)).First(),
                Condition = _context.Conditions.Where(x => x.Id == int.Parse(model.SelectedCondition)).First(),
                User = user,
                Price = model.Price
            };

            await _carService.AddCar(addRecord);

            var vm = await CreateVM();
            return View(vm);
        }
        
    }
}