using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exact_Hit
{
    class ComputingLogic
    {
        private HashSet<KeyValuePair<int, Guess>> GuessesHistSet = new HashSet<KeyValuePair<int, Guess>>();

        private Guess _lastGuess;

        //TODO: make dynamic guess length and not hard coded
        public ComputingLogic(bool disableRepeatingColors = false)
        {
            if (disableRepeatingColors)
            {
                //TODO: make it efficient (instead of c_NumberOfColors^codeSize make code size choose c_NumberOfColors)
                for (int i1 = 0; i1 < Guess.c_NumberOfColors; i1++)
                {
                    for (int i2 = 0; i2 < Guess.c_NumberOfColors; i2++)
                    {
                        if (i2 == i1)
                        {
                            // cant repeat same color
                            continue;
                        }

                        for (int i3 = 0; i3 < Guess.c_NumberOfColors; i3++)
                        {
                            if (i3 == i1 || i3 == i2)
                            {
                                // cant repeat same color
                                continue;
                            }

                            for (int i4 = 0; i4 < Guess.c_NumberOfColors; i4++)
                            {
                                if (i4 == i1 || i4 == i2 || i4 == i3)
                                {
                                    // cant repeat same color
                                    continue;
                                }
                                GuessesHistSet.Add(new KeyValuePair<int, Guess>(0, new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 })));
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i1 = 0; i1 < Guess.c_NumberOfColors; i1++)
                {
                    for (int i2 = 0; i2 < Guess.c_NumberOfColors; i2++)
                    {
                        for (int i3 = 0; i3 < Guess.c_NumberOfColors; i3++)
                        {
                            for (int i4 = 0; i4 < Guess.c_NumberOfColors; i4++)
                            {
                                GuessesHistSet.Add(new KeyValuePair<int, Guess>(0, new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 })));
                            }
                        }
                    }
                }
            }
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

            _lastGuess = iterator.Current.Value;
            
            iterator.Dispose();
            
            return _lastGuess;
        }

        public void ReceiveResponse(Response response)
        {
            GuessesHistSet.RemoveWhere(pair => 
                Utils.GetResponse(pair.Value, _lastGuess).Equals(response) == false);
        }
    }
}
