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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CarsApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGroupService _groupService;
        public AdminController(UserManager<IdentityUser> userManager,
                               ApplicationDbContext context,
                               IGroupService groupService)
        {
            _context = context;
            _userManager = userManager;
            _groupService = groupService;
        }

        public async Task<IActionResult> Admin()
        {
            var vm = await CreateVM();
            return View(vm);
        }
        
        public async Task<IActionResult> AddUserToGroup(AdminPageViewModel vm)
        {
            var model = await CreateVM();
            var group = _context.Groups.Where(x => x.Id == int.Parse(vm.SelectedGroup)).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(vm.SelectedUser);
            await _groupService.AddUserToGroup(user, group);
            return View("Admin", model);
        }
        public async Task<IActionResult> Approve(int id)
        {
            await _groupService.ApproveGroup(id);
            var vm = await CreateVM();
            return View("Admin", vm);
        }

        private async Task<AdminPageViewModel> CreateVM()
        {
            var vm = new AdminPageViewModel();
            var users = await _userManager.Users.ToListAsync();
            var groups = await _context.Groups.ToListAsync();
            vm.Users = new List<SelectListItem>();
            vm.Groups = new List<SelectListItem>();
            vm.AllGroups = new List<Group>();
            vm.AllGroups = groups;

            foreach (var user in users) 
            {
                vm.Users.Add(new SelectListItem { Value = user.Id.ToString(), Text = user.UserName });
            }
            foreach (var group in groups)
            {
                vm.Groups.Add(new SelectListItem { Value = group.Id.ToString(), Text = group.Name });
            }
            return vm;
        }
    }
}