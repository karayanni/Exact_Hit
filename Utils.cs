
namespace Exact_Hit
{
    static class Utils
    {
        public static Response GetResponse(Guess originalGuess, Guess tryingGuess)
        {
            //// original   1 - 1 - 2 - 3
            //// trying     0 - 1 - 3 - 4

            int almost = 0, exact = 0;

            // contains true if the original guess contains the corresponding color
            int[] colors_hist = new int[Guess.c_NumberOfColors];
            bool[] exact_hits = new bool[Guess.c_NumberOfColors];

            foreach (var color in originalGuess.ChosenGuess)
            {
                colors_hist[(int)color]++;
            }

            for (int i = 0; i < originalGuess.ChosenGuess.Length; i++)
            {
                if (originalGuess.ChosenGuess[i] == tryingGuess.ChosenGuess[i])
                {
                    exact++;

                    // since we received an exact hit on this particular tile we don't want it to affect the almost hits
                    colors_hist[(int)originalGuess.ChosenGuess[i]]--;
                    exact_hits[i] = true;
                }
            }

            // here colors_hist[color_i] isn't 0 if color_i is in the original guess and isn't exactly hit

            for (int i = 0; i < tryingGuess.ChosenGuess.Length; i++)
            {
                if (exact_hits[i]) continue;

                // the current index wasn't exactly matched thus it can contribute to the almost counter
                if (colors_hist[(int)tryingGuess.ChosenGuess[i]] <= 0) continue;

                almost++;
                colors_hist[(int)tryingGuess.ChosenGuess[i]]--;
            }

            return new Response
            {
                AlmostHits = almost,
                ExactHits = exact
            };
        }
    }
}
