using TheRestaurant.Folder;

namespace TheRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Restaurant restaurant = new();
            restaurant.Run(restaurant);
        }
    }
}
