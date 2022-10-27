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
        public bool Veg { get; set; }
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
            Lactose = true;
        }
    }
    internal class FishAndChips : Fish
    {
        public FishAndChips()
        {
            Name = "Fish and chips";
            Price = 139;
            Gluten = true;
        }
    }
}
