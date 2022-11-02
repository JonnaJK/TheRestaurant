using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;

namespace TheRestaurant.Persons
{
    internal class Waiter : Person
    {
        public List<Food> Order { get; set; } = new List<Food>();
        


    }
}
