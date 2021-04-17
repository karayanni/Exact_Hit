
namespace Exact_Hit
{
    using System;
    using System.Text;

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

    [Serializable]
    public class Guess
    {
        public readonly Colors[] ChosenGuess;

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

                if (newGuess.ChosenGuess.Length != ChosenGuess.Length)
                {
                    return false;
                }

                for (int i = 0; i < c_GuessLenght; i++)
                {
                    if (newGuess.ChosenGuess[i] != ChosenGuess[i])
                        return false;
                }

                return true;
            }
        }

        public override int GetHashCode()
        {
            var str = new StringBuilder($"{c_GuessLenght}{c_NumberOfColors}");
            foreach (var color in ChosenGuess)
            {
                str.Append($"{(int)color}");
            }
            return Convert.ToInt32(str.ToString());
        }
    }
}
