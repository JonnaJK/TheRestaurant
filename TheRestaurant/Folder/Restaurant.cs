using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Restaurant
    {
        public List<Table> Tables { get; set; } = new List<Table>();
        public Kitchen Kitchen { get; set; } = new Kitchen();
        //public Entrance Entrance { get; set; } = new Entrance();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public List<Chef> Chefs { get; set; } = new List<Chef>();

        private readonly Random _random = new Random();
        private Entrance entrance = new();


        public void Run()
        {
            // Creates guests and placed them in groups, a list of lists
            Guest.ChooseGuests(80, _random, entrance.GroupOfGuests);

            // Creates waiters in restaurant
            Person.Create(_random, Waiters, 3);
            // Creates chefs in kitchen
            Person.Create(_random, Kitchen.Chefs, 5);

            // Creates small and big tables
            Table.Create(_random, Tables, true, 5);
            Table.Create(_random, Tables, false, 5);
        }

        private void CheckPlaceGuest()
        {
            
            foreach (Table table in Tables)
            {
                if (entrance.GroupOfGuests.Count <= 2)
                {
                    if (table.Occupied == false && table.Small == true)
                    {
                        table.Guests.AddRange(entrance.GroupOfGuests[0]);
                        entrance.GroupOfGuests.RemoveAt(0);
                        table.Occupied = true;
                        break;
                    }
                }
                else if (entrance.GroupOfGuests.Count > 2)
                {
                    if (table.Occupied == false && table.Small == false)
                    {
                        table.Guests.AddRange(entrance.GroupOfGuests[0]);
                        entrance.GroupOfGuests.RemoveAt(0);
                        table.Occupied = true;
                        break;
                    }
                }
            }
        }


        public static void DrawRestaurant()
        {

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("┌" + "".PadRight(100, '─') + "┐");

            for (int rows = 0; rows < 25; rows++)
            {
                Console.Write("│");
                for (int cols = 0; cols < 100; cols++)
                {
                    Console.Write(" ");
                }
                Console.Write("│");
                Console.WriteLine();
            }
            Console.WriteLine("└" + "".PadRight(100, '─') + "┘");

        }
    }
}
