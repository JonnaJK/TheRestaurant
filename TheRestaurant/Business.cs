using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRestaurant.Folder;
using TheRestaurant.Foods;
using TheRestaurant.Interface;
using TheRestaurant.Persons;

namespace TheRestaurant
{
    /*
    The Restaurant by Jonna, Jesper, Mattias and Hanna(System22 @ Campus Nyköping)



         Business class is designed to breakdown certain points given to the table of guests from various sources(food quality, table placement, the waiters competence and how long they've waited for their food).
         The purpose of the points is to determine how satisfied they're with their visit to our restaurant and how much tip in % they should give.
         This is also where we calculate and compile their receipt.


         As for now, we calculate the table overall satisfaction and not the individual (they all get the same points from various sources).
         They all wait the same amount of time for their food, sit at the same table, their food is cooked by the same chef and they all have the same waiter(s).
        
        
         ---------------------------------------
        
        
         The 4 systems that give us the overallscore is:
        
         1. qualityScore(see CookFood in restaurant.cs)
         Each dish(in Food.cs) has an empty property called QualityScore.
         Each chef is given a randomized competencelevel between 1 - 5.
         When the food order arrives to the kitchen, the assigned chef "cooks" the food and give qualityscore a value
         based on the chefs competencelevel before sending it back to the table.
        
        
         2. placementScore (see Create in table.cs)
         Each table has an empty property called PlacementScore.
         Each table created with the method called Create, generates a randomized number between 1 - 5 abd then assign it to PlacementScore.
         
        
         3. serviceScore (see the method Pointsystem in business.cs for algorithm)
         Each table has an empty property called ServiceScore.
         Each waiter (in waiter.cs) is given a randomized number between 1 - 5, called ServiceScore.
         Every time the waiter do something for the table, wether is be taking the order or delivering the food. The waiter "drops" off the random number into the property ServiceScore.
         There are a total of 3 visits to the table and they all can be done by 3 different waiters with their individual score.
         This means that a table can have a total of 15 in servicescore, but it's highly unlikely that they will be served by the same waiter all night so a lower number is to be expected.
         Inside the method Pointsystem (below) we then convert the number given by the waiters into a number between 1 - 5, which then is the updated serviceScore inside business.cs and being used in overallscore.


         * If we implement a unique counter for each waiter, then we can track the workethic of that individual and divide the tip based on performance.
         
        
         4. waitingTimeScore (see the methods CheckIfHasOrdered and ServiceTimer in restaurant.cs)
         Each table has a bool called WaitingForFood that is set to false.
         Each table also has an empty property called WaitingTimeScore and this is the counter.
         Once the waiter has the table order, we set WaitingForFood to true and the counter start.
         The longer they wait, the lower the score gets and mathematically the lowest the counter can be is 12 unless we change the waiters priority.
         We use the same type of converting method like we do in serviceScore, but reversed.
         For this system we utilize the fixed int called timeToCookFood (which is set to 10) and just add +1 / +2 / +3 etc.to give us a value of 1 - 5.



         Each system can give a total of 5 points to the table and / or the guest.


         5 = Extremely good
         4 = Very good
         3 = Good
         2 = Not so good
         1 = Bad



         These systems score feed into the overallscore, and then we use that to tell us how satisfied they are.
         Max score is 20 and 4 is the lowest.
         
         Equal to or more than 16 = Very pleased
         More than 12 but below 16 = Pleased
         More than 8 but below or equal to 11 = Average
         More than 4 but below or equal to 8 = Dissatisfied
         Below or equal 4 = Very dissatisfied


         -------------------------------------- -


         We use the same overallscore to determine how much tip they give when leaving the restaurant.


         If the overallscore is 20, they leave 20 % in tip(the highest possible %).
         If the overallscore is 4, they leave 4 % in tip(the lowest possible %).


         The amount of money they leave as tip is based on the price of their respective food order.

         Example: 
         If they eat a stake that costs 300sek and they have an overallscore of 10.
         The amount of tip is 10 % of 300, which is 30.
         See the method Tips in business.cs.

         We have variable called totalGuestPrice which is the combined amount a certain guest has to pay(price of food + tip).
         Each guest has a randomized number for money and if the totalguestprice exceeds their money, they have to do the dishes as punishment / payment.



         -------------------------------------- -


         By separating these values / scores we can use them for further calculations in the future depending on the stakeholders needs.


         Examples of business metrics that can be created with these variables:

         > Daily profit for the restaurant
         > Daily tip for the staff
         > Average value of each table
         > Average value of each guest
         > Average tip left by the guests
         > Divide tip / profit between staff and the "house"

         and many more can be added by tweaking or adding some properties / variables for basiclly all classes(track each day that feeds into a log, rent, salaries, raw materials etc.)

         ***************************************
         This text has been updated 9 / 11 - 22.
    */



