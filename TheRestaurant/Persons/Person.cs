using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TheRestaurant.Interface;

namespace TheRestaurant.Persons
{
    public class Person : IPerson
    {
        public string Name { get; set; }
        public int Counter { get; set; }

        // Kolla ifall vi kan lägga till en bool HasOrder - DRY! - Väntetiden(service)

        public Person()
        {
            Name = Helpers.GetName();

        }

        // Generic method - not complete. Using Create(Overload) now, but wants to use Generic method
        //internal static List<T> Creates<T>(T anything, int number, Random random)
        //{
        //    if (anything is Chef)
        //    {
        //        List<Chef> list = new();
        //        for (int i = 0; i < number; i++)
        //        {
        //            list.Add(new Chef(random));
        //        }
        //    }
        //    else
        //    {
        //        List<Waiter> list = new();
        //        for (int i = 0; i < number; i++)
        //        {
        //            list.Add(new Waiter());
        //        }
        //    }
        //    return list;
        //}

        internal static void Create(Random random, List<Chef> chefs, int numberOfChefs)
        {
            for (int i = 0; i < numberOfChefs; i++)
            {
                chefs.Add(new Chef(random));
            }
        }

        internal static void Create(Random random, List<Waiter> waiters, int numberOfWaiters)
        {
            for (int i = 0; i < numberOfWaiters; i++)
            {
                waiters.Add(new Waiter(random, "W" + (i + 1)));
            }
        }

    }
}
