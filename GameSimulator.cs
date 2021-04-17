
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Exact_Hit
{
    using System;

    public class GameSimulator
    {
        public static int total_steps = 0;
        public static ActionBlock<bool> actionBlock = new ActionBlock<bool>(SimulateGameAsync);

        static async Task Main(string[] args)
        {
            ////Console.WriteLine("welcome to Exact Hit Simulator (aka Bulls and Cows)\n" +
            ////                  "for a manual simulation of the system cracking your code press 1, for automated simulation press 2");
            ////int decision = Console.Read();

            ////Console.WriteLine(decision == 1 ? ManualGame() : AutomatedSimulations());

            var x = await AutomatedSimulationsAsync().ConfigureAwait(false);
            Console.WriteLine(x);
            Console.WriteLine(total_steps);
            Console.ReadLine();
            Console.ReadLine();
        }

        /// <summary>
        /// simulates a game where the given logic is trying to crack a random secret
        /// </summary>
        /// <param name="disableRepeatingColors"></param>
        private static void SimulateGameAsync(bool disableRepeatingColors)
        {
            Console.WriteLine("pass..");
            //Player player = new Player();

            //ComputingLogic logic = new ComputingLogic(disableRepeatingColors: disableRepeatingColors);

            var stepsCount = 1;
            //var newGuess = logic.MakeGuess();
            //var response = player.TryNewGuess(newGuess);

            //while (response.ExactHits != Guess.c_GuessLenght)
            //{
            //    logic.ReceiveResponse(response);
            //    newGuess = logic.MakeGuess();
            //    response = player.TryNewGuess(newGuess);
            //    stepsCount++;
            //}

            Interlocked.Add(ref total_steps, stepsCount);
            Console.WriteLine("finished worker function");
            return;
        }

        /// <summary>
        /// enables you to manually simulate the logic trying to crack your secret code
        /// </summary>
        private static string ManualGame()
        {
            ComputingLogic logic = new ComputingLogic(disableRepeatingColors: true);
            int steps = 1;

            while (true)
            {
                var newGuess = logic.MakeGuess();

                Console.WriteLine(newGuess.ToString());

                var response = new Response();

                Console.WriteLine("enter the number of exact hits (B) : ");
                response.ExactHits = Console.Read();

                if (response.ExactHits == Guess.c_GuessLenght)
                {
                    return $"manual simulation finished, cracked the code in {steps} steps";
                }

                Console.WriteLine("enter the number of Almost hits (X) : ");
                response.AlmostHits = Console.Read();


                if (!logic.ReceiveResponse(response))
                {
                    return "mistake in at least one of the step, no secret possible for the given results";
                }
                steps++;
            }
        }

        private static async Task<string > AutomatedSimulationsAsync()
        {
            int totalSteps = 0, max = 0, min = 1000000;
            bool disableRepeatingColors = false;

            int num_of_steps = 1;
            int total_runs = 0;
            for (int i = 0; i < num_of_steps; i++)
            {
                if (actionBlock.Post(disableRepeatingColors))
                {
                    total_runs++;
                }
            }
            Thread.Sleep(9000);


            Console.WriteLine(actionBlock.InputCount);
            await actionBlock.Completion.ConfigureAwait(false);

            double avg = (double)totalSteps / num_of_steps;

            return $"average steps needed to crack the combination was {avg}, max :{max}, min: {min}, total runs: {total_runs}";
        }
    }
}
