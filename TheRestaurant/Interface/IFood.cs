using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Interface
{
    internal interface IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Lactose { get; set; }
        public bool Gluten { get; set; }
        public bool Veg { get; set; }
    }
}
