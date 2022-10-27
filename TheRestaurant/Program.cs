using TheRestaurant.Person_Objects;

namespace TheRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Guest guest = new(random);
            Chef chef = new(random);
        }
    }
}