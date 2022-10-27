using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Person_Objects
{
    internal class Chef : IPerson
    {
        public string Name { get; set; }
        public int Counter { get; set; }
        public bool IsCompetent { get; set; } // ev ändra till en int, som foodpoints

        public Chef(Random random)
        {
            int probability = random.Next(100);
            if (probability < 70)
            {
                IsCompetent = true;
            }
        }
    }
}
