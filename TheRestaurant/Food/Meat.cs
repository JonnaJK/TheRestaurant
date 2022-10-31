using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food
{
    internal class Meat : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    internal class PastaBolgonese : Meat
    {
        public PastaBolgonese()
        {
            Name = "Pasta bolgonese";
            Price = 169;
        }
    }
    internal class FilétMignon : Meat
    {
        public FilétMignon()
        {
            Name = "Filét mignon";
            Price = 399;
        }
    }
    internal class Hamburger : Meat
    {
        public Hamburger()
        {
            Name = "Prime rib hamburger";
            Price = 219;
        }
    }
}
