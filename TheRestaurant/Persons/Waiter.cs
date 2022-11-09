using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Foods;

namespace TheRestaurant.Persons
{
    internal class Waiter : Person
    {
        //public List<Food> Order { get; set; } = new List<Food>();

        // IF THIS DOESNT WORK, use string insted of table.
        public Dictionary<string, List<Food>> InOrder { get; set; } = new();
        public Dictionary<string, List<Food>> OutOrder { get; set; } = new();
        public bool HasOrder { get; set; }
        public bool HasFoodToDeliver { get; set; }
        public bool CleaningTable { get; set; }

        public int ServiceScore { get; set; }


        public Waiter(Random _random, string name)
        {
            Name = name;
            ServiceScore = _random.Next(1, 6);
        }
    }


}
