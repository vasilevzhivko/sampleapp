using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CarsApp.Models
{
    public class CreateJoinGroupVM
    {
        public Group Group { get; set; }
        public List<SelectListItem> AllGroups { get; set; }
        public string SelectedGroup { get; set; }
    }
}