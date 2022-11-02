using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Foods
{
    internal class Food : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
        public bool IsVegetarian { get; set; }

        public Food(string name, int price, bool isVegetarian)
        {
            Name = name;
            Price = price;
            IsVegetarian = isVegetarian;
        }

        public static void CreateMenu(List<Food> menu)
        {
            menu.Add(new Food("Vegetarian 1", 123, true));
            menu.Add(new Food("Vegetarian 2", 123, true));
            menu.Add(new Food("Vegetarian 3", 123, true));
            menu.Add(new Food("Meat 1", 123, false));
            menu.Add(new Food("Meat 2", 123, false));
            menu.Add(new Food("Meat 3", 123, false));
            menu.Add(new Food("Fish 1", 123, false));
            menu.Add(new Food("Fish 2", 123, false));
            menu.Add(new Food("Fish 3", 123, false));
        }
    }
}
