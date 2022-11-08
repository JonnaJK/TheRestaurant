using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Persons;

namespace TheRestaurant
{
    internal class GUI
    {
        internal static void DrawRestaurant(Entrance entrance)
        {
            DrawAnyList("Entrance", 0, 0, entrance.GroupOfGuests);
        }
        public static void Draw(string header, int fromLeft, int fromTop, string[] graphics)
        {
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
    }
}
