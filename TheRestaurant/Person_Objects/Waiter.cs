using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person_Objects
{
    internal class Waiter : IPerson
    {
        // ev prop med Guest list, ifall waitern måste "visa" dem till bordet, annars kan de "hoppa" från entren 
        // ev prop med Food list, ifall waitern måste bära maten från köket till bordet, annars kan maten "hoppa" från köket
        public string Name { get; set; }
        public int Counter { get; set; }

    }
}
