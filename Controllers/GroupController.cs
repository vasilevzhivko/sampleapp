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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApp.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGroupService _groupService;
        public GroupController(UserManager<IdentityUser> userManager,
                              ApplicationDbContext context,
                              IGroupService groupService)
        {
            _context = context;
            _userManager = userManager;
            _groupService = groupService;
        }

        
        public async Task<IActionResult> CreateGroup()
        {
            var vm = await CreateVM();

            return View(vm);
        }
        private async Task<CreateJoinGroupVM> CreateVM() 
        {
            var vm = new CreateJoinGroupVM();
            vm.AllGroups = new List<SelectListItem>();
            var allGroups = await _groupService.GetAllGroups();
            foreach (var group in allGroups)
            {
                vm.AllGroups.Add(new SelectListItem { Value = group.Id.ToString(), Text = group.Name });
            }
            return vm;
        }

        [HttpPost]
        public async Task<IActionResult> JoinGroup(CreateJoinGroupVM groupVM)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var group = await _groupService.FindGroupById(int.Parse(groupVM.SelectedGroup));
            await _groupService.AddUserToGroup(user, group);
            return View("CreateGroup");
        }

        [HttpPost]
        public async Task<IActionResult> GroupCreated(CreateJoinGroupVM groupVM)
        {
            var group = new Group
            {
                Name = groupVM.Group.Name,
                IsApproved = false
            };
            bool groupExists = _context.Groups.Where(x => x.Name == groupVM.Group.Name).Count() >= 1 ? true : false;

            if (groupExists)
            {
                ModelState.AddModelError("Name", "Group name already exists");
                return View("CreateGroup");
            }
            else 
            {
                await _groupService.CreateGroup(groupVM.Group.Name);
            }

            return View();
        }
        
    }
}