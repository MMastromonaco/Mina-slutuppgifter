using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Function;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Model
{
    public class Cabin
    {
        [Key]
        public int CabinId { get; set; }
        public string CabinName { get; set; }
        public int CabinCapacity { get; set; }

        public ICollection<Camper> Campers { get; set; }
        
        public Counselor Counselor { get; set; }



        //Alla våra model klasser innehåller en specifik Display method som ser lite olika ut. Men i stort är det samma metoder.
        public static void Display(string s)
        {
            using (SqlConnection connection = new SqlConnection(s))
            {

                var cabin = new CamperContext();
                var cabins = cabin.Cabins.ToArray();

                Console.WriteLine("CabinId\t | Cabin name\t\t | Capacity\t");
                foreach (Cabin c in cabins)
                {
                    Console.WriteLine("{0, -9}| {1, -22}| {2, 0}", c.CabinId, c.CabinName, c.CabinCapacity);
                }
            }
        }
        //Här skapar vi en stuga som vi sparar i databasen.
        public static void CreateCabin()
        {
            Console.WriteLine("Cabin namn:");
            string newCabinName = Console.ReadLine();
            Console.WriteLine("Kapacitet:");
            int capacity = int.Parse(Console.ReadLine());

            var newCabin = new Cabin()
            {
                CabinName = newCabinName,
                CabinCapacity = capacity
            };

            var context = new CamperContext();
            context.Add(newCabin);
            context.SaveChanges();
            Console.Clear();
        }
    }
}