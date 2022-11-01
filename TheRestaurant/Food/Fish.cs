using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food
{
    internal class Fish : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
    }

    internal class Oyster : Fish
    {
        public Oyster()
        {
            Name = "Oysters";
            Price = 279;
        }
    }
    internal class RainbowSalmon : Fish
    {
        public RainbowSalmon()
        {
            Name = "Rainbow salmon";
            Price = 199;
        }
    }
    internal class FishAndChips : Fish
    {
        public FishAndChips()
        {
            Name = "Fish and chips";
            Price = 139;
        }
    }
}
