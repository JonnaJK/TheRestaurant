using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Interface;
using TheRestaurant.Persons;
using TheRestaurant.Foods;
using System.Diagnostics.Metrics;

namespace TheRestaurant.Folder
{
    internal class Restaurant
    {
        public List<Table> Tables { get; set; } = new List<Table>();
        public Kitchen Kitchen { get; set; } = new Kitchen();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        //public List<Chef> Chefs { get; set; } = new List<Chef>();
        public List<Food> Menu { get; set; } = new();

        private readonly Random _random = new Random();
        private Entrance entrance = new();


        public void Run()
        {
            CreateRestaurant();
            Food.CreateMenu(Menu);

            while (true)
            {
                foreach (Waiter waiter in Waiters)
                {
                    DropOfFood(waiter);
                    MatchTableForGuests();
                    CheckIfHasOrdered(waiter);
                    DropOffOrder(waiter);
                    PickUpFoodFromKitchen(waiter);
                }

                foreach (Chef chef in Kitchen.Chefs)
                {
                    CookFood(chef);
                }

                // If timer needed for table - place here
                foreach (Table table in Tables)
                {
                    ServiceTimer(table);
                    EatingTimer(table);
                }
                //Console.ReadKey();
            }
        }

        private void DropOfFood(Waiter waiter)
        {
            // Leave food at table
            if (waiter.OutOrder.Count > 0)
            {
                foreach (KeyValuePair<string, List<Food>> foods in waiter.OutOrder)
                {
                    // result becomes same as table object that we want to find
                    // Addrange line adds foodlist to tableOrder
                    var result = Tables.Single(table => table.Name == foods.Key);
                    result.Order.AddRange(foods.Value);
                    waiter.OutOrder.Clear();
                    waiter.HasOrder = false;
                }
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
                    // Skicka med en waiter från entré till bord
                    table.Guests.AddRange(entrance.GroupOfGuests[0]);
                    entrance.GroupOfGuests.RemoveAt(0);
                    table.Occupied = true;
                    break;
                }
            }
        }

        private void CheckIfHasOrdered(Waiter waiter)
        {
            foreach (Table table in Tables)
            {
                if (table.HasOrdered is false && table.Guests.Count > 0 && waiter.HasOrder is false)
                {
                    foreach (var guest in table.Guests)
                    {
                        TakeOrder(guest, _random, table);
                    }
                    waiter.InOrder.Add(table.Name, table.Order);
                    table.Order = new();
                    table.HasOrdered = true;
                    table.WaitingForFood = true;
                    waiter.HasOrder = true;
                    break;
                }
            }
        }

        // Check for vegetarians, they dont eat meat or fish. The rest can eat anything. Waiter takes order
        private void TakeOrder(Guest guest, Random random, Table table)
        {
            int index = 0;
            if (guest.IsVegetarian)
            {
                var vegetarianFood = Menu.Where(food => food.IsVegetarian).ToList();
                index = random.Next(vegetarianFood.Count);
                table.Order.Add(vegetarianFood[index]);
            }
            else
            {
                index = random.Next(Menu.Count);
                table.Order.Add(Menu[index]);
            }
        }

        // Put Clear() outside of the foreach, and give the waiter the ability
        // to take mulitple table orders to the kitchen at once.
        private void DropOffOrder(Waiter waiter)
        {
            if (waiter.HasOrder)
            {
                foreach (KeyValuePair<string, List<Food>> order in waiter.InOrder)
                {
                    Kitchen.InOrders.Add(order.Key, order.Value);
                    waiter.InOrder.Clear();
                    waiter.HasOrder = false;
                }
            }
        }

        // Waiter picks up food from kitchen
        private void PickUpFoodFromKitchen(Waiter waiter)
        {
            if (Kitchen.OutOrders.Count > 0 && waiter.HasOrder is false && waiter.HasFoodToDeliver is false)
            {
                foreach (KeyValuePair<string, List<Food>> order in Kitchen.OutOrders)
                {
                    waiter.OutOrder.Add(order.Key, order.Value);
                    Kitchen.OutOrders.Remove(order.Key);
                    break;  // If not break; one waiter takes every order from kitchen outorders (effective restaurant variant)
                }
            }
        }

        /// <summary>
        ///  Chef cooks meal for table for 10 turns and add points
        /// </summary>
        /// <param name="chef"></param> 
        private void CookFood(Chef chef)
        {
            //Chef takes order
            if (Kitchen.InOrders.Count > 0 && chef.HasOrder == false)
            {
                foreach (KeyValuePair<string, List<Food>> order in Kitchen.InOrders)
                {
                    chef.Order.Add(order.Key, order.Value);
                    Kitchen.InOrders.Remove(order.Key);
                    chef.HasOrder = true;
                    break;
                }
            }
            //Chef cooks for 10 turns
            if (chef.HasOrder == true)
            {
                chef.Counter++;
            }
            //Chef puts cooked food in outOrders (kitchen window)
            if (chef.Counter == 10)
            {
                foreach (KeyValuePair<string, List<Food>> order in chef.Order)
                {
                    //Give score to each food in order
                    foreach (Food food in order.Value)
                    {
                        food.QualityScore = chef.CompetenceLevel;
                    }
                    Kitchen.OutOrders.Add(order.Key, order.Value);
                    chef.Order.Clear();
                    chef.HasOrder = false;
                    chef.Counter = 0;                    
                }
            }
        }

        // TIMER
        private void ServiceTimer(Table table)
        {
            if (table.WaitingForFood == true)
            {
                // WaitingTimeScore++ now increases points longer the wait - backwardsthinking
                table.WaitingTimeScore++;
            }
        }

        // TIMER
        private void EatingTimer(Table table)
        {
            if (table.WaitingForFood == false && table.HasOrdered == true)
            {
                // WaitingTimeScore++ now increases points longer the wait - backwardsthinking
                table.EatingFoodCounter++;
            }
            if (table.EatingFoodCounter == 20)
            {
                // CheckoutMETHOD here (Table table)
            }
        }

        // Create Persons and Tables
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

        // GUI
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
