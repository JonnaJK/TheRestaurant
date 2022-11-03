﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Foods;
using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Table
    {
        public List<Guest> Guests { get; set; } = new List<Guest> { };
        public List<Food> Order { get; set; } = new List<Food> { };
        public int Number { get; set; }
        public string Name { get; set; }
        public bool Small { get; set; }
        public bool IsDirty { get; set; }
        public bool Occupied { get; set; }
        public bool HasOrdered { get; set; }
        public bool WaitingForFood { get; set; }
        public int Score { get; set; }

        public Table(Random random, string name, bool small)
        {
            Name = name;
            Score = random.Next(6);
            Small = small;

        }


        internal static void Create(Random random, List<Table> tables, bool small, int amount)
        {
            int number = tables.Count;
            for (int i = number + 1; i <= amount + number; i++)
            {
                string name = "Table " + i;
                tables.Add(new Table(random, name, small));
            }
        }


    }
}
