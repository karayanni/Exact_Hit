
namespace Exact_Hit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public class GameSimulator
    {
        public static int total_steps;
        public static ActionBlock<bool> actionBlock = new ActionBlock<bool>(SimulateGameAsync, new ExecutionDataflowBlockOptions(){BoundedCapacity = 1000, MaxDegreeOfParallelism = 100});

        static async Task Main(string[] args)
        {
            int decisionNumber = GetGameMode();

            string message = decisionNumber == 2 ? await AutomatedSimulationsAsync().ConfigureAwait(false) : ManualGame();
           
            Console.WriteLine(message);
            Console.WriteLine("\n simulation finished, press enter to exit...");
            Console.Read();
        }

        /// <summary>
        /// simulates a game where the given logic is trying to crack a random secret
        /// </summary>
        /// <param name="disableRepeatingColors"></param>
        private static void SimulateGameAsync(bool disableRepeatingColors)
        {
            Player player = new Player();

            ComputingLogic logic = new ComputingLogic(disableRepeatingColors: disableRepeatingColors);

            var stepsCount = 1;
            var newGuess = logic.MakeGuess();
            var response = player.TryNewGuess(newGuess);

            while (response.ExactHits != Guess.c_GuessLenght)
            {
                logic.ReceiveResponse(response);
                newGuess = logic.MakeGuess();
                response = player.TryNewGuess(newGuess);
                stepsCount++;
            }

            Interlocked.Add(ref total_steps, stepsCount);
        }

        /// <summary>
        /// enables you to manually simulate the logic trying to crack your secret code
        /// </summary>
        private static string ManualGame()
        {
            ComputingLogic logic = new ComputingLogic(disableRepeatingColors: false);
            int steps = 1;

            while (true)
            {
                var newGuess = logic.MakeGuess();

                Console.WriteLine(newGuess.ToString());

                var response = new Response();

                Console.WriteLine("enter the number of exact hits (B) : ");

                var input = Console.ReadLine();
                int input_int;

                while ((!int.TryParse(input, out input_int)))
                {
                    Console.WriteLine("invalid number, please provide an integer");
                    input = Console.ReadLine();
                }

                response.ExactHits = input_int;

                if (response.ExactHits == Guess.c_GuessLenght)
                {
                    return $"manual simulation finished, cracked the code in {steps} steps";
                }

                Console.WriteLine("enter the number of Almost hits (X) : ");

                input = Console.ReadLine();

                while ((!int.TryParse(input, out input_int)))
                {
                    Console.WriteLine("invalid number, please provide an integer");
                    input = Console.ReadLine();
                }

                response.AlmostHits = input_int;


                if (!logic.ReceiveResponse(response))
                {
                    return "mistake in at least one of the step's Response, no secret possible for the given results";
                }
                
                steps++;
            }
        }

        private static async Task<string> AutomatedSimulationsAsync()
        {
            var numberOfSimulations = GetRequestedNumberOfSimulations();

            var totalRuns = 0;
            for (var i = 0; i < numberOfSimulations; i++)
            {
                if (await actionBlock.SendAsync( false))
                {
                    totalRuns++;
                }
            }

            // polling until action block workers finish - awaiting completion Task isn't supported unless Complete()
            // is called which stops all unfinished processes... polling does the job...
            while (actionBlock.InputCount != 0)
            {
                Thread.Sleep(500);
            }

            actionBlock.Complete();
            await actionBlock.Completion.ConfigureAwait(false);

            double avg = (double)total_steps / totalRuns;

            return $"average steps needed to crack the secret: {avg} , total runs simulated : {totalRuns}";
        }

        private static int GetGameMode()
        {
            Console.WriteLine("welcome to Exact Hit Simulator (aka Bulls and Cows)\n" +
                              "for a manual simulation of the system cracking your code press 1, for automated simulation press 2");
            int decisionNumber;

            var decision = Console.ReadLine();

            while (!int.TryParse(decision, out decisionNumber) || (decisionNumber != 1 && decisionNumber != 2))
            {
                Console.WriteLine("invalid number enter 1 or 2 ...");
                decision = Console.ReadLine();
            }

            return decisionNumber;
        }

        private static int GetRequestedNumberOfSimulations()
        {
            Console.WriteLine("Enter number of simulations requested");

            int numberOfSimulations;
            var numberOfSimulationsInput = Console.ReadLine();

            while ((!int.TryParse(numberOfSimulationsInput, out numberOfSimulations)) || numberOfSimulations < 1 || numberOfSimulations > 10000)
            {
                Console.WriteLine("invalid number, please provide positive integer smaller than 10000");
                numberOfSimulationsInput = Console.ReadLine();
            }

            return numberOfSimulations;
        }
    }
}
