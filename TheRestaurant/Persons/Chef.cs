﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Persons
{
    internal class Chef : Person
    {
        // Ärver Name och counter från Person
        public bool IsCompetent { get; set; } // ev ändra till en int, som foodpoints

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
