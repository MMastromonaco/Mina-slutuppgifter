using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Function
{
    internal class SendToArchive
    {
        //Här i ligger metoden som tar bort den personen som vi väljer med ID nummret. Innan den tar bort den så sparas den in i den historiska tablen i databasen.
        public static void SendToTheArchive(string s)
        {
            int minimenu = Menu.ShowMenu("Vem vill du flytta till historikfilen?", new[]
            {
                        "Counselor",
                        "Camper",
            });
            if (minimenu == 0)
            {
                using (var context = new CamperContext())
                {
                    Console.Clear();
                    Counselor.Display(s);
                    Console.WriteLine("Skriv in Id nummret på den counselor du vill arkivera:");
                    int a = int.Parse(Console.ReadLine());
                    var counselor = context.Counselors.FirstOrDefault(c=> c.CounselorId == a);
                    if (counselor != null)
                    {
                        var past = new PastCamperAndCounselor
                        {
                            FirstName = counselor.FirstName,
                            LastName = counselor.LastName,
                            PersonNumber = counselor.PersonNumber,
                            CamperOrCounselor = "Counselor",
                            InChargeStart = counselor.InChargeStart,
                            InChargeEnd = counselor.InChargeEnd,
                        };
                        context.PastCamperAndCounselors.Add(past);
                        context.Counselors.Remove(counselor);
                        context.SaveChanges();
                    }
                }
            }
            else if (minimenu == 1)
            {
                using (var context = new CamperContext())
                {
                    Console.Clear();
                    Camper.Display(s);
                    Console.WriteLine("Skriv in Id nummret på den camper du vill arkivera:");
                    int a = int.Parse(Console.ReadLine());
                    var camper = context.Campers.FirstOrDefault(c => c.CamperId == a);
                    if (camper != null)
                    {
                        var past = new PastCamperAndCounselor
                        {
                            FirstName = camper.FirstName,
                            LastName = camper.LastName,
                            PersonNumber = camper.PersonNumber,
                            CamperOrCounselor = "Camper",
                            VisitStart = camper.VisitStart,
                            VisitEnd = camper.VisitEnd,
                        };
                        var nextOfKin = context.NextOfKins.Where(nok => nok.Camper == camper);
                        context.NextOfKins.RemoveRange(nextOfKin);
                        context.PastCamperAndCounselors.Add(past);
                        context.Campers.Remove(camper);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}