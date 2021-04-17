
namespace Exact_Hit
{
    using System;
    using System.Collections.Generic;

    class Player
    {
        private Guess PlayersGuess;

        public Player(bool disableRepeatingColorsInGuess = false)
        {
            Random r = new Random();

            Guess randomGuess= new Guess(new Colors[Guess.c_GuessLenght]);

            if (!disableRepeatingColorsInGuess)
            {
                for (int i = 0; i < Guess.c_GuessLenght; i++)
                {
                    randomGuess.ChosenGuess[i] = (Colors)r.Next(0, Guess.c_NumberOfColors);
                }
            }
            else
            {
                HashSet<int> usedNums = new HashSet<int>();

                for (int i = 0; i < Guess.c_GuessLenght; i++)
                {
                    var currRandom = r.Next(0, Guess.c_NumberOfColors);

                    while (usedNums.Contains(currRandom))
                    {
                        currRandom = r.Next(0, Guess.c_NumberOfColors);
                    }

                    usedNums.Add(currRandom);

                    randomGuess.ChosenGuess[i] = (Colors)currRandom;
                }
            }

            PlayersGuess = randomGuess;
        }

        public Response TryNewGuess(Guess new_guess)
        {
            return Utils.GetResponse(this.PlayersGuess, new_guess);
        }
    }
}
