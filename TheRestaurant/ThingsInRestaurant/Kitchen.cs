using TheRestaurant.Foods;
using TheRestaurant.Persons;

namespace TheRestaurant.ThingsInRestaurant
{
    internal class Kitchen
    {
        public List<Chef> Chefs { get; set; } = new();
        public Dictionary<string, List<Food>> InOrders { get; set; } = new();
        public Dictionary<string, List<Food>> OutOrders { get; set; } = new();
        public bool HasOrders { get; set; }
        public bool HasFoodToPickUp { get; set; }

        // Put new(); outside of the foreach, and give the waiter the ability
        // to take mulitple table orders to the kitchen at once.
        internal void DropOffOrders(Waiter waiter)
        {
            foreach (KeyValuePair<string, List<Food>> order in waiter.InOrder)
            {
                InOrders.Add(order.Key, order.Value);
                waiter.InOrder = new();
                waiter.HasOrder = false;
            }
        }

        internal void TakeNewOrder(Chef chef)
        {
            //Chef takes order from kitchen
            if (InOrders.Count > 0 && chef.HasOrder == false)
            {
                foreach (KeyValuePair<string, List<Food>> order in InOrders)
                {
                    chef.Order.Add(order.Key, order.Value);
                    InOrders.Remove(order.Key);
                    chef.HasOrder = true;
                    break;
                }
            }
        }

        internal void PlaceCookedFoodForPickup(Chef chef)
        {
            //Chef puts cooked food in outOrders (kitchen window)
            foreach (KeyValuePair<string, List<Food>> order in chef.Order)
            {
                //Give score to each food in order
                Chef.GiveFoodQuality(chef, order);

                OutOrders.Add(order.Key, order.Value);
                chef.Order = new();
                chef.HasOrder = false;
                chef.Counter = 0;
            }
        }

        // Waiter picks up food from kitchen
        internal void PickUpFood(Waiter waiter)
        {
            if (OutOrders.Count > 0 && waiter.HasOrder is false && waiter.HasFoodToDeliver is false)
            {
                foreach (KeyValuePair<string, List<Food>> order in OutOrders)
                {
                    waiter.OutOrder.Add(order.Key, order.Value);
                    OutOrders.Remove(order.Key);
                    break;  // If not break; one waiter takes every order from kitchen outorders (effective restaurant variant)
                }
            }
        }
    }
}
