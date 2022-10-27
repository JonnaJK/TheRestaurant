using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person_Objects
{
    interface IPerson
    {
        public string Name { get; set; }
        public int Counter { get; set; }
    }
}
