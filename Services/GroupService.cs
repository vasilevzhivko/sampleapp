using System.Collections.Generic;
using CarsApp.Models;
using System.Threading.Tasks;
using CarsApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarsApp.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public GroupService(ApplicationDbContext context,
                            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddUserToGroup(IdentityUser user, Group gr)
        {
            await _context.GroupUsers.AddAsync(new GroupUsers { User = user, Group = gr });
            await _context.SaveChangesAsync();
        }

        public async Task ApproveGroup(int id)
        {
            var group = _context.Groups.Where(g => g.Id == id).FirstOrDefault();
            if (group != null)
            {
                group.IsApproved = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateGroup(string groupName)
        {
            await _context.Groups.AddAsync(new Group { Name = groupName, IsApproved = false });
            await _context.SaveChangesAsync();
        }

        public async Task<List<IdentityUser>> FindAllUsersInGroup(int groupId)
        {
            List<IdentityUser> users = new List<IdentityUser>();
            var userGroups = await _context.GroupUsers.Where(g => g.Group.Id == groupId).Select(x => x.User.Id).ToListAsync();
            foreach (var usr in userGroups)
            {
                if (usr != null)
                {
                    var user = await _userManager.FindByIdAsync(usr);
                    users.Add(user);
                }
            }

            return users;
        }

        public async Task<Group> FindGroupById(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.Id == id && x.IsApproved == true);
        }

        public async Task<List<Group>> FindGroupsByUser(IdentityUser user)
        {
            var userGroups = await _context.GroupUsers.Where(u => u.User == user).Select(x => x.Group.Id).ToListAsync();
            var groups = await _context.Groups.Where(x => userGroups.Contains(x.Id)).ToListAsync();
            return groups;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _context.Groups.Where(x=> x.IsApproved == true).ToListAsync();
        }
    }
}