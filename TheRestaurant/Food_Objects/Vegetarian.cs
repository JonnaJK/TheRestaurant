using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food_Objects
{
    internal class Vegetarian : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Lactose { get; set; }
        public bool Gluten { get; set; }
        public bool FoodIsVego { get; set; }
    }

    internal class Mjölk : Vegetarian
    {
        public Mjölk()
        {
            Name = "Mjölk";
            Price = 123;
        }
    }
    internal class Gräs : Vegetarian
    {
        public Gräs()
        {
            Name = "Gräs";
            Price = 456;
            Lactose = true;
            Gluten = true;
        }
    }
    internal class Bönor : Vegetarian
    {
        public Bönor()
        {
            Name = "Bönor";
            Price = 789;
            FoodIsVego = true;
        }
    }
}
