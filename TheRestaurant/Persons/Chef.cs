using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;

namespace TheRestaurant.Persons
{
    internal class Chef : Person
    {
        // Ärver Name och counter från Person
        public bool IsCompetent { get; set; } // ev ändra till en int, som foodpoints

        public bool HasOrder { get; set; }
        public Dictionary<string, List<Food>> Order { get; set; } = new();

        public Chef(Random random)
        {
            int probability = random.Next(101);
            if (probability > 50)
            {
                IsCompetent = true;
            }
        }
    }
}
