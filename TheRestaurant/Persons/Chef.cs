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
        public int CompetenceLevel { get; set; }

        public bool HasOrder { get; set; }
        public Dictionary<string, List<Food>> Order { get; set; } = new();

        public Chef(Random random)
        {
            CompetenceLevel = random.Next(1, 6);
        }
    }
}
