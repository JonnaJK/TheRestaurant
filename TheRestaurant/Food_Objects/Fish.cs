using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food_Objects
{
    internal class Fish : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Lactose { get; set; }
        public bool Gluten { get; set; }
        public bool FoodIsVego { get; set; }
    }

    internal class Torsk : Fish
    {
        public Torsk()
        {
            Name = "Torsk";
            Price = 123;
        }
    }
    internal class Lax : Fish
    {
        public Lax()
        {
            Name = "Lax";
            Price = 456;
            Lactose = true;
            Gluten = true;
        }
    }
    internal class Strömming : Fish
    {
        public Strömming()
        {
            Name = "Strömming";
            Price = 789;
            FoodIsVego = true;
        }
    }
}
