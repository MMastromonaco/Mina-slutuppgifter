using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Model
{
    public class PastCamperAndCounselor
    {
        [Key]
        public int HistoryId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        public string CamperOrCounselor { get; set; }
        public DateTime? VisitStart { get; set; }
        public DateTime? VisitEnd { get; set; }
        public DateTime? InChargeStart { get; set; }
        public DateTime? InChargeEnd { get; set; }



        public static void Display(string s)
        {
            using (SqlConnection connection = new SqlConnection(s))
            {
                var person = new CamperContext();
                var persons = person.PastCamperAndCounselors.ToArray();
                connection.Open();
                
                Console.WriteLine("HistoryId\t | FirstName\t | LastName\t | PersonNumber\t | CamperOrCounselor    \t | VisitStart          \t | VisitEnd          \t | InChargeStart          \t | InChargeEnd          \t");
                foreach (PastCamperAndCounselor p in persons)
                {
                    Console.WriteLine("{0,-19}{1,-16}{2,-16}{3,-16}{4,-32}{5,-24}{6,-24}{7,-32}{8,0}",
                        p.HistoryId, p.FirstName, p.LastName, p.PersonNumber, p.CamperOrCounselor, p.VisitStart, p.VisitEnd, p.InChargeStart, p.InChargeEnd);
                }
            }
        }
    }
}