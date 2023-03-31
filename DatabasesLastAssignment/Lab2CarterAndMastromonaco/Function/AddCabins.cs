using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Lab2CarterAndMastromonaco.Function
{
    internal class AddCabins
    {
        //Här ligger vårad seed-data för cabins/counselors och campers
        public static void AddCabin()
        {
            List<Cabin> cabins = new List<Cabin>();
                var cabin1 = new Cabin
                {
                    CabinName = "The Lumberjack Lounge",
                    CabinCapacity = 5,
                    Campers = new List<Camper>
                {
                    new Camper
                    {
                        FirstName = "Carlo",
                        LastName = "Torre di Rezzonico",
                        PersonNumber = "16930307-1219",
                        Allergies = "Nötter",
                        VisitStart = new DateTime(2022, 7, 1),
                        VisitEnd = new DateTime(2022, 7, 28),
                        Kin = new List<NextOfKin>
                        {
                            new NextOfKin
                            {
                                FirstName = "Mike",
                                LastName = "Wazowski",
                                Relation = "Father"
                            },
                            new NextOfKin
                            {
                                FirstName = "Celia",
                                LastName = "Mae",
                                Relation = "Mother"
                            }
                        }
                    },
                    new Camper
                    {
                        FirstName = "Giovanni",
                        LastName = "Ganganelli",
                        PersonNumber = "17051031-8859",
                        VisitStart = new DateTime(2022, 7, 7),
                        VisitEnd = new DateTime(2022, 7, 14),
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Count Giovanni",
                        LastName = "Braschi",
                        PersonNumber = "17171225-8818",
                        VisitStart = new DateTime(2022, 7, 1),
                        VisitEnd = new DateTime(2022, 7, 28), 
                        Kin = null 
                    },
                    new Camper
                    {
                        FirstName = "Count Barnaba", 
                        LastName = "Chiaramonti",
                        PersonNumber = "17420814-7535",
                        VisitStart = new DateTime(2022, 7, 7), 
                        VisitEnd = new DateTime(2022, 7, 21),
                        Kin = new List<NextOfKin>
                        {
                           new NextOfKin
                           {
                            FirstName = "Mona",
                            LastName = "Lisa",
                            Relation = "Friend"
                           },
                        }
                    },
                },
                    Counselor = new Counselor
                    {
                        FirstName = "Jeffrey",
                        LastName = "Epstein",
                        PersonNumber = "19530120-1051",
                        PhoneNumber = "073-123 45 67",
                        InChargeStart = new DateTime(2022, 7, 1),
                        InChargeEnd = new DateTime(2022, 7, 31)
                    },
                };
            cabins.Add(cabin1);
            var cabin2 = new Cabin
            {
                CabinName = "The Moose Motel",
                CabinCapacity = 5,
                Campers = new List<Camper>
                {
                    new Camper
                    {
                        FirstName = "Count Annibale",
                        LastName = "Genga",
                        PersonNumber = "17600822-9892",
                        VisitStart = new DateTime(2022, 7, 10),
                        VisitEnd = new DateTime(2022, 7, 17),
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Francesco",
                        LastName = "Castiglioni",
                        PersonNumber = "17611120-2132",
                        VisitStart = new DateTime(2022, 7, 14),
                        VisitEnd = new DateTime(2022, 7, 28),
                        Kin = new List<NextOfKin>
                        {
                           new NextOfKin
                           {
                               FirstName = "Ted",
                               LastName = "Mosby",
                               Relation = "Uncle"
                           }
                        }

                    },
                    new Camper
                    {
                        FirstName = "Bartolomeo",
                        LastName = "Cappellari",
                        PersonNumber = "17650918-4454",
                        VisitStart = new DateTime(2022, 7, 1),
                        VisitEnd = new DateTime(2022, 7, 21),
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Count Giovanni", 
                        LastName = "Mastai-Ferretti",
                        PersonNumber = "17920513-3332", 
                        VisitStart = new DateTime(2022, 7, 3), 
                        VisitEnd = new DateTime(2022, 7, 10),
                        Kin = null
                    },
                },
                Counselor = new Counselor
                {
                    FirstName = "Harvey",
                    LastName = "Weinstein",
                    PersonNumber = "19520319-9999",
                    PhoneNumber = "070-456 78 90",
                    InChargeStart = new DateTime(2022, 7, 1),
                    InChargeEnd = new DateTime(2022, 7, 31)
                }
            };
            cabins.Add(cabin2);
            var cabin3 = new Cabin
            {
                CabinName = "The Bear Necessities",
                CabinCapacity = 5,
                Campers = new List<Camper>
                {
                    new Camper
                    {
                        FirstName = "Gioacchino", 
                        LastName = "Raffaele",
                        PersonNumber = "18100302-2212",
                        VisitStart = new DateTime(2022, 7, 1),
                        VisitEnd = new DateTime(2022, 7, 14), 
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Giuseppe",
                        LastName = "Sarto",
                        PersonNumber = "18350602-5445", 
                        VisitStart = new DateTime(2022,7,7), 
                        VisitEnd = new DateTime(2022,7,13), 
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Giacomo",
                        LastName = "Chiesa", 
                        PersonNumber = "18541121-0132", 
                        VisitStart = new DateTime(2022,7,9), 
                        VisitEnd = new DateTime(2022,7,18), 
                        Kin = null
                    },
                    new Camper
                    {
                        FirstName = "Ambrogio",
                        LastName = "Ratti",
                        PersonNumber = "18570531-0666",
                        VisitStart = new DateTime(2022,7,10),
                        VisitEnd = new DateTime(2022,7,15), 
                        Kin = null
                    },
                },
                Counselor = new Counselor
                {
                    FirstName = "Albino",
                    LastName = "Luciani",
                    PersonNumber = "19121017-0313",
                    PhoneNumber = "076-789 01 23",
                    InChargeStart = new DateTime(2022, 7, 1),
                    InChargeEnd = new DateTime(2022, 7, 31),
                }
            };
            cabins.Add(cabin3);
            var context = new CamperContext();
            context.AddRange(cabins);
            context.SaveChanges();
        }
    }
}