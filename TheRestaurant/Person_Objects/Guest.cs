using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person_Objects
{
    internal class Guest : Person
    {
        public int Money { get; set; }
        public bool Vegetarian { get; set; }
        public int Satisfied { get; set; } 
        public bool LactoseIntolerant { get; set; }
        public bool GlutenIntolerant { get; set; }

        public Guest(Random random)
        {

            Money = random.Next(100, 1000);

            int probability = random.Next(100);
            if(probability < 30)
            {
                Vegetarian = true;
            }

            probability = random.Next(100);
            if (probability < 20)
            {
                LactoseIntolerant = true;
            }

            probability = random.Next(100);
            if (probability < 5)
            {
                GlutenIntolerant = true;
            }


        }
    }

}
