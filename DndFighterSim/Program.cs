using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndFighterSim
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your Name and the Campaign:");
            Console.ReadKey();      
            Console.WriteLine("Full Name:");
            Console.ReadLine();
            Console.WriteLine("Campaign:");
            Console.ReadLine();
            Console.WriteLine("Give Me The Characters and Inititives:");

            // Read total expected
            Console.WriteLine("Give the amount of players and the enemies combined:");
            string totalInput = Console.ReadLine();
            int totalExpected;
            while (!int.TryParse(totalInput, out totalExpected) || totalExpected < 1)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer for the amount of players and enemies combined:");
                totalInput = Console.ReadLine();
            }

            // Read number of fighters (players)
            int NumberofFighters = 0;
            while (NumberofFighters < 1)
            {
                Console.WriteLine("Please enter a valid number of fighters (at least 1):");
                string fightersInput = Console.ReadLine();
                while (!int.TryParse(fightersInput, out NumberofFighters) || NumberofFighters < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the amount of players (at least 1):");
                    fightersInput = Console.ReadLine();
                }
            }

            for (int i = 0; i < NumberofFighters; i++)
            {
                Console.WriteLine($"Enter the name of fighter {i + 1}:");
                string name = Console.ReadLine();
                Console.WriteLine($"Enter the initiative for {name}:");
                string initiativeInput = Console.ReadLine();
                int initiative;
                while (!int.TryParse(initiativeInput, out initiative))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the initiative:");
                    initiativeInput = Console.ReadLine();
                }
                PrintFighterInfo(name, initiative);
            }

            // Read number of enemies
            int NumberofEnemies = -1;
            while (NumberofEnemies < 0)
            {
                Console.WriteLine("Please enter a valid number of enemies (0 or more):");
                string enemiesInput = Console.ReadLine();
                while (!int.TryParse(enemiesInput, out NumberofEnemies) || NumberofEnemies < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the number of enemies (0 or more):");
                    enemiesInput = Console.ReadLine();
                }
            }

            for (int i = 0; i < NumberofEnemies; i++)
            {
                Console.WriteLine($"Enter the name of enemy {i + 1}:");
                string name = Console.ReadLine();
                Console.WriteLine($"Enter the initiative for {name}:");
                string initiativeInput = Console.ReadLine();
                int initiative;
                while (!int.TryParse(initiativeInput, out initiative))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the initiative:");
                    initiativeInput = Console.ReadLine();
                }
                PrintFighterInfo(name, initiative);
            }

            int totalEntered = NumberofFighters + NumberofEnemies;

            if (totalEntered == totalExpected)
            {
                Console.WriteLine("All fighters and enemies have been entered successfully.");
            }
            else
            {
                Console.WriteLine("The number of enemies and fighters do not add up correctly.");
                Console.WriteLine($"Expected: {totalExpected}, Entered: {totalEntered}");
            }

            Console.ReadKey();
        }
        static void PrintFighterInfo(string name, int initiative)
        {
            Console.WriteLine($"Fighter: {name}, Initiative: {initiative}");
        }
    }
}
