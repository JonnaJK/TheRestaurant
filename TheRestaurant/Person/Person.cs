using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;

namespace TheRestaurant.Person
{
    public class Person : IPerson
    {
        public string Name { get; set; }
        public int Counter { get; set; }

        public Person()
        {
            Name = Helpers.GetName();

        }

    }
}
