using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CarsApp.Models
{
    public class AdminPageViewModel
    {
        public List<Group> AllGroups { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public string SelectedGroup { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string SelectedUser { get; set; }

    }
}