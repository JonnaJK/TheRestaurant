using TheRestaurant.Foods;

namespace TheRestaurant.Persons
{
    internal class Chef : Person
    {
        public int CompetenceLevel { get; set; }
        public bool HasOrder { get; set; }
        public Dictionary<string, List<Food>> Order { get; set; } = new();

        public Chef(Random random, string name)
        {
            CompetenceLevel = random.Next(1, 6);
            name += Name;
            Name = name; 
        }

        internal static void GiveFoodQuality(Chef chef, KeyValuePair<string, List<Food>> order)
        {
            foreach (Food food in order.Value)
            {
                food.QualityScore = chef.CompetenceLevel;
            }
        }

        internal static void CookFood(Chef chef)
        {
            chef.Counter++;
        }
    }
}
