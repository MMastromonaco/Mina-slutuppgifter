using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Function
{
    internal class AddHistoryPeople
    {
        //Här ligger våran seed-data för PastCampersAndCounselors.
        public static void HistoryOfCamp()
        {
            List<PastCamperAndCounselor> history = new List<PastCamperAndCounselor>();
            var person1 = new PastCamperAndCounselor
            {
                FirstName = "Angelo",
                LastName = "Roncalli",
                PersonNumber = "18810625-6212",
                VisitStart = new DateTime(2022, 7, 10),
                VisitEnd = new DateTime(2022, 7, 20),
                CamperOrCounselor = "Camper"
            };
            history.Add(person1);

            var person2 = new PastCamperAndCounselor
            {
                FirstName = "Giovanni",
                LastName = "Montini",
                PersonNumber = "18970926-2167",
                VisitStart = new DateTime(2022, 7, 7),
                VisitEnd = new DateTime(2022, 7, 17),
                CamperOrCounselor = "Camper"
            };
            history.Add(person2);

            var person3 = new PastCamperAndCounselor
            {
                FirstName = "Karol",
                LastName = "Wojtyla",
                PersonNumber = "19200518-6566",
                VisitStart = new DateTime(2022, 7, 10),
                VisitEnd = new DateTime(2022, 7, 18),
                CamperOrCounselor = "Camper"
            };
            history.Add(person3);

            var person4 = new PastCamperAndCounselor
            {
                FirstName = "Joseph",
                LastName = "Ratzinger",
                PersonNumber = "19270416-4162",
                VisitStart = new DateTime(2022, 7, 10),
                VisitEnd = new DateTime(2022, 7, 20),
                CamperOrCounselor = "Camper"
            };
            history.Add(person4);

            var person5 = new PastCamperAndCounselor
            {
                FirstName = "Jorge",
                LastName = "Bergogoglie",
                PersonNumber = "19361217-2032",
                VisitStart = new DateTime(2022, 7, 10),
                VisitEnd = new DateTime(2022, 7, 20),
                CamperOrCounselor = "Camper"
            };
            history.Add(person5);

            var person6 = new PastCamperAndCounselor
            {
                FirstName = "Eugenio",
                LastName = "Pacelli",
                PersonNumber = "18760302-6660",
                VisitStart = new DateTime(2022, 7, 12),
                VisitEnd = new DateTime(2022, 7, 20),
                CamperOrCounselor = "Camper"
            };
            history.Add(person6);

            var person7 = new PastCamperAndCounselor
            {
                FirstName = "Donald",
                LastName = "Dump",
                PersonNumber = "18000321-1234",
                InChargeStart = new DateTime(2010, 7, 10),
                InChargeEnd = new DateTime(2012, 7, 10),
                CamperOrCounselor = "Counselor"
            };
            history.Add(person7);

            var context = new CamperContext();
            context.AddRange(history);
            context.SaveChanges();
        }
    }
}