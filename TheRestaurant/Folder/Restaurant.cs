using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;
using TheRestaurant.Persons;
using TheRestaurant.Foods;

namespace TheRestaurant.Folder
{
    internal class Restaurant
    {
        public List<Table> Tables { get; set; } = new List<Table>();
        //public List<Dictionary<string, Table>> Tables { get; set; }
        public Kitchen Kitchen { get; set; } = new Kitchen();
        //public Entrance Entrance { get; set; } = new Entrance();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public List<Chef> Chefs { get; set; } = new List<Chef>();
        public List<Food> Menu { get; set; } = new();

        private readonly Random _random = new Random();
        private Entrance entrance = new();


        public void Run()
        {
            CreateRestaurant();
            Food.CreateMenu(Menu);

            while (true)
            {
                MatchTableForGuests();


                CheckIfHasOrdered();



                Console.ReadKey();
            }

        }

        private void CheckIfHasOrdered()
        {
            foreach (Table table in Tables)
            {
                if (table.HasOrdered is false)
                {
                    foreach (var guest in table.Guests)
                    {
                        TakeOrder(guest, _random);
                    }
                    break;
                }
            }
        }

        private void TakeOrder(Guest guest, Random random)
        {
            if (guest.IsVegetarian)
            {
                var vegetarianFood = Menu.Where(x => x.IsVegetarian).ToList();
                int index = random.Next(vegetarianFood.Count);

            }
        }


        // Check for suitable table for party of guests
        private void MatchTableForGuests()
        {
            if (entrance.GroupOfGuests[0].Count <= 2)
            {
                var smallTableList = Tables.Where(x => x.Small).ToList();
                if (smallTableList.Count > 0) { PlaceAtTable(smallTableList); }
            }
            else
            {
                var bigTableList = Tables.Where(x => x.Small == false).ToList();
                if (bigTableList.Count > 0) { PlaceAtTable(bigTableList); }
            }
        }

        // Place guests at available table
        private void PlaceAtTable(List<Table> tables)
        {

            foreach (Table table in tables)
            {
                if (table.Occupied == false)
                {

                    table.Guests.AddRange(entrance.GroupOfGuests[0]);
                    entrance.GroupOfGuests.RemoveAt(0);
                    table.Occupied = true;
                    break;
                }

            }
        }
        private void CreateRestaurant()
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
