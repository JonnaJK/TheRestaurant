using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Foods;
using TheRestaurant.Persons;

namespace TheRestaurant
{
    internal static class Business
    {
        private static int _qualityScore;
        private static int _placementScore;
        private static int _waitingTimeScore;
        private static int _serviceScore;
        private static readonly int _maxOverallScore = 20;

        internal static void Run(Table table, Waiter waiter, Restaurant restaurant)
        {
            Pointsystem(table, waiter, restaurant);
            foreach (Guest guest in table.Guests)
            {
                guest.Tips = OverallScore();
                Tips(guest, table);
                table.OverallScore += guest.Tips;
            }
            table.OverallScore /= table.Guests.Count;
            RecieptAction(table);
        }
        private static void RecieptAction(Table table) // ev ta in gästens egna värde, istället för bordet, framtidssäkra
        {
            string action = "";

            if(table.OverallScore > _maxOverallScore * 0.8)
            {
                action = "The guests are very happy with their overall experience";
            }
            else if(table.OverallScore > _maxOverallScore * 0.6)
            {
                action = "The guests are content with their overall experience";
            }
            else if(table.OverallScore > _maxOverallScore * 0.4)
            {
                action = "The guests are neutral with their overall experience";
            }
            else if(table.OverallScore > _maxOverallScore * 0.2)
            {
                action = "The guests are dissatisfied with their overall experience";
            }
            else
            {
                action = "The guests are extremely unhappy with their overall experience";
            }
        }
        private static void Tips(Guest guest, Table table)
        {
            Dictionary<string, double> dicPriceWithTip = new();

            double percentTips = (guest.Tips / 100) + 1;
            double priceWithTip = guest.MyMeal.Price * percentTips;

            dicPriceWithTip.Add(guest.Name, priceWithTip);
            table.SummaryGuest.Add(dicPriceWithTip);
        }

        private static double OverallScore()
        {
            double overallScore = _qualityScore + _placementScore + _waitingTimeScore + _serviceScore;
            return overallScore;
        }

        private static void Pointsystem(Table table, Waiter waiter, Restaurant restaurant)
        {

            // Waiting for food Score
            int waitresult = table.WaitingTimeScore;

            if (waitresult <= restaurant._timeToCookFood + 2)
            {
                _waitingTimeScore = 5;
            }
            else if (waitresult == restaurant._timeToCookFood + 3)
            {
                _waitingTimeScore = 4;
            }
            else if (waitresult == restaurant._timeToCookFood + 4)
            {
                _waitingTimeScore = 3;
            }
            else if (waitresult == restaurant._timeToCookFood + 5)
            {
                _waitingTimeScore = 2;
            }
            else if (waitresult >= restaurant._timeToCookFood + 6)
            {
                _waitingTimeScore = 1;
            }

            // Table Placement Score
            _placementScore = table.PlacementScore;

            // Food Quality Score
            foreach (Food food in table.Order)
            {
                _qualityScore += food.QualityScore;
            }
            _qualityScore /= table.Order.Count;

            // Service Score
            if (table.ServiceScore >= 13)
            {
                _serviceScore = 5;
            }
            else if (table.ServiceScore > 9 && table.ServiceScore <= 12)
            {
                _serviceScore = 4;
            }
            else if (table.ServiceScore > 6 && table.ServiceScore <= 9)
            {
                _serviceScore = 3;
            }
            else if (table.ServiceScore > 3 && table.ServiceScore <= 6)
            {
                _serviceScore = 2;
            }
            else if (table.ServiceScore <= 3)
            {
                _serviceScore = 1;
            }

            // Quality Score


        }


    }
}
