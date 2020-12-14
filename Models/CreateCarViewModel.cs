using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApp.Models
{
    public class CreateCarViewModel
    {
        public List<SelectListItem> Groups { get; set; }
        public string SelectedGroup { get; set; }
        public List<Car> Cars { get; set; }
        public List<SelectListItem> Conditions { get; set; }
        public string SelectedCondition { get; set; }
        public List<SelectListItem> Colours { get; set; }
        public string SelectedColour { get; set; }
        public decimal Price { get; set; }
        public decimal PercentageOfNewCars { get; set; }
        public decimal AveragePrice { get; set; }
        public List<ChartVM> Chart { get; set; }
    }

    public class ChartVM 
    {
        public string Colour { get; set; }
        public int Quantity { get; set; }
    }
}