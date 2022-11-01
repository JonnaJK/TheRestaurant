﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Table
    {
        public List<Guest> Guests { get; set; } = new List<Guest> { };
        public int Number { get; set; }
        public string Name { get; set; }
        public bool Small { get; set; }
        public int Score { get; set; }

        public Table(Random random, string name, bool small)
        {
            Name = name;
            Score = random.Next(6);
            Small = small;

        }
    }
}
