using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person_Objects
{
    internal class Guest : IPerson
    {
        public string Name { get; set; }
        public int Counter { get; set; }  
        public int Money { get; set; }
        public bool Vegetarian { get; set; }
        public int Satisfied { get; set; } 
        public List<bool> Allergies { get; set; } = new List<bool>();
        public bool LactoseIntolerant { get; set; }
        public bool GlutenIntolerant { get; set; }

        public Guest(Random random)
        {
            // göra en metod eller i IPerson, för att slumpa namn, ev Helpers.cs? 

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

            Allergies.Add(LactoseIntolerant);
            Allergies.Add(GlutenIntolerant);
        }
    }

}
