using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;
using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Table
    {
        public List<Guest> Guests { get; set; } = new List<Guest> { };
        public List<Food> Order { get; set; } = new List<Food> { };
        public string Name { get; set; }
        public bool Small { get; set; }
        public bool IsDirty { get; set; }
        public bool Occupied { get; set; }
        public bool HasOrdered { get; set; }
        public bool WaitingForFood { get; set; }
        public int PlacementScore { get; set; }
        public int WaitingTimeScore { get; set; }
        public int EatingFoodCounter { get; set; }
        public List<Food> Menu { get; set; } = new();
        public int ServiceScore { get; set; }

        public Table(Random random, string name, bool small)
        {
            Name = name;
            PlacementScore = random.Next(1, 6);
            Small = small;

            Food.CreateMenu(Menu);
        }


        internal static void Create(Random random, List<Table> tables, bool small, int amount)
        {
            int number = tables.Count;
            for (int i = number + 1; i <= amount + number; i++)
            {
                string name = "Table " + i;
                tables.Add(new Table(random, name, small));
            }
        }


    }
}
