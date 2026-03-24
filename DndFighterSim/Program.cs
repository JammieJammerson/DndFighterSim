using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DndFighterSim
{
    internal class Program
    {
        /// <summary>
        /// Entry point for the DnD fighter simulator console app.
        /// Reads campaign selection, collects players and enemies, then runs a simple
        /// initiative-based turn loop until one side is eliminated.
        /// </summary>
        /// <param name="args">Command-line arguments (unused).</param>
        static void Main(string[] args)
        {
            // Campaign selection
            string campaign = "DefaultCampaign";
            Console.WriteLine("Choose based on if you haved use this or not:" +
                "\n1. Make a new campaign" +
                "\n2. Use an older one");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                ContinuingCampaign(campaign);
            }
            else if (choice == "2")
            {
                Console.WriteLine("Feature not implemented yet. Starting new campaign...");
                StartNewCampaign(campaign);
            }
            else
            {
                Console.WriteLine("Try again.");
            }
            
            using (StreamWriter sw = new StreamWriter("{campaign}.txt", true))
            {
                sw.WriteLine("Hello, {fullname}");
                sw.WriteLine("This is the party for {campign}.");
                for (int i = 0; i < NumberofNumberofFighters; i++)
                {
                    sw.WriteLine("");
                }
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

            // Collect enemy names and initiatives
            for (int i = 0; i < NumberofEnemies; i++)
            {
                Console.WriteLine($"Enter the name of enemy {i + 1}:");
                string name = Console.ReadLine();
                Console.WriteLine($"Enter the initiative for {name}:");
                string initiativeInput = Console.ReadLine();
                int initiative;

                Console.WriteLine($"Enter the AC for {name}:");
                string acInput = Console.ReadLine();
                int AC;
                while (!int.TryParse(acInput, out AC))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the AC:");
                    acInput = Console.ReadLine();
                }

                while (!int.TryParse(initiativeInput, out initiative))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the initiative:");
                    initiativeInput = Console.ReadLine();
                }
                // store enemy
                combatants.Add(new Fighter { Name = name, Initiative = initiative, IsEnemy = true });
                Console.WriteLine($"Added enemy: {name} (Init {initiative})");
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

            // Build turn order based on initiative (descending). ThenBy(f.IsEnemy)
            // keeps players before enemies when initiative ties occur.
            var turnOrder = combatants.OrderByDescending(f => f.Initiative).ThenBy(f => f.IsEnemy).ToList();

            Console.WriteLine("Press any key to start the turn-based combat...");
            Console.ReadKey();

            var rand = new Random();
            int round = 1;

            // Run simple turn-based loop until one side has no living combatants
            while (combatants.Any(c => c.IsAlive && !c.IsEnemy) && combatants.Any(c => c.IsAlive && c.IsEnemy))
            {
                Console.WriteLine($"\n-- Round {round} --");

                foreach (var actor in turnOrder.Where(a => a.IsAlive))
                {
                    // stop if one side has no survivors
                    if (!(combatants.Any(c => c.IsAlive && !c.IsEnemy) && combatants.Any(c => c.IsAlive && c.IsEnemy)))
                        break;

                    Console.WriteLine($"{actor.Name} ({(actor.IsEnemy ? "Enemy" : "Player")})'s turn. Initiative: {actor.Initiative}");
                    Console.WriteLine("Press any key to perform action...");
                    Console.ReadKey();

                    var opponents = combatants.Where(c => c.IsAlive && c.IsEnemy != actor.IsEnemy).ToList();
                    if (!opponents.Any())
                        break;

                    var target = opponents[rand.Next(opponents.Count)];
                    int attackRoll = rand.Next(1, 21);
                    Console.WriteLine($"{actor.Name} attacks {target.Name} (roll: {attackRoll})");
                    // Simple hit resolution: hit if roll >= target AC
                    // (replace with attack bonus + d20 vs AC for more realism)
                    if (attackRoll >= target.AC)
                    {
                        target.IsAlive = false;
                        Console.WriteLine($"{target.Name} is defeated!");
                    }
                    else
                    {
                        Console.WriteLine($"{actor.Name} missed.");
                    }
                }

                round++;
            }

            // Outcome
            if (combatants.Any(c => c.IsAlive && !c.IsEnemy))
            {
                Console.WriteLine("Players win!");
            }
            else
            {
                Console.WriteLine("Enemies win!");
            }
        }

        public static void StartNewCampaign(string campaign)
        {
            // Simple prompt helper to gather campaign info (placeholder)
            Console.WriteLine("Enter your Name and the Campaign:");
            Console.ReadKey();
            Console.WriteLine("Full Name:");
            string fullname = Console.ReadLine();
            Console.WriteLine("Campaign:");
            string campaign = Console.ReadLine();
            return fullname;
            return campaign;
        }
        public static void ContinuingCampaign(string campaign)
        {
            //Simple selection for campaign
            Console.WriteLine("Enter the Name for your Campaign:")
            Console.Readline();
            
        }
        public void Setup()
        {
            // Prompt user for combatants
            Console.WriteLine("Give Me The Characters and Inititives:");

            // Read total expected (players + enemies)
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
            List<Fighter> combatants = new List<Fighter>();
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

            // Collect player names, initiatives and AC
            for (int i = 0; i < NumberofFighters; i++)
            {
                Console.WriteLine($"Enter the name of fighter {i + 1}:");
                string name = Console.ReadLine();
                Console.WriteLine($"Enter the initiative for {name}:");
                string initiativeInput = Console.ReadLine();
                int initiative;
                Console.Writeline($"Enter the race for {name}:")
                string Dndrace = Console.Readline(); 
                while (!int.TryParse(initiativeInput, out initiative))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the initiative:");
                    initiativeInput = Console.ReadLine();
                }

                Console.WriteLine($"Enter the AC for {name}:");
                string acInput = Console.ReadLine();
                int AC;
                while (!int.TryParse(acInput, out AC))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the AC:");
                    acInput = Console.ReadLine();
                }

                // store fighter (include AC)
                combatants.Add(new Fighter { Name = name, Initiative = initiative, IsEnemy = false, AC = AC });
                Console.WriteLine($"Added player: {name} (Init {initiative}) (AC {AC})");
            }
        }
    }

    internal class Fighter
    {
        public string Name { get; set; }
        public int Initiative { get; set; }
        public int MaxHP { get; set; } = 10;
        public int AC { get; set; } = 10;
        public string Playerclass { get; set;}
        public bool IsEnemy { get; set; }
        public bool IsAlive { get; set; } = true;
    }
}
