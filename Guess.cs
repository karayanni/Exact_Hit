using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exact_Hit
{
    public enum Colors
    {
        Green,
        Red,
        LightBlue,
        DarkBlue,
        Orange,
        Yellow,
        Black,
        Brown,
        Purple,
        Pink,
    }

    public class Guess
    {
        public Colors[] ChosenGuess;

        public static readonly int c_NumberOfColors = Enum.GetNames(typeof(Colors)).Length;

        public static readonly int c_GuessLenght = 4;

        public Guess(Colors[] guess)
        {
            if (guess.Length != c_GuessLenght)
            {
                throw new ArgumentException($"a guess must have exactly {c_GuessLenght} colors");
            }

            ChosenGuess = guess;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            foreach (var color in this.ChosenGuess)
            {
                // TODO: use efficient enum to string 
                str.Append($" - {color} num: {(int)color} - ");
            }

            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Guess newGuess = (Guess) obj;

                for (int i = 0; i < c_GuessLenght; i++)
                {
                    if (newGuess.ChosenGuess[i] != ChosenGuess[i])
                        return false;
                }

                return true;
            }
        }
    }
}
