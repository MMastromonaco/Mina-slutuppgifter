using Lab2CarterAndMastromonaco.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Function
{
    internal class Menu
    {
        public static void MainMenu(string s)
        {
            //Vi valde att lägga hela våran meny i denna klassen inklusive methoden "ShowMenu"
            bool done = false;
            while (!done)
            {
                Console.Clear();
                int menu = ShowMenu("Välj", new[]
                {
                    "Sök efter Camper (baserat på Cabin eller Counselor)",
                    "Visa nuvarande Campers/Counselors och NextOfKins",
                    "Visa tidigare Campers/Counselors",
                    "Visa Cabins",
                    "Lägg till/Ta bort/Koppla samman",
                    "Avsluta"
                });
                if (menu == 0)
                {
                    Console.Clear();
                    Camper.ShowCampersUnderCounselor();
                    Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                    Console.ReadLine();
                }
                else if (menu == 1)
                {
                    Console.Clear();
                    int witchone = ShowMenu("Vad vill du titta på?", new[]
                    {
                        "Campers",
                        "Counselor",
                        "NextOfKins",
                        "Återgå till huvudmenyn"
                    });
                    Console.WriteLine();
                    if (witchone == 0)
                    {
                        Camper.Display(s);
                        Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                        Console.ReadLine();
                    }
                    else if (witchone == 1)
                    {
                        Counselor.Display(s);
                        Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                        Console.ReadLine();
                    }
                    else if (witchone == 2)
                    {
                        NextOfKin.Display(s);
                        Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                        Console.ReadLine();
                    }
                    else if (witchone == 3)
                    {
                        Console.Clear();
                    }
                }
                else if (menu == 2)
                {
                    Console.Clear();
                    PastCamperAndCounselor.Display(s);
                    Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                    Console.ReadLine();
                }
                else if (menu == 3)
                {
                    Console.Clear();
                    Cabin.Display(s);
                    Console.WriteLine("Återgå till huvudmenyn genom att trycka enter");
                    Console.ReadLine();
                }
                else if (menu == 4)
                {
                    Console.Clear();
                    int alternative = ShowMenu("Vad vill du göra?", new[]
                    {
                        "Skapa ny Cabin",
                        "Skapa ny Counselor",
                        "Skapa ny Camper",
                        "Skapa ny Kin",
                        "Hantera förhållande Cabin/Counselor",
                        "Hantera förhållande Cabin/Campers",
                        "Arkivera aktiv deltagare",
                        "Återgå till huvudmenyn"
                    });
                    if (alternative == 0)
                    {
                        Console.Clear();
                        Cabin.CreateCabin();
                    }
                    else if (alternative == 1)
                    {
                        Console.Clear();
                        Counselor.CreateCounselor();
                    }
                    else if (alternative == 2)
                    {
                        Console.Clear();
                        Camper.CreateCamper();
                    }
                    else if (alternative == 3)
                    {
                        Console.Clear();
                        NextOfKin.CreatKin(s);
                    }
                    else if (alternative == 4)
                    {
                        Console.Clear();
                        Counselor.ConnectCounselorToCabin(s);
                    }
                    else if (alternative == 5)
                    {
                        Console.Clear();
                        Camper.ConnectCamperToCabin(s);
                    }
                    else if (alternative == 6)
                    {
                        Console.Clear();
                        SendToArchive.SendToTheArchive(s);
                    }
                    else if(alternative == 7)
                    {
                        
                    }
                }
                else if (menu == 5)
                {
                    done = true;
                }
            }
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
                else if (key == ConsoleKey.Backspace && selected > 0)
                {
                    break;
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
}
