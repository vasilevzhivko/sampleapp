using Microsoft.AspNetCore.Identity;
using CarsApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace CarsApp.Services
{
    public interface IGroupService
    {
        Task CreateGroup(string groupName);
        Task ApproveGroup(int id);
        Task AddUserToGroup(IdentityUser user, Group gr);
        Task<List<Group>> FindGroupsByUser(IdentityUser user);
        Task<List<IdentityUser>> FindAllUsersInGroup(int groupId);
        Task<List<Group>> GetAllGroups();
        Task<Group> FindGroupById(int id);
    }
}