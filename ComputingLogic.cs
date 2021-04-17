
namespace Exact_Hit
{
    using System;
    using System.Collections.Generic;

    class ComputingLogic
    {
        private HashSet<Guess> GuessesHistSet;

        private Guess _lastGuess;

        public ComputingLogic(bool disableRepeatingColors = false)
        {
            GuessesHistSet = disableRepeatingColors ? GuessesHistFactory.GetGuessesHistSetNoRepeatingColors() : GuessesHistFactory.GetGuessesHistSetRepeatingColorsAllowed();
        }

        public Guess MakeGuess()
        {
            Random r = new Random();
            
            var i = r.Next(0, GuessesHistSet.Count-1);

            var iterator = GuessesHistSet.GetEnumerator();
            iterator.MoveNext();

            for (int j = 0; j < i; j++)
            {
                iterator.MoveNext();
            }

            _lastGuess = iterator.Current;
            
            iterator.Dispose();
            
            return _lastGuess;
        }

        public bool ReceiveResponse(Response response)
        {
            GuessesHistSet.RemoveWhere(guess=> 
                Utils.GetResponse(guess, _lastGuess).Equals(response) == false);
            return GuessesHistSet.Count >= 1;
        }
    }
}
