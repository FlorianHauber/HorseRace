//
// Florian Hauber
// 1IHIF
// 23.6.2026
// description: gambling with horses and 10 has an unfair advantage, because i wanted it to look good :)
//


using System;
using System.IO;
using System.Text;
using System.Threading;

namespace HorseRace
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            Horse[] horses = ReadHorsesFromCSV("horses.csv");
            Horse tippHorse = GetTipp(horses);
            bool raceFinished = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Your bet with starting number {0} calls {1} and is {2} years old. ",
                    tippHorse.GetNumber(), tippHorse.GetName(), tippHorse.GetAge());
                Console.WriteLine();
                raceFinished = MoveHorses(horses);
            } while (!raceFinished);
            GetRaceResults(horses);
            Console.WriteLine("Your bet starting number {0}, name {1}, {2} years old, reached place {3}! ",
                tippHorse.GetNumber(), tippHorse.GetName(), tippHorse.GetAge(), tippHorse.GetRank());

            Console.WriteLine("Press any key to finish ...");
            Console.ReadKey();
        }

        /// <summary>
        /// All lines of the specified CSV file are read.
        /// A Horse array (new Horse[..]) is created with the number
        /// of lines that were read.
        /// From each line, a new Horse is created; the name and
        /// the age are extracted from the text line (Split!)
        /// and assigned to the Horse.
        /// The starting number (Number) of the horse is set to
        /// the loop index increased by 1.
        /// The new horse must be assigned to the Horse array
        /// at the correct position.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Array of horses</returns>
        private static Horse[] ReadHorsesFromCSV(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            Horse[] horses = new Horse[lines.Length];
            string[] parts;

            for (int i = 0; i < lines.Length; i++)
            {
                parts = lines[i].Split(';');

                horses[i] = new Horse(i + 1, parts[0], int.Parse(parts[1]));
            }

            return horses;
        }

        /// <summary>
        /// Output of the starting list to the console:
        /// No - Name - Age
        /// {0,-3} {1,-10} {2}
        /// </summary>
        /// <param name="horses"></param>
        private static void PrintStartList(Horse[] horses)
        {
            Console.WriteLine("No - Name - Age");

            for (int i = 0; i < horses.Length; i++)
            {
                Console.WriteLine($"{horses[i].GetNumber(),-3} {horses[i].GetName(),-10} {horses[i].GetAge()}");
            }
        }

        /// <summary>
        /// First, the starting list is printed.
        /// Then the user must select a starting number.
        /// As long as the number is not between 1 and 10
        /// (length of the array), the input is repeated.
        /// </summary>
        /// <param name="horses"></param>
        /// <returns>The horse with the selected starting number</returns>
        private static Horse GetTipp(Horse[] horses)
        {
            PrintStartList(horses);

            int tippNumber;

            do
            {
                Console.Write("Enter your bet (1-10): ");

                if (!int.TryParse(Console.ReadLine(), out tippNumber) || tippNumber < 1 || tippNumber > horses.Length)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 10.");
                }
            } while (tippNumber < 1 || tippNumber > horses.Length);

            return horses[tippNumber - 1];
        }

        /// <summary>
        /// One "round" of the race – all horses are
        /// randomly moved forward by one position or not.
        /// The probability that a horse advances by one position
        /// should be one third (in 1 out of 3 cases).
        /// Additionally, the horses and their current positions
        /// are printed to the console (line by line, one horse per line).
        /// If one of the horses has reached the finish position 60,
        /// true is returned (=> finished).
        /// At the end of a round, execution pauses for 100 ms.
        /// </summary>
        /// <param name="horses"></param>
        /// <returns>true if at least one horse has reached the finish</returns>
        private static bool MoveHorses(Horse[] horses)
        {
            bool raceFinished = false;

            for (int i = 0; i < horses.Length; i++)
            {
                if (random.Next(3) == 0)
                {
                    horses[i].Move();
                }

                DrawPosition(horses[i], i);

                if (horses[i].GetPosition() >= 60)
                {
                    raceFinished = true;
                }
            }

            Thread.Sleep(100);

            return raceFinished;
        }

        /// <summary>
        /// Output of the horse with starting number as well as
        /// the current position of a horse using
        /// '*' and the finish line ('|') at position 60.
        /// If a horse has already reached position 60,
        /// '*' is printed instead of '|'.
        /// </summary>
        /// <param name="horse"></param>
        private static void DrawPosition(Horse horse, int horseNumber)
        {
            int position = 0;
            string output = "";
            int howManySpaces = 60;

            if (horseNumber == 9)
            {
                howManySpaces = 59;
            }

            position = horse.GetPosition();

            for (int i = 0; i <= howManySpaces; i++)
            {
                if (position == howManySpaces)
                {
                    output += "*";
                }

                else if (i >= howManySpaces)
                {
                    output += "|";
                }

                else if (i == position)
                {
                    output += "*";
                }

                else
                {
                    output += " ";
                }
            }

            Console.WriteLine($"Horse {horseNumber + 1}: {output}");
        }

        /// <summary>
        /// The horse array is sorted in descending order
        /// based on the horses' positions; afterward,
        /// the corresponding ranks are assigned to the horses.
        /// Finally, the determined result is printed to the console.
        /// </summary>
        /// <param name="horses"></param>
        private static void GetRaceResults(Horse[] horses)
        {
            SortByPosition(horses);
            AssignRanks(horses);
            PrintResults(horses);
        }

        /// <summary>
        /// Sort the horse array in descending order
        /// by position.
        /// </summary>
        /// <param name="horses"></param>
        private static void SortByPosition(Horse[] horses)
        {
            for (int i = 0; i < horses.Length - 1; i++)
            {
                for (int j = i + 1; j < horses.Length; j++)
                {
                    if (horses[i].GetPosition() < horses[j].GetPosition())
                    {
                        (horses[i], horses[j]) = (horses[j], horses[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Assign the appropriate rank to the horses in the array,
        /// starting with rank 1 (SetRank).
        /// Horses with the same position receive the same
        /// ranking (ex aequo).
        /// </summary>
        /// <param name="horses"></param>
        private static void AssignRanks(Horse[] horses)
        {
            int rank = 0;

            for (int i = 0; i < horses.Length; i++)
            {
                rank = i;

                while (rank > 0 && horses[rank].GetPosition() == horses[rank - 1].GetPosition())
                {
                    rank--;
                }

                horses[i].SetRank(rank + 1);
            }
        }

        /// <summary>
        /// Output of the race results to the console:
        /// Rank   No    Name    Age   Position
        /// {0,-5} {1,3} {2,-10} {3,-5} {4}
        /// </summary>
        /// <param name="horses"></param>
        private static void PrintResults(Horse[] horses)
        {
            Console.WriteLine("Rank   No    Name    Age   Position");
            for (int i = 0; i < horses.Length; i++)
            {
                Console.WriteLine($"{horses[i].GetRank(),-5} {horses[i].GetNumber(),3} {horses[i].GetName(),-10} {horses[i].GetAge(),-5} {horses[i].GetPosition()}");
            }
        }
    }
}
