
namespace Exact_Hit
{
    using System.Collections.Generic;

    public static class GuessesHistFactory
    {
        public static readonly HashSet<Guess> GuessesHistSetRepeatingColorsAllowed;
        public static readonly HashSet<Guess> GuessesHistSetNoRepeatingColors;

        //TODO: make dynamic guess length and not hard coded - changing number of colors/secret size will cause errors
        static GuessesHistFactory()
        {
            GuessesHistSetRepeatingColorsAllowed = new HashSet<Guess>();
            GuessesHistSetNoRepeatingColors = new HashSet<Guess>();

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
                            GuessesHistSetNoRepeatingColors.Add(new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 }));
                        }
                    }
                }
            }

            for (int i1 = 0; i1 < Guess.c_NumberOfColors; i1++)
            {
                for (int i2 = 0; i2 < Guess.c_NumberOfColors; i2++)
                {
                    for (int i3 = 0; i3 < Guess.c_NumberOfColors; i3++)
                    {
                        for (int i4 = 0; i4 < Guess.c_NumberOfColors; i4++)
                        {
                            GuessesHistSetRepeatingColorsAllowed.Add(new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 }));
                        }
                    }
                }
            }
        }

        public static HashSet<Guess> GetGuessesHistSetRepeatingColorsAllowed()
        {
            return new HashSet<Guess>(GuessesHistSetRepeatingColorsAllowed);
        }

        public static HashSet<Guess> GetGuessesHistSetNoRepeatingColors()
        {
            return new HashSet<Guess>(GuessesHistSetNoRepeatingColors);
        }
    }
}
