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
using System.ComponentModel.Design;
using System.Net.WebSockets;

namespace TheRestaurant.Folder
{
    internal class Restaurant
    {
        public List<Table> Tables { get; set; } = new List<Table>();
        public Kitchen Kitchen { get; set; } = new Kitchen();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public double CashRegister { get; set; }
        public double TipJar { get; set; }
        private readonly Random _random = new Random();
        private Entrance entrance = new();
        internal readonly int _timeToCookFood = 10;
        internal readonly int _timeToEatFood = 20;
        internal readonly int _timeToCleanTable = 3;



        public void Run(Restaurant restaurant)
        {
            CreateRestaurant();
            DrawRestaurant();

            while (true)
            {
                GUI.DrawRestaurant(entrance);
                foreach (Waiter waiter in Waiters)
                {
                    var freeTables = Tables.Where(x => !x.Occupied).ToList();
                    if (waiter.CleaningTable)
                    {
                        CleanTable(waiter);
                    }
                    else
                    {
                        if (waiter.OutOrder.Count > 0)
                        {
                            DropOfFood(waiter);
                        }
                        else if (entrance.GroupOfGuests.Count is not 0 && freeTables.Count > 0)
                        {
                            MatchTableForGuests(freeTables);
                        }
                        else if (waiter.HasOrder)
                        {
                            DropOffOrder(waiter);
                        }
                        else if (Kitchen.OutOrders.Count > 0 && waiter.HasOrder is false && waiter.HasFoodToDeliver is false)
                        {
                            PickUpFoodFromKitchen(waiter);
                        }
                        else
                        {
                            CheckIfHasOrdered(waiter);
                        }
                    }
                }

                foreach (Chef chef in Kitchen.Chefs)
                {
                    CookFood(chef);
                }

                // If timer needed for table - place here
                foreach (Table table in Tables)
                {
                    ServiceTimer(table);
                    EatingTimer(table, restaurant);
                }
                //Console.ReadKey();
                // KOM IHÅG ATT TA BORT ALLA HÅRDKODADE TAL

                //Actionlist();

            }
        }

        private void Actionlist()
        {
            foreach (Table table in Tables)
            {
                Console.WriteLine("Namn: " + table.Name);
                Console.WriteLine("Order: " + String.Join(", ", table.Order));
                Console.WriteLine("Ockupado: " + table.Occupied);
                Console.WriteLine("Tid att äta mat: " + table.EatingFoodCounter);
                Console.WriteLine("Bordet har beställt: " + table.HasOrdered);
                Console.WriteLine("Smutsigt bord: " + table.IsDirty);
                Console.WriteLine("Totalpoäng för bordet: " + table.OverallScore);
                Console.WriteLine("WaitingTimeScore: " + table.WaitingTimeScore);
                Console.WriteLine("Waiting for food: " + table.WaitingForFood);
                Console.WriteLine("ServiceScore: " + table.ServiceScore);
                Console.WriteLine("antal gäster i sällskap: " + table.Guests.Count);
                Console.WriteLine(String.Join(", ", table.Actions));
                Console.WriteLine("--------------------------------");
                //foreach (Guest guest in table.Guests)
                //{

                //    Console.WriteLine();
                //    Console.WriteLine("Namn på gäst: " + guest.Name);
                //    Console.WriteLine("Pengar: " + guest.Money);
                //    Console.WriteLine("Vegetarian: " + guest.IsVegetarian);
                //    Console.WriteLine("Poäng/procent dricks: " + guest.Score);
                //    Console.WriteLine("Måltid: " + guest.MyMeal);
                //    Console.WriteLine("Pris på måltid: " + guest.MyMeal);
                //    Console.WriteLine("Dricks för måltid: " + guest.Tips);

                //}
            }
            Console.WriteLine("1:a sällskapet på tur: " + entrance.GroupOfGuests[0].Count);
            Console.WriteLine("2:a sällskapet på tur: " + entrance.GroupOfGuests[1].Count);
            Console.WriteLine("3:a sällskapet på tur: " + entrance.GroupOfGuests[2].Count);
            Console.WriteLine("4:a sällskapet på tur: " + entrance.GroupOfGuests[3].Count);
            Console.WriteLine("\nKassaregister: " + CashRegister);
            Console.WriteLine("TipJar: " + TipJar);
            Console.ReadKey();
            Console.Clear();
        }

