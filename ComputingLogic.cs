using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Exact_Hit
{
    class ComputingLogic
    {
        private HashSet<Guess> GuessesHistSet = new HashSet<Guess>();

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
                                GuessesHistSet.Add(new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 }));
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
                                GuessesHistSet.Add(new Guess(new Colors[4] { (Colors)i1, (Colors)i2, (Colors)i3, (Colors)i4 }));
                            }
                        }
                    }
                }
            }


            ////using (var file = File.Create("C:\\Users\\nkarayanni\\OneDrive - Microsoft\\nader's\\Exact_Hit\\serialized_objects"))
            ////{
            ////    FileStream fs = new FileStream("DataFile.dat", FileMode.Create);
            ////    BinaryFormatter formatter = new BinaryFormatter();

            ////    try
            ////    {
            ////        formatter.Serialize(fs, GuessesHistSet);
            ////        fs.Seek(0, SeekOrigin.Begin);
            ////        fs.CopyTo(file);
            ////    }
            ////    catch (SerializationException e)
            ////    {
            ////        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            ////        throw;
            ////    }
            ////    finally
            ////    {
            ////        fs.Close();
            ////    }
            ////}
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
