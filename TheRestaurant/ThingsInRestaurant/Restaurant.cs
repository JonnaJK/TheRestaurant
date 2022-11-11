using TheRestaurant.Persons;

namespace TheRestaurant.ThingsInRestaurant
{
    internal class Restaurant
    {
        public List<Table> Tables { get; set; } = new();
        public Kitchen Kitchen { get; set; } = new();
        public List<Waiter> Waiters { get; set; } = new();
        public double CashRegister { get; set; }
        public double TipJar { get; set; }
        public int Time { get; set; }
        public int HasDoneDishes { get; set; }

        private Entrance entrance = new();
        private readonly Random _random = new();
        internal readonly int _timeToCookFood = 10;
        internal readonly int _timeToEatFood = 20;

        public void Run(Restaurant restaurant)
        {
            CreateRestaurant();

            while (true)
            {
                GUI.DrawRestaurant(restaurant, entrance);
                Actionlist();

                ManageWaiters(restaurant);

                ManageChefs();

                ManageTables();

                Time++;
                Console.ReadKey();
            }
        }

        private void ManageWaiters(Restaurant restaurant)
        {
            var availableWaiters = Waiters.Where(x => !x.CleaningTable).ToList();
            var occupiedWaiters = Waiters.Where(x => x.CleaningTable).ToList();
            foreach (var waiter in occupiedWaiters)
            {
                waiter.CleanTable(waiter, Tables, _random);
            }
            foreach (Waiter waiter in availableWaiters)
            {
                // Sort out tables that are available, waiting for order or waiting to be checked out.
                var availableTables = Tables.Where(x => !x.Occupied).ToList();
                var tablesWaitingToOrder = Tables.Where(x => !x.HasOrdered && x.Occupied).ToList();
                var tablesToCheckOut = Tables.Where(x => x.Occupied && x.EatingFoodCounter >= _timeToEatFood).ToList();

                // If there is a matching table size for a company in the entrance.
                var canMatchTableToGuest = CanMatchGuestsToTable(availableTables);

                // The waiters different tasks to do.
                if (waiter.OutOrder.Count > 0)
                {
                    Waiter.DropOffFood(waiter, Tables);
                }
                else if (canMatchTableToGuest == true)
                {
                    MatchTableForGuests(availableTables);
                }
                else if (waiter.HasOrder)
                {
                    Kitchen.DropOffOrders(waiter);
                }
                else if (Kitchen.OutOrders.Count > 0 && waiter.HasOrder is false && waiter.HasFoodToDeliver is false)
                {
                    Kitchen.PickUpFood(waiter);
                }
                else if (tablesWaitingToOrder.Count > 0 && waiter.HasOrder is false)
                {
                    Waiter.CheckIfHasOrdered(waiter, tablesWaitingToOrder, _random);
                }
                else if (tablesToCheckOut.Count > 0)
                {
                    Checkout(restaurant, tablesToCheckOut, waiter);
                }
            }
        }

        private bool CanMatchGuestsToTable(List<Table> availableTables)
        {
            var canMatchTableToGuest = false;
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
            return canMatchTableToGuest;
        }

        // Check for suitable table for party of guests
        private void MatchTableForGuests(List<Table> availableTables)
        {
            var availableSmallTables = availableTables.Where(x => x.Small).ToList();
            var availableBigTables = availableTables.Where(x => !x.Small).ToList();

            for (int i = 0; i < entrance.GroupOfGuests.Count; i++)
            {
                if (entrance.GroupOfGuests[i].Count <= 2)
                {
                    if (availableSmallTables.Count > 0)
                    {
                        Waiter.PlaceGuestsAtTable(i, availableSmallTables, entrance.GroupOfGuests);
                        break;
                    }
                }
                else if (entrance.GroupOfGuests[i].Count > 2)
                {
                    if (availableBigTables.Count > 0)
                    {
                        Waiter.PlaceGuestsAtTable(i, availableBigTables, entrance.GroupOfGuests);
                        break;
                    }
                }
            }
        }

        private void Checkout(Restaurant restaurant, List<Table> tablesToCheckOut, Waiter waiter)
        {
            foreach (var table in tablesToCheckOut)
            {
                table.ServiceScore += waiter.ServiceScore;
                Business.SetPoints(table, restaurant);
                Business.Pay(table, restaurant);
                Receipt(table);
                Waiter.StartCleaning(table, waiter);
                break;
            }
        }

        private void ManageChefs()
        {
            foreach (Chef chef in Kitchen.Chefs)
            {
                if (Kitchen.InOrders.Count > 0 && !chef.HasOrder)
                {
                    Kitchen.TakeNewOrder(chef);
                }
                if (chef.HasOrder)
                {
                    Chef.CookFood(chef);
                }
                if (chef.Counter == _timeToCookFood)
                {
                    Kitchen.PlaceCookedFoodForPickup(chef);
                }
            }
        }

        private void ManageTables()
        {
            foreach (Table table in Tables)
            {
                if (table.WaitingForFood)
                {
                    table.WaitingTimeScore++;
                }
                if (!table.WaitingForFood && table.HasOrdered)
                {
                    table.EatingFoodCounter++;
                }
                if (table.EatingFoodCounter >= _timeToEatFood)
                {
                    table.IsDirty = true;
                }
            }
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
                    HasDoneDishes++;
                }
            }
            table.Receipt.Add(table.Actions);
            table.Receipt.Add(new string(' ', 98));
            GUI.DrawActionList("Receipt", 0, 28, table.Receipt);
        }

        // Create Persons and Tables
        private void CreateRestaurant()
        {
            // Creates guests and placed them in groups, a list of lists
            Guest.CreateGuests(80, _random, entrance.GroupOfGuests);

            // Creates waiters in restaurant
            Person.Create(_random, Waiters, 3);
            // Creates chefs in kitchen
            Person.Create(_random, Kitchen.Chefs, 5);

            // Creates small and big tables
            Table.Create(_random, Tables, true, 5);
            Table.Create(_random, Tables, false, 5);
        }

        private void Actionlist()
        {
            List<string> actionlist = new();

            actionlist.Add("Minutes passed: " + Time);
            actionlist.Add("Cashregister: " + Math.Round(CashRegister, 2));
            actionlist.Add("TipJar: " + Math.Round(TipJar, 2));
            actionlist.Add("Numbers of guests having to wash dishes: " + HasDoneDishes);

            GUI.DrawActionList("Newsfeed", 105, 0, actionlist);
        }
    }
}
