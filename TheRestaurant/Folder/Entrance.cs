using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Person;

namespace TheRestaurant.Folder
{
    internal class Entrance
    {
        public List<List<Guest>> GroupOfGuests { get; set; }

        public Entrance()
        {
            GroupOfGuests = new List<List<Guest>>();
        }

    }
}
