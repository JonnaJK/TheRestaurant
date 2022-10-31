using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Person;

namespace TheRestaurant
{
    internal class Helpers
    {
        public static string GetName()
        {
            Random random = new Random();
            int index;
            string name;

            List<string> Surnames = new();
            string[] string_surnames =
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones",
                "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
                "Hernandez", "Lopez", "Gonzales", "Wilson", "Anderson",
                "Thomas", "Taylor", "Moore", "Jackson", "Martin",
                "Lee", "Perez", "Thompson", "White", "Harris",
                "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright",
                "Scott", "Torres", "Nguyen", "Hill", "Flores",
                "Green", "Adams", "Nelson", "Baker", "Hall",
                "Rivera", "Campbell", "Mitchell", "Carter", "Gomez",
                "Roberts", "Philips", "Evans", "Turner", "Diaz",
                "Parker", "Cruz", "Edwards", "Collins", "Reyes",
                "Stewart", "Morris", "Morales", "Murphy", "Cook",
                "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
                "Peterson", "Bailey", "Reed", "Kelly", "Howard",
                "Ramos", "Kim", "Cox", "Ward", "Richardson",
                "Watson", "Brooks", "Chavez", "Wood", "James",
                "Bennet", "Gray", "Mendoza", "Ruiz", "Hughes",
                "Price", "Alvarez", "Castillo", "Sanders", "Patel",
                "Myers", "Long", "Ross", "Foster", "Jimenez"
            };
            Surnames.AddRange(string_surnames);

            index = random.Next(Surnames.Count);
            name = Surnames[index];

            return name;
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


        public static void DrawAnyList<T>(string header, int fromLeft, int fromTop, List<T> anyList)
        {
            string[] graphics = new string[anyList.Count];

            for (int i = 0; i < anyList.Count; i++)
            {

                graphics[i] = (anyList[i] as Guest).Name;
                //if (anyList[i] is Guest)
                //{
                //    graphics[i] = (anyList[i] as Guest).Item;
                //}
                //if (anyList[i] is Movie)
                //{
                //    graphics[i] = (anyList[i] as Movie).Title + "(" + (anyList[i] as Movie).PlayingTime + " min)";
                //}
            }
            Helpers.Draw(header, fromLeft, fromTop, graphics);
        }
    }
}
