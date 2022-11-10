using TheRestaurant.Persons;

namespace TheRestaurant.Folder
{
    internal class Entrance
    {
        public List<List<Guest>> GroupOfGuests { get; set; }

        public Entrance()
        {
            GroupOfGuests = new List<List<Guest>>();
        }
    }
}
