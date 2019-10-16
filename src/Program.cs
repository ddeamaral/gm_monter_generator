using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using src;

namespace gm_monster
{
    class Program
    {
        static void Main(string[] args)
        {

            var encounterGenerationRequest = new EncounterGenerationRequest();

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
                    encounterGenerationRequest.Players.Add(playerLevel);
                    i++;
                }
                else if (args[i].ToLower() == "-d")
                {
                    encounterGenerationRequest.Difficulty = args[i + 1].ToLower() == "hard" ? Difficulty.Hard : Difficulty.Deadly;
                    i++;
                }
                else if (args[i].ToLower() == "-r")
                {
                    encounterGenerationRequest.Rank = args[i + 1][0];
                    i++;
                }
                else if (args[i].ToLower() == "-min")
                {
                    if (int.TryParse(args[i + 1], out var minimum))
                    {
                        encounterGenerationRequest.MinimumAdjustedExperience = minimum;
                        i++;
                        continue;
                    }

                    using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                    {
                        writer.WriteLine($"You must provide a valid minimum adjusted experience value");
                    }
                }
                else if (args[i].ToLower() == "-max")
                {
                    if (int.TryParse(args[i + 1], out var maximum))
                    {
                        encounterGenerationRequest.MaximumAdjustedExperience = maximum;
                        i++;
                        continue;
                    }

                    using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                    {
                        writer.WriteLine($"You must provide a valid maximum adjusted experience value");
                    }
                }
                else
                {
                    using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                    {
                        writer.WriteLine($"Unrecognized flag {args[i]}");
                        return;
                    }
                }
            }

            if (encounterGenerationRequest.Players.Count == 0)
            {
                using (TextWriter writer = new StreamWriter(Console.OpenStandardError()))
                {
                    writer.WriteLine($"You must have at least one player. Use the -p flag, followed by a number, like so...");
                    writer.WriteLine("whatever.exe -p 3 -p 4 -p 4 -p 3");
                }
                return;
            }

            Console.WriteLine("Congrats, you managed not to screw everything up so far...");
            Console.WriteLine("Here's what you entered, double check it: ");
            Console.WriteLine($"Difficulty: {(encounterGenerationRequest.Difficulty == Difficulty.Hard ? "Hard" : "Deadly")}");
            Console.WriteLine($"Rank: {encounterGenerationRequest.Rank}");
            Console.WriteLine($"Number of players: {encounterGenerationRequest.Players.Count} ({string.Join(' ', encounterGenerationRequest.Players.Select(p => $"Lv{p}"))})");
            Console.WriteLine("Press any key to continue...or press N to cancel");
            var moveForward = false;
#if RELEASE
            var response = Console.Read();
            moveForward = Convert.ToChar(response) != 'N' || Convert.ToChar(response) != 'n';
#endif



#if DEBUG
            moveForward = true;
#endif

            try
            {
                if (moveForward)
                {
                    var challengeRatingCalculator = new ChallengeRatingCalculator(encounterGenerationRequest);
                    var result = challengeRatingCalculator.GenerateChallengeRatingOutcomes();

                    if (result)
                    {
                        Console.WriteLine("Alrighty, copy your macro in macro.txt, and import it into roll20.");
                    }
                    else
                    {
                        Console.WriteLine("Wow, you managed to break it somehow, send Dylan the exact command you ran.");
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Something went wrong, send the file error.txt to Dylan");
                using (var writer = new StreamWriter(System.IO.File.OpenWrite(Constants.OutputPath("error.txt"))))
                {
                    writer.WriteLine($"Message: {e.Message}");
                    writer.WriteLine($"Stack Trace: {e.StackTrace}");
                }
                Console.ReadKey();
            }

        }
    }
}