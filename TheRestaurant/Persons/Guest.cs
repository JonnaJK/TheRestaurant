using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRestaurant.Persons
{
    internal class Guest : Person
    {
        public int Money { get; set; }
        public int Satisfied { get; set; }


        public Guest(Random random)
        {
            Money = random.Next(100, 1000);
        }


        internal static void ChooseGuests(int amountOfGuests, Random random, List<List<Guest>> groups)
        {
            while (amountOfGuests > 0)
            {
                List<Guest> group = new();

                if (amountOfGuests >= 4)
                {
                    int number = random.Next(1, 5);
                    for (int i = 0; i < number; i++)
                    {
                        group.Add(new Guest(random));
                    }
                    amountOfGuests -= number;
                }
                else
                {
                    for (int i = amountOfGuests; i > 0; i--)
                    {
                        group.Add(new Guest(random));
                    }
                    amountOfGuests = 0;
                }

                groups.Add(group);
            }
        }
    }
}
