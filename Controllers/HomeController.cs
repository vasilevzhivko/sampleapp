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

namespace CarsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICarService _carService;
        public HomeController(ILogger<HomeController> logger,
                              RoleManager<IdentityRole> roleManager,
                              UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              ApplicationDbContext context,
                              ICarService carService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _carService = carService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task<IActionResult> CreateData()
        {
            // Add sample data like colours and conditions
            var blackColor = new CarColour
            {
                ColorName = "black"
            };
            var redColor = new CarColour
            {
                ColorName = "red"
            };
            var whiteColor = new CarColour
            {
                ColorName = "white"
            };
            var greenColor = new CarColour
            {
                ColorName = "green"
            };

            var addColorsToList = new List<CarColour> { blackColor, redColor, whiteColor, greenColor };
            _context.Colours.AddRange(addColorsToList);

            var newCondition = new CarCondition
            {
                Condition = "New"
            };
            var secondHandCondition = new CarCondition
            {
                Condition = "Second hand"
            };

            // create roles and add administrator user
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole("User"));

            var user = new IdentityUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

            var result = await _userManager.CreateAsync(user, "Administrator@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Administrator");
            }

            _context.SaveChanges();

            return View();
        }
    }
}