        private void DropOfFood(Waiter waiter)
        {
            // Leave food at table
            foreach (KeyValuePair<string, List<Food>> foods in waiter.OutOrder)
            {
                // resultTable becomes same as table object that we want to find
                // Addrange line adds foodlist to tableOrder
                var resultTable = Tables.Single(table => table.Name == foods.Key);
                resultTable.Order.AddRange(foods.Value);
                resultTable.ServiceScore += waiter.ServiceScore;
                waiter.OutOrder = new();
                waiter.HasOrder = false;
                resultTable.WaitingForFood = false;
            }
        }

        // Check for suitable table for party of guests
        private void MatchTableForGuests(List<Table> freeTables)
        {
            var smallTableList = freeTables.Where(x => x.Small).ToList();
            var bigTableList = freeTables.Where(x => x.Small == false).ToList();

            for (int i = 0; i < entrance.GroupOfGuests.Count; i++)
            {
                if (entrance.GroupOfGuests[i].Count <= 2)
                {

                    if (smallTableList.Count > 0)
                    {
                        PlaceAtTable(i, smallTableList);
                        break;
                    }
                }
                else
                {
                    if (bigTableList.Count > 0)
                    {
                        PlaceAtTable(i, bigTableList);
                        break;
                    }
                }
            }
        }

        // Place guests at available table
        private void PlaceAtTable(int i, List<Table> tables)
        {
            foreach (Table table in tables)
            {
                // Skicka med en waiter från entré till bord
                table.Guests.AddRange(entrance.GroupOfGuests[i]);
                entrance.GroupOfGuests.RemoveAt(i);
                table.Occupied = true;
                break;
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
                    table.ServiceScore += waiter.ServiceScore;
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

        // Put new(); outside of the foreach, and give the waiter the ability
        // to take mulitple table orders to the kitchen at once.
        private void DropOffOrder(Waiter waiter)
        {
            foreach (KeyValuePair<string, List<Food>> order in waiter.InOrder)
            {
                Kitchen.InOrders.Add(order.Key, order.Value);
                waiter.InOrder = new();
                waiter.HasOrder = false;
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
            //Chef cooks for TimeToCookFood(10) turns 
            if (chef.HasOrder == true)
            {
                chef.Counter++;
            }
            //Chef puts cooked food in outOrders (kitchen window)
            if (chef.Counter == _timeToCookFood)
            {
                foreach (KeyValuePair<string, List<Food>> order in chef.Order)
                {
                    //Give score to each food in order
                    foreach (Food food in order.Value)
                    {
                        food.QualityScore = chef.CompetenceLevel;
                    }
                    Kitchen.OutOrders.Add(order.Key, order.Value);
                    chef.Order = new();
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
                table.WaitingTimeScore++;
            }
        }

        // TIMER
        private void EatingTimer(Table table, Restaurant restaurant)
        {
            if (table.WaitingForFood == false && table.HasOrdered == true)
            {
                // WaitingTimeScore++ now increases points longer the wait - backwardsthinking
                table.EatingFoodCounter++;
            }
            // TimeToEatFood = 20
            if (table.EatingFoodCounter == _timeToEatFood)
            {
                Checkout(table, restaurant);
            }
        }

        private void Checkout(Table table, Restaurant restaurant)
        {
            // TA BORT restaurant på något sätt
            Business.Run(table, restaurant);
            Business.Pay(table, restaurant);

            // Newsfeed();
            table.Guests = new();
            table.Order = new();
            table.Actions = new();
            table.IsDirty = true;
            table.HasOrdered = false;
            table.EatingFoodCounter = 0;
            table.WaitingTimeScore = 0;
            table.OverallScore = 0;

            foreach (Waiter waiter in Waiters)
            {
                if (waiter.HasOrder is false && waiter.HasFoodToDeliver is false && waiter.CleaningTable is false)
                {
                    table.ServiceScore += waiter.ServiceScore;
                    waiter.CleaningTable = true;
                    break;
                }
            }
        }

        private void CleanTable(Waiter waiter)
        {
            foreach (Table table in Tables)
            {
                if (table.IsDirty == true && waiter.CleaningTable == true)
                {
                    waiter.Counter++;
                    // TimeToCleanTable = 3
                    if (waiter.Counter >= _timeToCleanTable)
                    {
                        waiter.CleaningTable = false;
                        table.IsDirty = false;
                        waiter.Counter = 0;
                        table.Occupied = false;
                        table.ServiceScore = 0;
                    }
                    break;
                }
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
