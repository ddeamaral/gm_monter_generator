using System;
using System.Collections.Generic;
using System.IO;
using src;

namespace gm_monster
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<int>();
            Difficulty difficulty = Difficulty.Hard;
            char rank = 'F';

            for (int i = 0; i < args.Length; i++)
            {
                int playerLevel;

                if (args[i].ToLower() == "-p" && int.TryParse(args[i + 1], out playerLevel))
                {
                    if (playerLevel < 1 || playerLevel > 20)
                    {
                        using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                        {
                            writer.WriteLine($"Unrecognized flag {args[i]}");
                        }

                        return;
                    }
                    players.Add(playerLevel);
                    i++;
                }
                else if (args[i].ToLower() == "-d")
                {
                    difficulty = args[i + 1].ToLower() == "hard" ? Difficulty.Hard : Difficulty.Deadly;
                    i++;
                }
                else if (args[i].ToLower() == "-r")
                {
                    rank = args[i + 1][0];
                    i++;
                }
                else
                {
                    using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                    {
                        writer.WriteLine($"Unrecognized flag {args[i]}");
                    }
                }
            }

            if (players.Count == 0)
            {
                using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                {
                    writer.WriteLine($"You must have at least one player. Use the -p flag, followed by a number");
                }
                return;
            }

            var challengeRatingCalculator = new ChallengeRatingCalculator(difficulty, players, rank);
            var result = challengeRatingCalculator.GenerateChallengeRatingOutcomes();
        }
    }
}