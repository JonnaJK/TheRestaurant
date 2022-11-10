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
        public int Time { get; set; }
        private readonly Random _random = new Random();
        private Entrance entrance = new();
        internal readonly int _timeToCookFood = 10;
        internal readonly int _timeToEatFood = 20;
        internal readonly int _timeToCleanTable = 3;



        public void Run(Restaurant restaurant)
        {
            CreateRestaurant();

            while (true)
            {
                DrawRestaurant();
                GUI.DrawRestaurant(restaurant, entrance);
                var tablesWaitingForOrder = Tables.Where(x => !x.HasOrdered && x.Occupied).ToList();
                var availableWaiters = Waiters.Where(x => !x.CleaningTable).ToList();
                var occupiedWaiters = Waiters.Where(x => x.CleaningTable).ToList();

                //Actionlist(dirtyTables);

                //foreach (Waiter waiter in occupiedWaiters)
                //{
                var dirtyTables = Tables.Where(x => x.IsDirty).ToList();
                if (dirtyTables.Count > 0)
                {
                    CleanTable(dirtyTables, occupiedWaiters);
                }
                //}

                // Fetch next group (if there is one) and get an available table to the company
                //List<Guest>? nextMatchedGroup = new(); // ev ta bort

                foreach (Waiter waiter in availableWaiters)
                {
                    List<Table> availableTables = Tables.Where(x => !x.Occupied).ToList();
                    bool canMatchTableToGuest = false;
                    if (entrance.GroupOfGuests.Count > 0)
                    {
                        for (int i = 0; i < entrance.GroupOfGuests.Count; i++)
                        {
                            var nextMatchedGroup = entrance.GroupOfGuests[i];
                            var availableSmallTables = availableTables.Where(x => !x.Occupied && x.Small).ToList();
                            var availableBigTables = availableTables.Where(x => !x.Occupied && !x.Small).ToList();

                            if (nextMatchedGroup.Count > 2 && availableBigTables.Count > 0)
                            {
                                canMatchTableToGuest = true;
                                break;
                            }
                            else if (nextMatchedGroup?.Count < 3 && availableSmallTables.Count > 0)
                            {
                                canMatchTableToGuest = true;
                                break;
                            }
                        }
                    }

                    if (waiter.OutOrder.Count > 0)
                    {
                        DropOfFood(waiter);
                    }
                    else if (canMatchTableToGuest == true)
                    {
                        MatchTableForGuests(availableTables);
                    }
                    else if (waiter.HasOrder)
                    {
                        DropOffOrder(waiter);
                    }
                    else if (Kitchen.OutOrders.Count > 0 && waiter.HasOrder is false && waiter.HasFoodToDeliver is false)
                    {
                        PickUpFoodFromKitchen(waiter);
                    }
                    else if (tablesWaitingForOrder.Count > 0)
                    {
                        CheckIfHasOrdered(waiter);
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
                Console.ReadKey();
                // KOM IHÅG ATT TA BORT ALLA HÅRDKODADE TAL

                Time++;

            }
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
        private void MatchTableForGuests(List<Table> availableTables)
        {
            //availableTables[0].Guests.AddRange(groupOfGuests);
            //entrance.GroupOfGuests.Remove(groupOfGuests);
            //availableTables[0].Occupied = true;
            //availableTables[0].Receipt = new();
            var availableSmallTables = availableTables.Where(x => x.Small).ToList();
            var availableBigTables = availableTables.Where(x => !x.Small).ToList();

            for (int i = 0; i < entrance.GroupOfGuests.Count; i++)
            {
                if (entrance.GroupOfGuests[i].Count <= 2)
                {
                    if (availableSmallTables.Count > 0)
                    {
                        PlaceAtTable(i, availableSmallTables);
                        break;
                    }
                }
                else if (entrance.GroupOfGuests[i].Count > 2)
                {
                    if (availableBigTables.Count > 0)
                    {
                        PlaceAtTable(i, availableBigTables);
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
                table.Receipt = new();
                break;
            }
        }

        // THIS IS WRONG, MAKE IT STOP!!
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
            if (table.EatingFoodCounter >= _timeToEatFood && table.Guests.Count > 0)
            {
                Checkout(table, restaurant);

                // Newsfeed();
                table.Guests = new();
                table.IsDirty = true;
                table.HasOrdered = false;

                foreach (Waiter waiter in Waiters)
                {
                    if (waiter.HasOrder is false && waiter.HasFoodToDeliver is false && waiter.CleaningTable is false)
                    {
                        table.Order = new();
                        table.Actions = "";
                        table.WaitingTimeScore = 0;
                        table.OverallScore = 0;

                        table.EatingFoodCounter = 0;
                        table.ServiceScore += waiter.ServiceScore;
                        waiter.CleaningTable = true;
                        table.Waiter = waiter;
                        break;
                    }
                }
            }
        }

        private void Checkout(Table table, Restaurant restaurant)
        {
            // TA BORT restaurant på något sätt
            Business.Run(table, restaurant);
            Business.Pay(table, restaurant);
            Receipt(table);
        }

        private void CleanTable(List<Table> dirtyTables, List<Waiter> occupiedWaiters)
        {
            foreach (var waiter in occupiedWaiters)
            {
                foreach (Table table in dirtyTables)
                {
                    //var result = dirtyTables.Where(x => x.Waiter == waiter).FirstOrDefault();
                    if (table.Waiter.Name != "")
                    {
                        if (table.Waiter.Name == waiter.Name)
                        {
                            waiter.Counter++;

                            if (waiter.Counter >= _timeToCleanTable) // _timeToCleanTable = 3
                            {
                                table.IsDirty = false;
                                table.Occupied = false;
                                waiter.CleaningTable = false;
                                waiter.Counter = 0;
                                table.ServiceScore = 0;
                                table.Waiter = new(_random, "");
                            }
                            break;
                        }
                    }
                    else
                    {
                        var availableWaiters = Waiters.Where(x => !x.CleaningTable).ToList();

                        if (availableWaiters.Count > 0)
                        {
                            table.Waiter = availableWaiters[0];
                            availableWaiters[0].CleaningTable = true;
                        }
                    }
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

        internal void Receipt(Table table)
        {

            table.Receipt.Add("Group: " + table.Guests[0].Name + " +" + (table.Guests.Count - 1));
            foreach (Guest guest in table.Guests)
            {
                if (guest.Tips != -1)
                {
                    table.Receipt.Add($"{guest.Name}: {guest.MyMeal.Name}, {guest.MyMeal.Price} SEK and tipped {Math.Round(guest.Tips, 2)} SEK");
                }
                else
                {
                    table.Receipt.Add($"{guest.Name}: {guest.MyMeal.Name}, {guest.MyMeal.Price} SEK but could only pay {Math.Round(guest.Receipt, 2)} SEK,");
                    table.Receipt.Add($"so they also had to wash the dishes");
                }
            }
            table.Receipt.Add(table.Actions);
            GUI.DrawActionList("Receipt", 0, 28, table.Receipt);
        }

        private void Actionlist(List<Table> dirtyTables)
        {

            List<string> actionlist = new();
            var counter = 0;
            foreach (var waiter in Waiters)
            {
                actionlist.Add("Namn: " + waiter.Name);
                actionlist.Add("HasOrder: " + waiter.HasOrder);
                actionlist.Add("Order: " + String.Join(", ", waiter.InOrder));
                actionlist.Add("Cleaning table: " + waiter.CleaningTable);
                actionlist.Add("Tid att städa: " + waiter.Counter);
                actionlist.Add("Har mat att lämna: " + waiter.HasFoodToDeliver); // visar fel
                actionlist.Add("Maten: " + String.Join(", ", waiter.OutOrder));
                actionlist.Add("Antal smutsiga bord: " + dirtyTables.Count);
                actionlist.Add("ServiceScore: " + waiter.ServiceScore);
                actionlist.Add("--------------------------------");
                counter++;
                //if (counter >= 2)
                //{
                //    break;
                //}

            }

            //foreach (Table waiter in Tables)
            //{

            //    actionlist.Add("Namn: " + waiter.Name);
            //    actionlist.Add("Order: " + String.Join(", ", waiter.Order));
            //    actionlist.Add("Ockupado: " + waiter.Occupied);
            //    actionlist.Add("Tid att äta mat: " + waiter.EatingFoodCounter);
            //    actionlist.Add("Bordet har beställt: " + waiter.HasOrdered);
            //    actionlist.Add("Smutsigt bord: " + waiter.IsDirty);
            //    actionlist.Add("Totalpoäng för bordet: " + waiter.OverallScore);
            //    actionlist.Add("WaitingTimeScore: " + waiter.WaitingTimeScore);
            //    actionlist.Add("Waiting for food: " + waiter.WaitingForFood);
            //    actionlist.Add("ServiceScore: " + waiter.ServiceScore);
            //    actionlist.Add("antal gäster i sällskap: " + waiter.Guests.Count);
            //    actionlist.Add(String.Join(", ", waiter.Actions));
            //    actionlist.Add("--------------------------------");
            //    counter++;
            //    if (counter >= 2)
            //    {
            //        break;
            //    }

            //}

            actionlist.Add("Kassaregister: " + Math.Round(CashRegister, 2));
            actionlist.Add("TipJar: " + Math.Round(TipJar, 2));
            GUI.DrawActionList("Actionlist", 105, 0, actionlist);

        }
    }
}
