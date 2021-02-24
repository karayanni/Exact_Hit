
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
        public static ActionBlock<int> actionBlock = new ActionBlock<int>(SimulateGame);

        static void Main(string[] args)
        {
            //int totalSteps = 0, max = 0, min = 1000000;

            //int num_of_steps = 50;
            //int total_runs = 0;
            //for (int i = 0; i < num_of_steps; i++)
            //{
            //    if (actionBlock.Post(-999999))
            //    {
            //        total_runs++;
            //    }
            //}

            //actionBlock.Completion.Wait();

            //double avg = (double)totalSteps / num_of_steps;

            //Console.WriteLine($"average steps needed to crack the combination was {avg}, max :{max}, min: {min}, total runs: {total_runs}");
            //Console.ReadLine();

            ManualGame();
        }

        private static void SimulateGame(int x)
        {
            Player player = new Player();

            ComputingLogic logic = new ComputingLogic();

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

        private static void ManualGame()
        {
            ComputingLogic logic = new ComputingLogic(disableRepeatingColors: true);

            while (true)
            {
                var newGuess = logic.MakeGuess();

                Console.WriteLine(newGuess.ToString());

                var response = new Response();

                Console.WriteLine("enter the number of exact hits (B) : ");
                response.ExactHits = Convert.ToInt32(Console.ReadLine());


                Console.WriteLine("enter the number of Almost hits (X) : ");
                response.AlmostHits = Convert.ToInt32(Console.ReadLine());

                logic.ReceiveResponse(response);
            }

        }
    }
}
