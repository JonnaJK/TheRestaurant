using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Persons;

namespace TheRestaurant
{
    internal class Business
    {
        public int QualityScore { get; set; }
        public int PlacementScore { get; set; }
        public int WaitingTimeScore { get; set; }
        public int ServiceScore { get; set; }






        private void Pointsystem(Table table, Waiter waiter, Restaurant restaurant)
        {

            // Waiting for food Score
            int waitresult = table.WaitingTimeScore;

            if (waitresult <= restaurant._timeToCookFood + 2)
            {
                WaitingTimeScore = 5;
            }
            else if (waitresult == restaurant._timeToCookFood + 3)
            {
                WaitingTimeScore = 4;
            }
            else if (waitresult == restaurant._timeToCookFood + 4)
            {
                WaitingTimeScore = 3;
            }
            else if (waitresult == restaurant._timeToCookFood + 5)
            {
                WaitingTimeScore = 2;
            }
            else if (waitresult >= restaurant._timeToCookFood + 6)
            {
                WaitingTimeScore = 1;
            }

            // Table Placement Score
            PlacementScore = table.PlacementScore;

            // Service Score
            if (table.ServiceScore >= 13)
            {
                ServiceScore = 5;
            }
            else if (table.ServiceScore > 9 && table.ServiceScore <= 12)
            {
                ServiceScore = 4;
            }
            else if (table.ServiceScore > 6 && table.ServiceScore <= 9)
            {
                ServiceScore = 3;
            }
            else if (table.ServiceScore > 3 && table.ServiceScore <= 6)
            {
                ServiceScore = 2;
            }
            else if (table.ServiceScore <= 3)
            {
                ServiceScore = 1;
            }

            // Quality Score


        }


    }
}
