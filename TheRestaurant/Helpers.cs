﻿namespace TheRestaurant
{
    internal class Helpers
    {
        public static string GetName()
        {
            Random random = new();
            List<string> surnames = new()
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones",
                "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
                "Hernandez", "Lopez", "Gonzales", "Wilson", "Anderson",
                "Thomas", "Taylor", "Moore", "Jackson", "Martin",
                "Lee", "Perez", "Thompson", "White", "Harris",
                "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright",
                "Scott", "Torres", "Nguyen", "Hill", "Flores",
                "Green", "Adams", "Nelson", "Baker", "Hall",
                "Rivera", "Campbell", "Mitchell", "Carter", "Gomez",
                "Roberts", "Philips", "Evans", "Turner", "Diaz",
                "Parker", "Cruz", "Edwards", "Collins", "Reyes",
                "Stewart", "Morris", "Morales", "Murphy", "Cook",
                "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
                "Peterson", "Bailey", "Reed", "Kelly", "Howard",
                "Ramos", "Kim", "Cox", "Ward", "Richardson",
                "Watson", "Brooks", "Chavez", "Wood", "James",
                "Bennet", "Gray", "Mendoza", "Ruiz", "Hughes",
                "Price", "Alvarez", "Castillo", "Sanders", "Patel",
                "Myers", "Long", "Ross", "Foster", "Jimenez"
            };
            int index = random.Next(surnames.Count);

            return surnames[index];
        }
    }
}
