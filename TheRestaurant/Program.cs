﻿using TheRestaurant.Food;
using TheRestaurant.Interface;
using TheRestaurant.Person;
using TheRestaurant.Folder;

namespace TheRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Restaurant restaurant = new();
            restaurant.Run();


        //    Random random = new Random();
        //    Chef chef = new(random);

        //    Guest guest1 = new(random);
        //    Guest guest2 = new(random);
        //    Guest guest3 = new(random);
        //    Guest guest4 = new(random);
        //    Guest guest5 = new(random);

        //    Table table1 = new(random, "Bord 1(2)", true);
        //    Table table2 = new(random, "Bord 2(4)", false);


        //    table1.Guests.Add(guest1);
        //    table1.Guests.Add(guest2);
        //    table2.Guests.Add(guest3);
        //    table2.Guests.Add(guest4);
        //    table2.Guests.Add(guest5);

        //    DrawRestaurant(table2);
        //    Helpers.DrawAnyList<Guest>(table1.Name, 5, 5, table1.Guests); // Så här ritar vi ut borden i matsalen
        //    Helpers.DrawAnyList<Guest>(table2.Name, 10, 10, table2.Guests);
        //    Console.ReadKey();




        //    IFood tomatosoup = new TomatoSoup(); // varför IFood?
        //    TomatoSoup tomatoSoup = new();       // och inte detta?

        //}


    }
}
