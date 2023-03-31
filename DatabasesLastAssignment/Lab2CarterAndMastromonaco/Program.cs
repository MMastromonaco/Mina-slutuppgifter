using Lab2CarterAndMastromonaco.Function;
using Microsoft.EntityFrameworkCore;
using Lab2CarterAndMastromonaco.Data;
using Lab2CarterAndMastromonaco.Model;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Identity.Client;
using System.Globalization;

namespace Lab2CarterAndMastromonaco
{
    // Paul Carter & Martin Larsson.
    internal class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Martin
            var connectionString = @"Server=localhost, 1433\SQLEXPRESS;Database=CampSleepaway;User Id=SA;Password=XenoYamaha1;Trusted_Connection=True; Integrated Security=false;TrustServerCertificate=true;MultipleActiveResultSets=true";
            var options = new DbContextOptionsBuilder<CamperContext>()
                .UseSqlServer(connectionString)
                .Options;

            //Paul
            //var connectionString = @"Server=LAPTOP-5PA0JF9C\SQLEXPRESS;Database=CampSleepaway;Trusted_Connection=True; Integrated Security=true;TrustServerCertificate=true;";
            //var options = new DbContextOptionsBuilder<CamperContext>()
            //   .UseSqlServer(connectionString)
            //   .Options;

            //Här skapas databasen med hjälp av options ovanför
            using var db = new CamperContext(options);
            db.Database.EnsureCreated();

            // Här har vi en väldigt simpel check om databasen innehåller någon data så kommer den inte fyllas med våran seed-data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT (*) FROM Cabins";
                SqlCommand command = new SqlCommand(query, connection);

                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    //Vi valde att lägga in all seed-data via 2 metoder, här anropas dem (Men endast om databasen är tom).
                    AddCabins.AddCabin();
                    AddHistoryPeople.HistoryOfCamp();
                }
            }

            //Här kallar vi på våran MainMenu method som vi valt att lägga hela menyn och methoden som bygger menyn.
            Menu.MainMenu(connectionString);
        }
    }
}