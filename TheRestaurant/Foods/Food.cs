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
        public int QualityScore { get; set; }
        public bool IsVegetarian { get; set; }

        public Food(string name, int price, bool isVegetarian)
        {
            Name = name;
            Price = price;
            IsVegetarian = isVegetarian;
        }

        public static void CreateMenu(List<Food> menu)
        {
            menu.Add(new Food("Vegetarian mushroom stroganoff with pumpkin", 229, true));
            menu.Add(new Food("Vegetarian curries", 189, true));
            menu.Add(new Food("Vegetarian beetroot pie", 219, true));
            menu.Add(new Food("Whole roast beef tenderloin", 279, false));
            menu.Add(new Food("Pork filet stew", 249, false));
            menu.Add(new Food("Fried sirloin steak", 199, false));
            menu.Add(new Food("Salmon fillet in the oven with lemon", 239, false));
            menu.Add(new Food("Breaded cod", 123, false));
            menu.Add(new Food("Pestobaked saithe with carrot salad", 179, false));
        }
    }
}
