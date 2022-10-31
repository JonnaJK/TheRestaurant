using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food
{
    internal class Vegetarian : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    internal class QuinoaSalad : Vegetarian
    {
        public QuinoaSalad()
        {
            Name = "Quinoa salad";
            Price = 149;
        }
    }

    internal class TomatoSoup : Vegetarian
    {
        public TomatoSoup()
        {
            Name = "Tomato soup";
            Price = 119;
        }
    }

    internal class Falafel : Vegetarian
    {
        public Falafel()
        {
            Name = "Falafel";
            Price = 129;
        }
    }
}
