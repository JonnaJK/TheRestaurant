using System;
using TheRestaurant.Foods;
using TheRestaurant.ThingsInRestaurant;

namespace TheRestaurant.Persons
{
    internal class Waiter : Person
    {
        public Dictionary<string, List<Food>> InOrder { get; set; } = new();
        public Dictionary<string, List<Food>> OutOrder { get; set; } = new();
        public bool HasOrder { get; set; }
        public bool HasFoodToDeliver { get; set; }
        public bool CleaningTable { get; set; }
        public int ServiceScore { get; set; }
        public int CursorPos { get; set; }
        private readonly int _timeToCleanTable = 3;
        public Waiter(Random _random, string name, int cursorPos)
        {
            Name = name;
            ServiceScore = _random.Next(1, 6);
            CursorPos = cursorPos;
        }

        internal static void DropOffFood(Waiter waiter, List<Table> tables)
        {
            // Leave food at table
            foreach (KeyValuePair<string, List<Food>> foods in waiter.OutOrder)
            {
                // resultTable becomes same as table object that we want to find
                // Addrange line adds foodlist to tableOrder
                var resultTable = tables.Single(table => table.Name == foods.Key);
                resultTable.Order = new();
                resultTable.Order.AddRange(foods.Value);
                resultTable.ServiceScore += waiter.ServiceScore;
                waiter.OutOrder = new();
                waiter.HasOrder = false;
                resultTable.WaitingForFood = false;

                Console.SetCursorPosition(105, waiter.CursorPos);
                Console.WriteLine(waiter.Name + " places food at " + resultTable.Name + "                                                 ");
            }
        }

        // Place guests at available table
        internal static void PlaceGuestsAtTable(int i, List<Table> tables, List<List<Guest>> groupOfGuests)
        {
            foreach (Table table in tables)
            {
                table.Guests.AddRange(groupOfGuests[i]);
                groupOfGuests.RemoveAt(i);
                table.Occupied = true;
                table.Receipt = new();
                break;
            }
        }

        internal static void CheckIfHasOrdered(Waiter waiter, List<Table> tablesWaitingToOrder, Random random)
        {
            foreach (Table table in tablesWaitingToOrder)
            {
                foreach (var guest in table.Guests)
                {
                    TakeOrder(guest, random, table);
                }
                waiter.InOrder.Add(table.Name, table.Order);
                table.ServiceScore += waiter.ServiceScore;
                table.HasOrdered = true;
                table.WaitingForFood = true;
                waiter.HasOrder = true;

                Console.SetCursorPosition(105, waiter.CursorPos);
                Console.WriteLine(waiter.Name + " escorted the guests to " + table.Name + " and takes their order.                                          ");
                break;
            }
        }

        // Check for vegetarians, they dont eat meat or fish. The rest can eat anything. Waiter takes order
        private static void TakeOrder(Guest guest, Random random, Table table)
        {
            int index = 0;
            if (guest.IsVegetarian)
            {
                var vegetarianFood = table.Menu.Where(food => food.IsVegetarian).ToList();
                index = random.Next(vegetarianFood.Count);
                table.Order.Add(vegetarianFood[index]);
                guest.MyMeal = vegetarianFood[index];
            }
            else
            {
                index = random.Next(table.Menu.Count);
                table.Order.Add(table.Menu[index]);
                guest.MyMeal = table.Menu[index];
            }
        }

        internal static void StartCleaning(Table table, Waiter waiter)
        {
            table.Guests = new();
            waiter.CleaningTable = true;
            table.EatingFoodCounter = 0;
            table.Waiter = waiter;
        }

        internal void CleanTable(Waiter waiter, List<Table> tables, Random random)
        
        {
            var result = tables.Where(x => x.Waiter == waiter).FirstOrDefault();

            if (result is not null)
            {
                waiter.Counter++;

                if (waiter.Counter >= _timeToCleanTable)
                {
                    result.IsDirty = false;
                    result.Occupied = false;
                    waiter.CleaningTable = false;
                    result.HasOrdered = false;
                    result.Order = new();
                    result.Waiter = new(random, "", 0);
                    result.Actions = "";
                    waiter.Counter = 0;
                    result.ServiceScore = 0;
                    result.OverallScore = 0;
                    result.WaitingTimeScore = 0;
                    result.EatingFoodCounter = 0;
                }
            }
        }
    }
}
