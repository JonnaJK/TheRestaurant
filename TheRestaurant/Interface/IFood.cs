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
    }
}
