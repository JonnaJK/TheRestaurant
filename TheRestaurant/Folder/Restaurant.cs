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
        public List<Table> Tables { get; set; } = new List<Table>(10);
        public Entrance Entrance { get; set; } = new Entrance();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public List<Chef> Chefs { get; set; } = new List<Chef>();

        private readonly Random _random = new Random();

        //public Restaurant(int smallTables, int bigTables)
        //{
            
        //}

        public void Run()
        {
            // Creates guests and placed them in groups, a list of lists
            Guest.ChooseGuests(80, _random, Entrance.GroupOfGuests);

            // Creates chefs and waiters in restaurant
            Person.Create(_random, Chefs, 5);
            Person.Create(_random, Waiters, 3);
        }

        private void CheckFreeTable()
        {

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