    internal static class Business
    {
        private static int _qualityScore;
        private static int _placementScore;
        private static int _waitingTimeScore;
        private static int _serviceScore;
        private static readonly int _maxOverallScore = 20;
       
        internal static void ShowReceipt()
        {

        }
        internal static void Run(Table table, Restaurant restaurant)
        {
            Pointsystem(table, restaurant);
            foreach (Guest guest in table.Guests)
            {
                guest.Score = OverallScore();
                Tips(guest, table);
                table.OverallScore += guest.Score;
            }
            table.OverallScore /= table.Guests.Count;
            RecieptAction(table);
        }

        internal static void Pay(Table table, Restaurant restaurant)
        {
            foreach (var guest in table.Guests)
            {
                if (guest.Money >= guest.MyMeal.Price)
                {
                    var totalGuestPrice = guest.Tips + guest.MyMeal.Price;

                    // If they have enough money to both pay their meal and leave a tip.
                    if (guest.Money >= totalGuestPrice)
                    {
                        restaurant.CashRegister += guest.MyMeal.Price;
                        restaurant.TipJar += guest.Tips;
                        guest.Money -= totalGuestPrice;
                    }
                    // If they have enough money to pay for their meal, but not leave a tip.
                    else
                    {
                        restaurant.CashRegister += guest.MyMeal.Price;
                        guest.Money -= guest.MyMeal.Price;
                        guest.Tips = 0;
                    }
                }
                // They dont have enough money to pay for their meal, nor leaving a tip.
                else
                {
                    restaurant.CashRegister += guest.Money;
                    guest.Receipt = guest.Money;
                    guest.Money -= guest.Money;
                    guest.Tips = -1;
                }
            }
        }

        // If we choose to set each individual satisfaction and want to print it to receipt, we need to use the guests overallscore and not table.
        private static void RecieptAction(Table table)
        {
            if (table.OverallScore > _maxOverallScore * 0.8)
            {
                table.Actions = "The guests are very happy with their overall experience.";
            }
            else if (table.OverallScore > _maxOverallScore * 0.6)
            {
                table.Actions = "The guests are content with their overall experience.";
            }
            else if (table.OverallScore > _maxOverallScore * 0.4)
            {
                table.Actions = "The guests are neutral with their overall experience.";
            }
            else if (table.OverallScore > _maxOverallScore * 0.2)
            {
                table.Actions = "The guests are dissatisfied with their overall experience.";
            }
            else
            {
                table.Actions = "The guests are extremely unhappy with their overall experience.";
            }
        }

        private static void Tips(Guest guest, Table table)
        {
            guest.Tips = (guest.Score / 100) * guest.MyMeal.Price;
        }

        private static double OverallScore()
        {
            double overallScore = _qualityScore + _placementScore + _waitingTimeScore + _serviceScore;
            return overallScore;
        }

        private static void Pointsystem(Table table, Restaurant restaurant)
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
        }
    }
}
