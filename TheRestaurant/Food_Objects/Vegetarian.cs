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
        public bool Veg { get; set; } = true;
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
            Lactose = true;
        }
    }

    internal class Falafel : Vegetarian
    {
        public Falafel()
        {
            Name = "Falafel";
            Price = 129;
            Gluten = true;
        }
    }
}
