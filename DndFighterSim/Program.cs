using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

            // Build turn order based on initiative (descending), enemies after players when tie
            var turnOrder = combatants.OrderByDescending(f => f.Initiative).ThenBy(f => f.IsEnemy).ToList();

            Console.WriteLine("Press any key to start the turn-based combat...");
            Console.ReadKey();

            var rand = new Random();
            int round = 1;

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
                    if (attackRoll >= 11)
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
        public void Setup()
        {

        }
    }

    internal class Fighter
    {
        public string Name { get; set; }
        public int Initiative { get; set; }
        public int MaxHP { get; set; } = 10;
        public int AC { get; set; } = 10;
        public bool IsEnemy { get; set; }
        public bool IsAlive { get; set; } = true;
    }
}
