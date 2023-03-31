using Lab2CarterAndMastromonaco.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Model
{
    public class NextOfKin
    {
        [Key]
        public int KinId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }

        public Camper Camper { get; set; }

        public static void Display(string s)
        {
            using (SqlConnection connection = new SqlConnection(s))
            {
                var person = new CamperContext();
                var persons = person.NextOfKins.Include(c => c.Camper).ToArray();

                Console.WriteLine("KinId\t | FirstName\t | LastName\t | Relation\t | CamperId");
                foreach (NextOfKin p in persons)
                {
                    Console.WriteLine("{0, -9}| {1, -14}| {2, -14}| {3, -14}| {4, 0}",
                        p.KinId, p.FirstName, p.LastName, p.Relation, p.Camper.CamperId);
                }
            }
            Console.WriteLine();
        }
        public static void CreatKin(string s)
        {
            bool done = false;
            while (!done)
            {

                Console.Clear();
                Camper.Display(s);
                Console.WriteLine("Ange CamperId:");
                int camperID = int.Parse(Console.ReadLine());

                var context = new CamperContext();
                var camper = context.Campers.FirstOrDefault(c => c.CamperId == camperID);
                if (camper != null)
                {
                    if (camper.Kin == null)
                    {
                        camper.Kin = new List<NextOfKin>();
                    }
                    Console.WriteLine("Förnamn:");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("Efternamn:");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("Relation");
                    string relation = Console.ReadLine();
                    //Detta tog lång tid att få att fungera. Tillslut kom vi på att vi var tvugna att skapa en ny lista för att lägga till en KIN.
                    List<NextOfKin> newKin = new List<NextOfKin>();
                    var nKin = new NextOfKin
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Relation = relation,
                        Camper = camper
                    };
                    newKin.Add(nKin);
                    context.AddRange(newKin);
                    context.SaveChanges();
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("Det finns ingen Camper med det id nummret.");
                }
            }
        }
    }
}