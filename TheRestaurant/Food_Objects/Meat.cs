using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Food_Objects
{
    internal class Meat : IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Lactose { get; set; }
        public bool Gluten { get; set; }
        public bool Veg { get; set; }
    }

    internal class Köttbullar : Meat
    {
        public Köttbullar()
        {
            Name = "Köttbullar";
            Price = 123;
        }
    }
    internal class Disktrasa : Meat
    {
        public Disktrasa()
        {
            Name = "Disktrasa";
            Price = 456;
            Lactose = true;
            Gluten = true;
        }
    }
    internal class Skosula : Meat
    {
        public Skosula()
        {
            Name = "Skosula";
            Price = 789;
            Veg = true;
        }
    }
}
