using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Person;

namespace TheRestaurant.Folder
{
    internal class Restaurant
    {
        private readonly Random _random = new Random();
        public List<Table> Tables { get; set; } = new List<Table>(10);
        public Entrance Entrance { get; set; } = new Entrance();


        public void Run()
        {
            //Guest guest = new Guest(_random);
            Guest.ChooseGuests(80, _random, Entrance.GroupOfGuests);

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
