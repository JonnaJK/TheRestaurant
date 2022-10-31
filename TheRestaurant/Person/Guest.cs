using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person
{
    internal class Guest : Person
    {
        public int Money { get; set; }
        public int Satisfied { get; set; }


        public Guest(Random random)
        {

            Money = random.Next(100, 1000);

        }
    }
}
