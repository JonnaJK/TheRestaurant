using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Persons;

namespace TheRestaurant
{
    internal class GUI
    {
        private static int _width = 0;
        internal static void DrawRestaurant(Restaurant restaurant, Entrance entrance)
        {
            DrawAnyList("Entrance", 0, 0, entrance.GroupOfGuests);
            int fromLeft = 25;
            int fromTop = 3;
            int counter = 0;
            int brakeRow = 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < brakeRow; j++)
                {
                    Draw(restaurant.Tables[counter].Name, fromLeft, fromTop, restaurant.Tables[counter].Guests, restaurant.Tables[counter].Small);
                    fromLeft += 25;
                    counter++;
                }
                fromLeft = 1;
                fromTop += 7;
                if (brakeRow == 3)
                {
                    brakeRow = 4;
                }
                else if (brakeRow == 4)
                {
                    brakeRow = 3;
                    fromLeft = 25;
                }
            }

        }
        public static void Draw(string header, int fromLeft, int fromTop, List<Guest> guests, bool isSmall)
        {
            int tableSize = 0;
            if (isSmall)
            {
                tableSize = 2;
            }
            else if (!isSmall)
            {
                tableSize = 4;
            }
            string[] graphics = new string[tableSize];
            //for (int i = 0; i < guests.Count; i++)
            //{
            //    graphics[i] = guests[i].Name;
            //}
            for (int i = 0; i < tableSize; i++)
            {
                if (guests.Count > 0)
                {
                    int amountOfGuests = guests.Count;
                    if (amountOfGuests < tableSize)
                    {
                        if (i == tableSize - 1)
                        {
                            graphics[i] = " ";
                        }
                        else
                        {
                            graphics[i] = guests[i].Name;
                        }
                    }
                    else
                    {
                        graphics[i] = guests[i].Name;
                    }
                }
                else
                    graphics[i] = " ";
            }

            int width = 0;

            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics != null && graphics[i].Length > width)
                {
                    width = graphics[i].Length;
                }
            }
            _width = width;

            if (width < header.Length + 4)
            { width = header.Length + 4; };

            Console.SetCursorPosition(fromLeft, fromTop);
            if (header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new String('─', width - header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new String('─', width + 2) + '┐');
            }
            Console.WriteLine();
            int maxRows = 0;
            int length = 0;
            if (isSmall)
            {
                length = 2;
            }
            else if (!isSmall)
            {
                length = 4;
            }
            for (int j = 0; j < length; j++)
            {
                Console.SetCursorPosition(fromLeft, fromTop + j + 1);
                Console.WriteLine('│' + " " + (graphics[j] == null ? " " : graphics[j]) + new String(' ', width - graphics[j].Length + 1) + '│');
                maxRows = j;
            }
            Console.SetCursorPosition(fromLeft, fromTop + maxRows + 2);
            Console.Write('└' + new String('─', width + 2) + '┘');
        }
        public static void Draw(string header, int fromLeft, int fromTop, string[] graphics)
        {
            int width = 0;

            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i] is null)
                {
                    graphics[i] = " ";
                }
                if (graphics[i].Length > width)
                {
                    width = graphics[i].Length;
                }
            }

            _width = width;

            if (width < header.Length + 4)
            { width = header.Length + 4; };

            Console.SetCursorPosition(fromLeft, fromTop);
            if (header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new String('─', width - header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new String('─', width + 2) + '┐');
            }
            Console.WriteLine();
            int maxRows = 0;
            for (int j = 0; j < graphics.Length; j++)
            {
                Console.SetCursorPosition(fromLeft, fromTop + j + 1);
                Console.WriteLine('│' + " " + graphics[j] + new String(' ', width - graphics[j].Length + 1) + '│');
                maxRows = j;
            }
            Console.SetCursorPosition(fromLeft, fromTop + maxRows + 2);
            Console.Write('└' + new String('─', width + 2) + '┘');
        }

        public static void DrawAnyList<T>(string header, int fromLeft, int fromTop, List<List<T>> anyList)
        {
            string[] graphics = new string[5];
            int i = 0;

            foreach (var group in anyList)
            {
                foreach (var guest in group)
                {
                    if (guest is Guest g)
                    {
                        graphics[i] = g.Name + " +" + (group.Count - 1);
                        i++;
                        break;
                    }
                }
                if (i >= 5)
                {
                    break;
                }
            }
            //for (int j = 0; j < anyList[i].Count; j++)
            //{
            //    if (anyList[j] is Guest)
            //    {
            //        graphics[i] = (anyList[j] as Guest).Name;

            //    }

            //}
            Draw(header, fromLeft, fromTop, graphics);
        }

        public static void DrawActionList(string header, int fromLeft, int fromTop, List<string> actionlist)
        {
            string[] graphics = actionlist.ToArray();


            int width = 0;
            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i].Length > width)
                {
                    width = graphics[i].Length;
                }
            }
            if (width < header.Length + 4)
            { width = header.Length + 4; };

            Console.SetCursorPosition(fromLeft, fromTop);
            if (header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new String('─', width - header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new String('─', width + 2) + '┐');
            }
            Console.WriteLine();
            int maxRows = 0;
            for (int j = 0; j < graphics.Length; j++)
            {
                Console.SetCursorPosition(fromLeft, fromTop + j + 1);
                Console.WriteLine('│' + " " + graphics[j] + new String(' ', width - graphics[j].Length + 1) + '│');
                maxRows = j;
            }
            Console.SetCursorPosition(fromLeft, fromTop + maxRows + 2);
            Console.Write('└' + new String('─', width + 2) + '┘');

        }
    }
}
