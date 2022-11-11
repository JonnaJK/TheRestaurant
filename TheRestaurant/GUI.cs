using TheRestaurant.Persons;
using TheRestaurant.ThingsInRestaurant;

namespace TheRestaurant
{
    internal class GUI
    {
        private static int _width = 0;

        // Build Entrance and calls the method for drawing tables
        internal static void DrawRestaurant(Restaurant restaurant, Entrance entrance)
        {
            DrawRestaurantBorders();
            DrawAnyList("Entrance", 0, 0, entrance.GroupOfGuests);
            DrawAnyList("Kitchen", 0, 20, restaurant.Kitchen.Chefs);

            int fromLeft = 26;
            int fromTop = 3;
            int counter = 0;
            int brakeRow = 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < brakeRow; j++)
                {
                    if (restaurant.Tables[counter].EatingFoodCounter > 0 && restaurant.Tables[counter].Waiter.Name == "")
                    {
                        Draw(restaurant.Tables[counter].Name, restaurant.Tables[counter].EatingFoodCounter.ToString(), fromLeft, fromTop, restaurant.Tables[counter].Guests, restaurant.Tables[counter].Small);                       
                    }
                    else
                    {
                        Draw(restaurant.Tables[counter].Name, restaurant.Tables[counter].Waiter.Name, fromLeft, fromTop, restaurant.Tables[counter].Guests, restaurant.Tables[counter].Small);
                    }
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
                    fromLeft = 26;
                }
            }
            int width1 = 0;
            int width2 = 0;
            for (int i = 0; i < entrance.GroupOfGuests.Count; i++)
            {
                for (int j = 0; j < entrance.GroupOfGuests[i].Count; j++)
                {
                    if (entrance.GroupOfGuests[i][j].Name.Length > width1)
                    {
                        width1 = entrance.GroupOfGuests[i][j].Name.Length;
                    }
                }
            }
            for (int i = 0; i < restaurant.Kitchen.Chefs.Count; i++)
            {
                if (restaurant.Kitchen.Chefs[i].Name.Length > width2)
                {
                    width2 = restaurant.Kitchen.Chefs[i].Name.Length;
                }
            }
            var availableWaiters = restaurant.Waiters.Where(x => !x.CleaningTable).ToList();

            for (int i = 0; i < availableWaiters.Count; i++)
            {
                Waiter waiter = availableWaiters[i];
                if (waiter.HasOrder == false && waiter.HasFoodToDeliver == false)
                {
                    Console.SetCursorPosition(width1 + 8, 3 + i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(waiter.Name);
                }
                else if (waiter.HasOrder == true || waiter.HasFoodToDeliver == true)
                {
                    Console.SetCursorPosition(width2 + 5, 22 + i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(waiter.Name);
                }
            }
            Console.ResetColor();

            foreach (Table table in restaurant.Tables)
            {
                if (table.Receipt.Count > 0)
                {
                    DrawActionList("Receipt", 0, 28, table.Receipt);
                }
            }
        }

        // Draws the border of the restaurant.
        public static void DrawRestaurantBorders()
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

        // Build tables, used in DrawRestaurant
        public static void Draw(string header, string footer, int fromLeft, int fromTop, List<Guest> guests, bool isSmall)
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
            if (footer != "")
            {
                Console.Write('└' + " ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(footer);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new String('─', width - footer.Length) + '┘');
            }
            else
            {
                Console.Write('└' + new String('─', width + 2) + '┘');
            }
        }

        // Build the actual entrance, used in DrawAnylist
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

        // Make building ENTRANCE possible by converting list in list to array, used in DrawRestaurant
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
            Draw(header, fromLeft, fromTop, graphics);
        }

        // Make building KITCHEN possible by converting list to array, used in Drawrestaurant
        public static void DrawAnyList<T>(string header, int fromLeft, int fromTop, List<T> anyList)
        {
            string[] graphics = new string[anyList.Count];
            int i = 0;

            foreach (var chef in anyList)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (chef is Chef c)
                {
                    graphics[i] = c.Name;
                    i++;
                }
            }
            Console.ResetColor();
            Draw(header, fromLeft, fromTop, graphics);
        }

        // Build the actionList
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
