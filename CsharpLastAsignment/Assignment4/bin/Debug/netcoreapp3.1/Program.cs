using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;

namespace Vaccination
{
    public class Person
    {
        public DateTime Birthdate;
        public string Lastname;
        public string Firstname;
        public bool HealthcareWorker;
        public bool RiskGroup;
        public bool Infected;
        public string CompleteDateOfBirth;
    }
    public class Availabledose
    {
        public static int Doses = 0;
        public static int AvailableVaccindoses()
        {
            Console.WriteLine("Skriv in hur många doser som tillkommit: ");
            while (true)
            {
                try
                {
                    Doses += int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Testa igen med heltal.");
                }
            }
            return Doses;
        }
    }
    public class Indatafile
    {
        public static string In = @"C:\Windows\Temp\Assignment4\People.csv";
        public static string IndatafileChanger()
        {
            Console.Clear();
            Console.WriteLine("Skriv in filväg som du vill ändra indatafil till:");
            while (true)
            {
                In = Console.ReadLine();
                Console.Clear();

                if (File.Exists(In))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Filvägen existerar inte. Var god försök igen.");
                }
            }
            return In;
        }
    }
    public class Outdatafile
    {
        public static string Out = @"C:\Windows\Temp\Assignment4\Vaccinations.csv";

        public static string OutdataFolder()
        {
            while (true)
            {
                Console.WriteLine("Skriv in filväg som du vill ändra utdatafil till:");
                Out = Console.ReadLine();
                Console.Clear();

                string directoryName;

                directoryName = Path.GetDirectoryName(Out);

                if (Directory.Exists(directoryName))
                {
                    break;
                }
                else
                {
                    //Om filen inte finns som skrivs ner skall denna skapas.
                    Console.Clear();
                    Console.WriteLine("Filvägen existerar inte. Var god försök igen.");
                }
            }
            return Out;
        }
    }

    public class Program
    {
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            bool vaccinateChildren = false;

            bool done = false;
            while (!done)
            {
                string[] peopleCsvIndata = File.ReadAllLines(Indatafile.In);

                Console.WriteLine("HuvudMeny");
                Console.WriteLine("--------");
                Console.WriteLine("Antal tillgängliga vaccindoser: " + Availabledose.Doses);
                if (vaccinateChildren == false)
                {
                    Console.WriteLine("Vaccinering under 18 år: Nej");
                }
                else
                {
                    Console.WriteLine("Vaccinering under 18 år: Ja");
                }
                Console.WriteLine($"Indatafil: {Indatafile.In}");
                Console.WriteLine($"Utdatafil: {Outdatafile.Out}");
                Console.WriteLine();

                int answer = ShowMenu("Vad vill du göra?", new[]
                {
                    "Skapa prioritetsordning",
                    "Ändra antal vaccindoser",
                    "Ändra åldersgräns",
                    "Ändra indatafil",
                    "Ändra utdatafil",
                    "Avsluta",
                });
                if (answer == 0)
                {
                    if (File.Exists(Outdatafile.Out))
                    {
                        Console.Clear();
                        int jaOrNej = ShowMenu("Är du säker på att du vill skriva över filen?", new[]
                        {
                        "Ja",
                        "Nej",
                        });

                        if (jaOrNej == 0)
                        {
                            string[] final = CreateVaccinationOrder(peopleCsvIndata, Availabledose.Doses, vaccinateChildren);
                            if (final == null)
                            {

                            }
                            else
                            {
                                final.ToString();
                                File.WriteAllLines(Outdatafile.Out, final);
                            }
                        }
                    }
                    else
                    {
                        string[] final = CreateVaccinationOrder(peopleCsvIndata, Availabledose.Doses, vaccinateChildren);
                        if (final == null)
                        {

                        }
                        else
                        {
                            final.ToString();
                            File.WriteAllLines(Outdatafile.Out, final);
                        }
                    }
                    ;
                }
                else if (answer == 1)
                {
                    Console.Clear();
                    Availabledose.AvailableVaccindoses();
                    Console.Clear();
                }
                else if (answer == 2)
                {
                    Console.Clear();
                    vaccinateChildren = UnderAgeVaccination(vaccinateChildren);
                }
                else if (answer == 3)
                {
                    Console.Clear();
                    Indatafile.IndatafileChanger();
                }
                else if (answer == 4)
                {
                    Console.Clear();
                    Outdatafile.OutdataFolder();
                }
                else if (answer == 5)
                {
                    done = true;
                }
            }
        }
        public static string[] CreateVaccinationOrder(string[] input, int availableDoses, bool vaccinateChildren)
        {
            List<Person> peopleToVaccinate = new List<Person>();
            List<Person> priorityOfVaccination = new List<Person>();
            List<Person> phase1 = new List<Person>();
            List<Person> phase2 = new List<Person>();
            List<Person> phase3 = new List<Person>();
            List<Person> phase4 = new List<Person>();
            List<Person> vaccinationOrder = new List<Person>();
            List<string> returnOrder = new List<string>();
            int errorCounter = 0;
            int row = 1;
            foreach (string errorpersons in input)
            {
                string[] parts = errorpersons.Split(',');

                if (parts[0].Contains('-'))
                {
                }
                else
                {
                    parts[0] = parts[0].Insert(parts[0].Length - 4, "-");
                }

                if (parts[0].Length == 11)
                {
                    parts[0] = "19" + parts[0];
                }

                if (parts.Length != 6)
                {
                    errorCounter++;
                    Console.WriteLine("Rad " + row + " var inmatad på fel sätt. Titta över din indatafil och försök igen.");
                }
                else if (parts[3] != "1" && parts[3] != "0")
                {
                    errorCounter++;
                    Console.WriteLine("Rad " + row + " var inmatad på fel sätt. Titta över din indatafil och försök igen.");
                }
                else if (parts[4] != "1" && parts[4] != "0")
                {
                    errorCounter++;
                    Console.WriteLine("Rad " + row + " var inmatad på fel sätt. Titta över din indatafil och försök igen.");
                }
                else if (parts[5] != "1" && parts[5] != "0")
                {
                    errorCounter++;
                    Console.WriteLine("Rad " + row + " var inmatad på fel sätt. Titta över din indatafil och försök igen.");
                }

                foreach (char c in parts[0])
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '-')
                    {
                    }
                    else
                    {
                        errorCounter++;
                        Console.WriteLine("Rad " + row + " var inmatad på fel sätt. Titta över din indatafil och försök igen.");
                    }
                }

                row++;
            }
            if (errorCounter != 0)
            {
                return null;
            }

            foreach (string personsToTreat in input)
            {
                string[] parts = personsToTreat.Split(',');
                if (parts[0].Contains('-'))
                {

                }
                else
                {
                    parts[0] = parts[0].Insert(parts[0].Length - 4, "-");
                }

                if (parts[0].Length == 11)
                {
                    parts[0] = "19" + parts[0];
                }

                string dateOfBirth = parts[0].Substring(0, 8);
                string year = dateOfBirth.Substring(0, 4);
                string month = dateOfBirth.Substring(4, 2);
                string day = dateOfBirth.Substring(6, 2);
                string completeDateOfBirth = parts[0].Substring(0, 13);
                string lastname = parts[1];
                string firstname = parts[2];
                DateTime birthDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                bool healthcareWorker;
                if (parts[3] == "1")
                {
                    healthcareWorker = true;
                }
                else
                {
                    healthcareWorker = false;
                }

                bool riskGroup;
                if (parts[4] == "1")
                {
                    riskGroup = true;
                }
                else
                {
                    riskGroup = false;
                }

                bool infected;
                if (parts[5] == "1" && Availabledose.Doses > 1)
                {
                    infected = true;
                }
                else
                {
                    infected = false;
                }

                if (Availabledose.Doses <= 1)
                {
                    Console.WriteLine("Alla patienter kunde inte vaccineras.");
                }

                Person person = new Person()
                {
                    Birthdate = birthDate,
                    Lastname = lastname,
                    Firstname = firstname,
                    HealthcareWorker = healthcareWorker,
                    RiskGroup = riskGroup,
                    Infected = infected,
                    CompleteDateOfBirth = completeDateOfBirth
                };
                peopleToVaccinate.Add(person);

                if (vaccinateChildren == true)
                {
                    // Resterande del utav metoden har vi behövt använda 'OrderBy(p => p.Birthdate)' på flera ställen, mer än vad vi tycker borde behövas.
                    // Tar vi bort det på något ställe fungerar inte koden, vi rådfrågade dig om detta men har inte lyckats lösa det på ett snyggare sätt.
                    //fas 1
                    var moveables1 = peopleToVaccinate.Where(p => p.HealthcareWorker == true);
                    foreach (Person p in moveables1.OrderBy(p => p.Birthdate))
                    {
                        phase1.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.HealthcareWorker == true);
                    }
                    //fas 2
                    var moveables2 = peopleToVaccinate.Where(p => p.Birthdate <= DateTime.Now.AddYears(-65));
                    foreach (Person p in moveables2.OrderBy(p => p.Birthdate <= DateTime.Now.AddYears(-65)))
                    {
                        phase2.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.Birthdate <= DateTime.Now.AddYears(-65));
                    }
                    //fas 3
                    var moveables3 = peopleToVaccinate.Where(p => p.RiskGroup == true);
                    foreach (Person p in moveables3.OrderBy(p => p.Birthdate))
                    {
                        phase3.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.RiskGroup == true);
                    }
                    //fas 4
                    var moveables4 = peopleToVaccinate.OrderBy(p => p.Birthdate);
                    foreach (Person p in moveables4.OrderBy(p => p.Birthdate))
                    {
                        phase4.Add(p);
                        peopleToVaccinate.Clear();
                    }
                }
                else
                {
                    peopleToVaccinate.RemoveAll(p => p.Birthdate >= DateTime.Now.AddYears(-18));
                    //fas 1
                    var moveables1 = peopleToVaccinate.Where(p => p.HealthcareWorker == true);
                    foreach (Person p in moveables1.OrderBy(p => p.Birthdate))
                    {
                        phase1.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.HealthcareWorker == true);
                    }
                    //fas 2
                    var moveables2 = peopleToVaccinate.Where(p => p.Birthdate <= DateTime.Now.AddYears(-65));
                    foreach (Person p in moveables2.OrderBy(p => p.Birthdate <= DateTime.Now.AddYears(-65)))
                    {
                        phase2.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.Birthdate <= DateTime.Now.AddYears(-65));
                    }
                    //fas 3
                    var moveables3 = peopleToVaccinate.Where(p => p.RiskGroup == true);
                    foreach (Person p in moveables3.OrderBy(p => p.Birthdate))
                    {
                        phase3.Add(p);
                        peopleToVaccinate.RemoveAll(p => p.RiskGroup == true);
                    }
                    //fas 4
                    var moveables4 = peopleToVaccinate.OrderBy(p => p.Birthdate);
                    foreach (Person p in moveables4.OrderBy(p => p.Birthdate))
                    {
                        phase4.Add(p);
                        peopleToVaccinate.Clear();
                    }
                }
            }

            foreach (Person p in phase1.OrderBy(p => p.Birthdate))
            {
                vaccinationOrder.Add(p);
            }

            foreach (Person p in phase2.OrderBy(p => p.Birthdate))
            {
                vaccinationOrder.Add(p);
            }

            foreach (Person p in phase3.OrderBy(p => p.Birthdate))
            {
                vaccinationOrder.Add(p);
            }

            foreach (Person p in phase4.OrderBy(p => p.Birthdate))
            {
                vaccinationOrder.Add(p);
            }

            foreach (Person p in vaccinationOrder)
            {
                if (p.Infected == true && Availabledose.Doses >= 1)
                {
                    Availabledose.Doses -= 1;
                    returnOrder.Add(p.CompleteDateOfBirth + "," + p.Lastname + "," + p.Firstname + ",1");

                }
                else if (p.Infected == false && Availabledose.Doses > 1)
                {
                    Availabledose.Doses -= 2;
                    returnOrder.Add(p.CompleteDateOfBirth + "," + p.Lastname + "," + p.Firstname + ",2");
                }
                else
                {
                    Console.WriteLine("Det finns inte tillräckligt med doser för att forsätta vaccinera patienter.");
                }
            }
            return returnOrder.ToArray();
        }

        public static bool UnderAgeVaccination(bool underEightteen)
        {
            Console.Clear();
            int yesOrNo = ShowMenu("Får personer under 18 år vaccineras?", new[]
            {
                    "Ja",
                    "Nej",
                    });
            if (yesOrNo == 0)
            {
                underEightteen = true;
            }
            else
            {
                underEightteen = false;
            }
            Console.Clear();
            return underEightteen;
        }

        public static int ShowMenu(string prompt, IEnumerable<string> options)
        {
            if (options == null || options.Count() == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty list of options.");
            }

            Console.WriteLine(prompt);

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            // Calculate the width of the widest option so we can make them all the same width later.
            int width = options.Max(option => option.Length);

            int selected = 0;
            int top = Console.CursorTop;
            for (int i = 0; i < options.Count(); i++)
            {
                // Start by highlighting the first option.
                if (i == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                var option = options.ElementAt(i);
                // Pad every option to make them the same width, so the highlight is equally wide everywhere.
                Console.WriteLine("- " + option.PadRight(width));

                Console.ResetColor();
            }
            Console.CursorLeft = 0;
            Console.CursorTop = top - 1;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(intercept: true).Key;

                // First restore the previously selected option so it's not highlighted anymore.
                Console.CursorTop = top + selected;
                string oldOption = options.ElementAt(selected);
                Console.Write("- " + oldOption.PadRight(width));
                Console.CursorLeft = 0;
                Console.ResetColor();

                // Then find the new selected option.
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Count() - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }

                // Finally highlight the new selected option.
                Console.CursorTop = top + selected;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                string newOption = options.ElementAt(selected);
                Console.Write("- " + newOption.PadRight(width));
                Console.CursorLeft = 0;
                // Place the cursor one step above the new selected option so that we can scroll and also see the option above.
                Console.CursorTop = top + selected - 1;
                Console.ResetColor();
            }

            // Afterwards, place the cursor below the menu so we can see whatever comes next.
            Console.CursorTop = top + options.Count();

            // Show the cursor again and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }

    }

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void BirthdateFormatTest()
        {
            string[] input =
            {
                "810203-2222,Efternamnsson,Eva,1,1,0",
                "9310051336,Larsson,Martin,1,1,1",
                "197209061111,Elba,Idris,1,0,1"
            };
            Availabledose.Doses = 10;
            bool vaccinateChildren = true;

            string[] output = Program.CreateVaccinationOrder(input, Availabledose.Doses, vaccinateChildren);

            Assert.AreEqual(output.Length, 3);
            Assert.AreEqual("19720906-1111,Elba,Idris,1", output[0]);
            Assert.AreEqual("19810203-2222,Efternamnsson,Eva,2", output[1]);
            Assert.AreEqual("19931005-1336,Larsson,Martin,1", output[2]);
        }
        [TestMethod]
        public void LargeGroupTest()
        {
            string[] input =
            {
                "19720906-1111,Elba,Idris,0,0,1",
                "951005-1337,Carter,Paul,1,0,1",
                "199310051336,Matin,Matin,1,1,1",
                "20100715-1234,Kidson,Kid,0,1,0",
                "19841111-2341,Selacovic,Stefan,0,1,1",
                "19641004-1872,Svensson,Ruben,1,1,1",
                "20071113-4321,Chadson,Chad,0,0,1",
                "20120301-2314,Derpalee,Derp,1,0,0",
                "8102032222,Efternamnsson,Eva,1,1,0",
                "19931003-2222,Först,Före,0,0,0",
                "19951003-1111,Sist,Sista,0,0,0"
            };
            Availabledose.Doses = 100;
            bool vaccinateChildren = true;

            string[] output = Program.CreateVaccinationOrder(input, Availabledose.Doses, vaccinateChildren);

            Assert.AreEqual(output.Length, 11);
            Assert.AreEqual("19641004-1872,Svensson,Ruben,1", output[0]);
            Assert.AreEqual("19810203-2222,Efternamnsson,Eva,2", output[1]);
            Assert.AreEqual("19931005-1336,Matin,Matin,1", output[2]);
            Assert.AreEqual("19951005-1337,Carter,Paul,1", output[3]);
            Assert.AreEqual("20120301-2314,Derpalee,Derp,2", output[4]);
            Assert.AreEqual("19841111-2341,Selacovic,Stefan,1", output[5]);
            Assert.AreEqual("20100715-1234,Kidson,Kid,2", output[6]);
            Assert.AreEqual("19720906-1111,Elba,Idris,1", output[7]);
            Assert.AreEqual("19931003-2222,Först,Före,2", output[8]);
            Assert.AreEqual("19951003-1111,Sist,Sista,2", output[9]);
            Assert.AreEqual("20071113-4321,Chadson,Chad,1", output[10]);
        }
        [TestMethod]
        public void NoChildrenTest()
        {
            string[] input =
            {
                "19720906-1111,Elba,Idris,0,0,1",
                "951005-1337,Carter,Paul,1,0,1",
                "199310051336,Matin,Matin,1,1,1",
                "20100715-1234,Kidson,Kid,0,1,0",
                "19841111-2341,Selacovic,Stefan,0,1,1",
                "19641004-1872,Svensson,Ruben,1,1,1",
                "20071113-4321,Chadson,Chad,0,0,1",
                "20120301-2314,Derpalee,Derp,1,0,0",
                "8102032222,Efternamnsson,Eva,1,1,0",
                "19931003-2222,Först,Före,0,0,0",
                "19951003-1111,Sist,Sista,0,0,0"
            };
            Availabledose.Doses = 100;
            bool vaccinateChildren = false;

            string[] output = Program.CreateVaccinationOrder(input, Availabledose.Doses, vaccinateChildren);

            Assert.AreEqual(output.Length, 8);
            Assert.AreEqual("19641004-1872,Svensson,Ruben,1", output[0]);
            Assert.AreEqual("19810203-2222,Efternamnsson,Eva,2", output[1]);
            Assert.AreEqual("19931005-1336,Matin,Matin,1", output[2]);
            Assert.AreEqual("19951005-1337,Carter,Paul,1", output[3]);
            Assert.AreEqual("19841111-2341,Selacovic,Stefan,1", output[4]);
            Assert.AreEqual("19720906-1111,Elba,Idris,1", output[5]);
            Assert.AreEqual("19931003-2222,Först,Före,2", output[6]);
            Assert.AreEqual("19951003-1111,Sist,Sista,2", output[7]);
        }
        [TestMethod]
        public void IncompleteIndataTest()

        {
            string[] input =
            {
                "19720906-111X,Elba,Idris,0,0,1",
                "951005-1337,Carter,Paul,1,0,1",
                "199310051336,Matin,1,1,1",
                "19841111-2341,Selacovic,Stefan,0,1,1",
                "19641004-1872,Svensson,Ruben,1,1",
                "8102032222,Efternamnsson,Eva,1,1,0",
                "19931003-2222,Först,Före,2,0,0",
                "19951003-1111,Sist,Sista,0,0,0"
            };
            Availabledose.Doses = 100;
            bool vaccinateChildren = false;

            string[] output = Program.CreateVaccinationOrder(input, Availabledose.Doses, vaccinateChildren);

            Assert.AreEqual(output, null);
        }
        [TestMethod]
        public void ShortageDosesTest()
        {
            string[] input =
            {
                "19720906-1111,Elba,Idris,0,0,1",
                "951005-1337,Carter,Paul,1,0,1",
                "199310051336,Matin,Matin,1,1,1",
                "19841111-2341,Selacovic,Stefan,0,1,1",
                "19641004-1872,Svensson,Ruben,1,1,1",
                "8102032222,Efternamnsson,Eva,1,1,0",
                "19931003-2222,Först,Före,0,0,0",
                "19951003-1111,Sist,Sista,0,0,0"
            };
            Availabledose.Doses = 7;
            bool vaccinateChildren = true;

            string[] output = Program.CreateVaccinationOrder(input, Availabledose.Doses, vaccinateChildren);

            Assert.AreEqual(output.Length, 6);
        }
    }
}