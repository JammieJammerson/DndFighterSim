using System;
using System.Collections.Generic;
using System.IO;
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
            List<Fighter> combatants = new List<Fighter>();
            string dm = "DefaultDM";
            string campaign = "DefaultCampaign";
            int NumberofPlayers = 0;
            Console.WriteLine("Choose based on if you haved use this or not:" +
                "\n1. Make a new campaign" +
                "\n2. Use an older one");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                (List<Fighter>, string, int) value = StartNewCampaign();
            }
            else if (choice == "2")
            {
                campaign = ContinuingCampaign();
            }
            else
            {
                Console.WriteLine("Invalid choice, continuing with default campaign.");
            }

            Console.WriteLine("Are you: \n 1. Wanting to start a fight. \n 2. {Enter Something here later} \n 3. Wanting to Quit Here");
            string Continuance = Console.ReadLine();

            if (Continuance == "1")
            {
                FightProtocol(combatants, NumberofPlayers);
            }
            else if (Continuance == "2")
            {

            }
            else if (Continuance == "3")
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// Prompt user to start a new campaign and return the campaign name.
        /// </summary>
        /// <returns>The campaign name entered by the user.</returns>
        public static (List<Fighter>, string, int) StartNewCampaign()
        {
            Console.WriteLine("Starting a new campaign.");
            Console.WriteLine("DM's Name:");
            string dm = Console.ReadLine();
            Console.WriteLine("Campaign name:");
            string campaign = Console.ReadLine();
            // Collect players and enemies using the Setup helper
            var setupResult = Setup(campaign, dm);
            var combatants = setupResult.combatants;
            int NumberofFighters = combatants.Count;
            return (combatants, campaign, NumberofFighters);
        }

        /// <summary>
        /// Continue an existing campaign by selecting its name.
        /// </summary>
        /// <returns>The chosen campaign name.</returns>
        public static string ContinuingCampaign()
        {
            Console.WriteLine("Enter the Name for your Campaign:");
            string campaign = Console.ReadLine();
            StreamReader reader = new StreamReader($"C:\\Users\\Hammonds\\Downloads\\{campaign}.txt");
            {
                foreach (string line in reader.ReadLine().Split(new char[] { ' ' }))
                {
                    reader.ReadLine();
                }

            }
            return string.IsNullOrWhiteSpace(campaign) ? "DefaultCampaign" : campaign;
        }

        /// <summary>
        /// Collects players and enemies from console input and returns the list plus expected total.
        /// </summary>
        /// <returns>Tuple containing the combatant list and the expected total count.</returns>
        public static (List<Fighter> combatants, int NumberofFighters) Setup(string campaign, string dm)
        {

            // Read number of fighters (players)
            int NumberofPlayers = 0;
            var combatants = new List<Fighter>();
            while (NumberofPlayers < 1)
            {
                Console.WriteLine("Please enter a valid number of Players (at least 1):");
                string fightersInput = Console.ReadLine();
                while (!int.TryParse(fightersInput, out NumberofPlayers) || NumberofPlayers < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the amount of players (at least 1):");
                    fightersInput = Console.ReadLine();
                }
            }

            // Collect player names, initiatives and AC
            for (int i = 0; i < NumberofPlayers; i++)
            {
                int initivemodifier;
                int AC;
                int level;
                string playerClass;
                Console.WriteLine($"Enter the name of fighter {i + 1}:");
                string name = Console.ReadLine();
                Console.WriteLine($"Enter the AC for {name}:");
                string acInput = Console.ReadLine();
                while (!int.TryParse(acInput, out AC))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value for the AC:");
                    acInput = Console.ReadLine();
                }
                Console.WriteLine($"Enter the Class for {name}");
                playerClass = Console.ReadLine();
                Console.WriteLine($"Enter the Level for {name}");
                level =  Console.ReadLine() != null && int.TryParse(Console.ReadLine(), out level) ? level : 1;
                Console.WriteLine($"Enter the initiative modifier for {name}");
                initivemodifier = Console.Read();
                // store fighter (include AC)
                combatants.Add(new Fighter { Name = name, Level = level, IsEnemy = false, AC = AC, Class = playerClass, InitiativeModifier = initivemodifier });
                Console.WriteLine($"Added player: {name} (AC {AC}) (Level {level}) (Class {playerClass})");
            }

            StreamWriter writer = new StreamWriter($"C:\\Users\\Hammonds\\Downloads\\{campaign}.txt");
            {
                writer.WriteLine($"Campaign: {campaign}");
                writer.WriteLine($"DM: {dm}");
                for (int i = 0;i < combatants.Count;i++)
                {
                    writer.WriteLine($"{combatants[i].Name},{combatants[i].Class},{combatants[i].Level},{combatants[i].AC}");
                }
                writer.Close();
            }

            return (combatants, NumberofPlayers);
        }
        public static void FightProtocol(List<Fighter> combatants, int NumberofPlayers)
        {
            Random rand = new Random();

            // Read number of enemies
            int NumberofEnemies = -1;
            while (NumberofEnemies < 0)
            {
                Console.WriteLine("Please enter a valid number of enemies (1 or more):");
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
                Console.WriteLine($"Enter the initiative modifier for {name}:");
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
                combatants.Add(new Fighter { Name = name, InitiativeModifier = initiative, IsEnemy = true, AC = AC });
                Console.WriteLine($"Added enemy: {name} (Init {initiative}) (AC {AC})");
            }

            int totalexpected = NumberofPlayers + NumberofEnemies;

            Console.WriteLine("All players and enemies have been entered successfully.");

            // Build turn order based on initiative (descending). ThenBy(f.IsEnemy)
            // keeps players before enemies when initiative ties occur.
            var turnOrder = combatants.OrderByDescending(f => f.InitiativeModifier).ThenBy(f => f.IsEnemy).ToList();

            Console.WriteLine("Press any key to start the turn-based combat...");
            Console.ReadKey();
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

                    Console.WriteLine($"{actor.Name} ({(actor.IsEnemy ? "Enemy" : "Player")})'s turn. Initiative: {actor.InitiativeModifier}");
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
    }

    internal class Fighter
    {
        public string Name { get; set; }
        public int InitiativeModifier { get; set; }
        public int MaxHP { get; set; } = 10;
        public int AC { get; set; } = 10;
        public string Playerclass { get; set;}
        public bool IsEnemy { get; set; }
        public bool IsAlive { get; set; } = true;
        public int Level { get; set; } = 1;
        public string Class { get; set; }
    }
}
