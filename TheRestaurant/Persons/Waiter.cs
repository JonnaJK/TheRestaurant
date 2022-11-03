﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Foods;

namespace TheRestaurant.Persons
{
    internal class Waiter : Person
    {
        //public List<Food> Order { get; set; } = new List<Food>();

        // IF THIS DOESNT WORK, use string insted of table.
        public Dictionary<string, List<Food>> Order { get; set; } = new();
        public bool HasOrder { get; set; }

    }
}
