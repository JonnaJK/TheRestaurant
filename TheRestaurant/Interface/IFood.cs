using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;

namespace TheRestaurant.Interface
{
    internal interface IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
        public bool IsVegetarian { get; set; }
    }
}
