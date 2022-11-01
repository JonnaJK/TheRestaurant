using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Interface
{
    internal interface IFood // ev ta bort interface och gör en basklass för Food, alt ta bort subklasserna som ärver IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
    }
}
