using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Function;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Model
{
    public class Counselor
    {
        [Key]
        public int CounselorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime InChargeStart { get; set; }
        public DateTime InChargeEnd { get; set; }

        public int? CabinId { get; set; }
        public Cabin Cabin { get; set; }
        public ICollection<Camper> Campers { get; set; }

        public static void CreateCounselor()
        {
            Console.WriteLine("Förnamn:");
            string firstNameCounselor = Console.ReadLine();
            Console.WriteLine("Efternamn:");
            string lastNameCounselor = Console.ReadLine();
            Console.WriteLine("Personnummer:");
            string personNrCounselor = Console.ReadLine();
            Console.WriteLine("Mobilnummer:");
            string mobileNrCounselor = Console.ReadLine();
            Console.WriteLine("Ansvarig från och med (YYYY-MM-DD):");
            DateTime startCounselor = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ansvar upphör(YYYY-MM-DD):");
            DateTime endCounselor = DateTime.Parse(Console.ReadLine());

            var newCounselor = new Counselor()
            {
                FirstName = firstNameCounselor,
                LastName = lastNameCounselor,
                PersonNumber = personNrCounselor,
                PhoneNumber = mobileNrCounselor,
                InChargeStart = startCounselor,
                InChargeEnd = endCounselor
            };

            var context = new CamperContext();
            context.Add(newCounselor);
            context.SaveChanges();
            Console.Clear();
        }
        public static void ConnectCounselorToCabin(string s)
        {
            bool done = false;
            while (!done)
            {
                var context = new CamperContext();
                var counselors = context.Counselors.Include(c => c.Cabin).ToList();
                var cabins = context.Cabins.Include(c => c.Counselor).ToList();
                var campers = context.Campers.ToList();

                Counselor.Display(s);
                Console.WriteLine();
                Console.WriteLine("Välj Counselor (CounselorId):");
                int idCounslelor = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Cabin.Display(s);
                Console.WriteLine();
                Console.WriteLine("Vilken Cabin skall hen ansvara över (CabinId)?\nMata in 0 om vald Counselor inte skall ansvara för en Cabin längre.");
                int idCabin = int.Parse(Console.ReadLine());

                var chosenCounselor = context.Counselors.Find(idCounslelor);

                if (idCabin != 0)
                {
                    chosenCounselor.CabinId = idCabin;
                    context.Update(chosenCounselor);
                    context.SaveChanges();
                    done = true;
                }
                else
                {
                    var chosenCabin = context.Cabins.Find(idCabin);
                    var containsCampers = campers.Any(c => c.CabinId == chosenCounselor.CabinId);
                    if (!containsCampers)
                    {
                        chosenCounselor.CabinId = null;
                        context.Update(chosenCounselor);
                        context.SaveChanges();
                        done = true;
                    }
                    else
                    {
                        Console.WriteLine("Det finns Campers i stugan. Du kan inte lämna stugan utan en Counselor.");
                    }
                }
            }
        }



        public static void Display(string s)
        {
            using (SqlConnection connection = new SqlConnection(s))
            {
                var person = new CamperContext();
                var persons = person.Counselors.ToArray();

                Console.WriteLine("CounselorId\t | FirstName\t | LastName\t | PersonNumber\t | Phonenumber \t | Inchargestart\t\t | Inchargeend\t\t | CabinID\t");
                foreach (Counselor p in persons)
                {
                    Console.WriteLine("{0, -17}| {1, -14}| {2, -14}| {3, -14}| {4, -14}| {5, -30}| {6, -22}| {7, -10}",
                        p.CounselorId, p.FirstName, p.LastName, p.PersonNumber, p.PhoneNumber, p.InChargeStart, p.InChargeEnd, p.CabinId);
                }
            }
            Console.WriteLine();
        }
    }
}