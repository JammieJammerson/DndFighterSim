using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndFighterSim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your Full Name and the Campaign:");
            Console.ReadKey();      
            Console.WriteLine("Full Name:");
            Console.ReadLine();
            Console.WriteLine("Press any Key to Continue:");
            Console.ReadKey();
            Console.WriteLine("Campaign:");
            Console.ReadLine();
            Console.ReadKey();
            Console.WriteLine("Give Me The Characters and Inititives:");
            Console.ReadLine();
            Console.ReadKey();
            Console.WriteLine("Give the amount of players and the enemies combined:");
            string fightersInput = Console.ReadLine();
            double NumberofFighters;
            while (!double.TryParse(fightersInput, out NumberofFighters))
            {
                Console.WriteLine("Invalid input. Please enter a numeric value for the amount of players and enemies combined:");
                fightersInput = Console.ReadLine();
            }
            Console.WriteLine($"Number of fighters: {NumberofFighters}");
            Console.ReadKey();
            Console.WriteLine("");
            NumberofFighters = 0;
             while (NumberofFighters < 1)
            {
                Console.WriteLine("Please enter a valid number of fighters (at least 1):");
                fightersInput = Console.ReadLine();
                while (!double.TryParse(fightersInput, out NumberofFighters) || NumberofFighters < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the amount of players and enemies combined (at least 1):");
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
         
        }
        static void PrintFighterInfo(string name, int initiative)
        {
            Console.WriteLine($"Fighter: {name}, Initiative: {initiative}");
        }
    }
}
