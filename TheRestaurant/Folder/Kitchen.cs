using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;
using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Kitchen
    {
        public List<Chef> Chefs { get; set; } = new();
        public Dictionary<string, List<Food>> InOrders { get; set; } = new();
        public Dictionary<string, List<Food>> OutOrders { get; set; } = new();
        public bool HasOrders { get; set; }



    }
}
