using Lab2CarterAndMastromonaco.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Model
{
    public class Camper
    {
        [Key]
        public int CamperId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        public string? Allergies { get; set; }
        public DateTime VisitStart { get; set; }
        public DateTime VisitEnd { get; set; }

        public int? CabinId { get; set; }
        public Cabin Cabin { get; set; }

        public ICollection<NextOfKin> Kin { get; set; }

        public static void Display(string s)
        {
            using (SqlConnection connection = new SqlConnection(s))
            {
                var person = new CamperContext();
                var persons = person.Campers.ToArray();

                Console.WriteLine("CamperId\t | FirstName\t | LastName\t\t | PersonNumber\t | Allergies \t | VisitStart\t\t | VisitEnd\t\t | CabinId\t");
                foreach (Camper p in persons)
                {
                    Console.WriteLine("{0, -17}| {1, -14}| {2, -22}| {3, -14}| {4, -14}| {5, -22}| {6, -22}| {7, -10}",
                        p.CamperId, p.FirstName, p.LastName, p.PersonNumber, p.Allergies, p.VisitStart, p.VisitEnd, p.CabinId);
                }
            }
            Console.WriteLine();


        }

        public static void CreateCamper()
        {
            Console.WriteLine("Förnamn:");
            string firstNameCamper = Console.ReadLine();
            Console.WriteLine("Efternamn:");
            string lastNameCamper = Console.ReadLine();
            Console.WriteLine("Personnummer:");
            string personNrCamper = Console.ReadLine();
            Console.WriteLine("Allergier:");
            string allergies = Console.ReadLine();
            Console.WriteLine("Deltagare från och med (YYYY-MM-DD):");
            DateTime startCamper = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Åker hem (YYYY-MM-DD):");
            DateTime endCamper = DateTime.Parse(Console.ReadLine());

            var newCamper = new Camper()
            {
                FirstName = firstNameCamper,
                LastName = lastNameCamper,
                PersonNumber = personNrCamper,
                Allergies = allergies,
                VisitStart = startCamper,
                VisitEnd = endCamper
            };

            var context = new CamperContext();
            context.Add(newCamper);
            context.SaveChanges();
            Console.Clear();
        }

        public static void ConnectCamperToCabin(string s)
        {
            bool done = false;
            while (!done)
            {

                var context = new CamperContext();
                var campers = context.Campers.Include(c => c.Cabin).AsNoTracking().ToList();
                var counselors = context.Counselors.ToList();
                var cabins = context.Cabins.Include(c => c.Counselor).ToList();


                Camper.Display(s);
                Console.WriteLine();
                Console.WriteLine("Välj Camper (CamperId):");
                int idCamper = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Cabin.Display(s);
                Console.WriteLine("Vilken Cabin skall hen bli placerad i (CabinId)?\nMata in 0 om vald Camper inte skall bo i vald Cabin längre.");
                int idCabin = int.Parse(Console.ReadLine());
                Console.WriteLine();
                var chosenCamper = context.Campers.Where(c => c.CamperId == idCamper).AsNoTracking().FirstOrDefault();

               //Här kollar den så att det finns en ansvarig i stugan som vi försöker placera campern i. om inte så syns felmeddelandet på rad 110.
                if (idCabin != 0)
                {
                    var chosenCabin = cabins.Where(c => c.CabinId == idCabin).FirstOrDefault();
                    if (chosenCabin.Counselor == null)
                    {
                        Console.WriteLine("Det finns ingen ansvarig Counselor i denna Cabin, var vänlig välj en annan Cabin.");
                    }
                    else
                    {
                        int spaceInCabin = 0;
                        foreach (Camper c in campers)
                        {
                            if (idCabin == c.CabinId)
                            {
                                spaceInCabin++;
                            }
                        }
                        //Här kollar vi så att cabinen inte är full. om den är det så kommer felmeddelandet på rad 139 att synas.
                        if (spaceInCabin < 4)
                        {
                            Camper updateCamper = new Camper();
                            updateCamper.CamperId = chosenCamper.CamperId;
                            updateCamper.FirstName = chosenCamper.FirstName;
                            updateCamper.LastName = chosenCamper.LastName;
                            updateCamper.PersonNumber = chosenCamper.PersonNumber;
                            updateCamper.Allergies = chosenCamper.Allergies;
                            updateCamper.CabinId = idCabin;

                            context.Update(updateCamper);
                            context.SaveChanges();
                            done = true;
                        }
                        else
                        {
                            Console.WriteLine("Denna Cabin är för tillfället full. Var god välj en annan Cabin.");
                        }
                    }
                }
                else
                {
                    chosenCamper.CabinId = null;
                    context.Update(chosenCamper);
                    context.SaveChanges();
                    done=true;
                }
            }

        }
        //Denna methoden är till för att visa campers under en specifik counselor.
        public static void ShowCampersUnderCounselor()
        {
            using (var context = new CamperContext())
            {
                //Här väljer du vilken counselor du vill kolla
                var counselors = context.Counselors.Include(c => c.Cabin).ToList();
                Console.WriteLine("Counselors:\nFirstname\t | Lastname\t | CounselorId\t | CabinId");
                foreach (Counselor c in counselors)
                {
                    Console.WriteLine("{0, -17}| {1, -14}| {2, -14}| {3, 0}", c.FirstName, c.LastName, c.CounselorId, c.CabinId);
                }
                Console.WriteLine();
                Console.WriteLine("Skriv counsler id för att titta vilka campers hen har hand om");
                //Och här visas de campers som bor i vald counselors stuga.
                var counselorId = int.Parse(Console.ReadLine());
                Console.WriteLine();
                var campers = context.Counselors
                    .Include(ca => ca.Cabin.Campers)
                    .Where(ca => ca.CounselorId == counselorId)
                    .SelectMany(ca => ca.Cabin.Campers)
                    .ToList();

                Console.WriteLine("Campers:\nFirstname\t | Lastname\t\t | CamperId\t | CabinId");
                foreach (Camper ca in campers)
                {
                    Console.WriteLine("{0, -17}| {1, -22}| {2, -14}| {3, 0}", ca.FirstName, ca.LastName, ca.CamperId, ca.CabinId);
                }
            }
            Console.WriteLine();
        }
    }
}