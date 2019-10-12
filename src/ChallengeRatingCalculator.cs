using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace src
{
    struct Challenge
    {
        public string cr;

        public int quantity;

        public override string ToString() => $"{cr} {quantity}";
    }

    public class ChallengeRatingCalculator
    {
        public ChallengeRatingCalculator(Difficulty difficulty, List<int> players, char Rank = 'F')
        {
            if (players is null || players.Count <= 0)
                throw new ArgumentOutOfRangeException("You must have at least 1 player");

            this.Difficulty = difficulty;
            this.Players = players;
            this.AssumedPartyRank = DeterminePartyRank(Rank);
        }

        private readonly int AssumedPartyRank;

        private int DeterminePartyRank(char rank)
        {
            switch (Char.ToUpper(rank))
            {
                case 'F':
                    return 2;
                case 'E':
                    return 3;
                case 'D':
                    return 5;
                case 'C':
                    return 9;
                case 'B':
                    return 14;
                case 'A':
                    return 17;
                case 'S':
                    return 20;
                default:
                    return 2;
            }
        }

        private readonly Difficulty Difficulty;

        private readonly List<int> Players;

        public bool GenerateChallengeRatingOutcomes()
        {
            var rankMaximum = Constants.RankMapping[AssumedPartyRank].RankMaxiumumAdjustedExperience;
            var minimumDifficultyExperience = Players
                .Select(player => Difficulty == Difficulty.Deadly ? Constants.RankMapping[player].Deadly : Constants.RankMapping[player].Hard)
                .Sum();

            GenerateEncounters(minimumDifficultyExperience, rankMaximum);

            return true;
        }

        public void GenerateEncounters(int minimumAdjustedExperience, int maximumAdjustedExperience)
        {
            // Get all possible combinations
            //var x = GetAllCRPermutations();
            var x = UsingWhile(new Challenge { cr = "CR0", quantity = 1 });
            //var x = GetChallenges();

            using (var writer = new StreamWriter("/home/castiel/programming/gm_monter_generator/test.txt"))
            {
                foreach (var perm in x)
                {
                    writer.WriteLine(String.Join(' ', perm.Select(kvp => $"{kvp.Key} {kvp.Value}")));
                }
            }

            // Evaluate the adjusted experience
            // var permutations = x.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

            // var formatted = new List<Tuple<Dictionary<string, int>, decimal>>();

            // foreach (var permutation in permutations)
            // {
            //     var encounter = new Dictionary<string, int>();

            //     for (int i = 0; i < permutation.Length; i++)
            //     {
            //         encounter.Add(permutation[i], int.Parse(permutation[i + 1]));
            //         i++;
            //     }

            //     var adjustedExperience = EvaluateAXP(encounter);
            //     var validEncounter = adjustedExperience >= minimumAdjustedExperience && adjustedExperience <= maximumAdjustedExperience;
            //     if (validEncounter)
            //         formatted.Add(new Tuple<Dictionary<string, int>, decimal>(encounter, adjustedExperience));
            // }

            // // Filter them by the range
            // using (var writer = new StreamWriter("/home/castiel/programming/gm_monter_generator/output.txt"))
            // {
            //     foreach (var p in formatted)
            //     {
            //         var result = string.Join(',', p.Item1.Select(kvp => $"{kvp.Key} x{kvp.Value}").ToArray());
            //         writer.WriteLine(result);
            //     }
            // }
        }

        private HashSet<string> GetAllCRPermutations()
        {
            var permutations = new HashSet<string>();
            var sb = new StringBuilder();

            foreach (var BaseChallengeRating in Constants.ChallengeRatings.Keys)
            {
                var ci = Constants.ChallengeRatings.Keys.ToList().IndexOf(BaseChallengeRating);

                for (int i = 1; i < 13; i++)
                {
                    sb.Append($"{BaseChallengeRating} {i} ");
                    var result = sb.ToString();
                    permutations.Add(result);
                    sb.Clear();

                    foreach (var challengeRating in Constants.ChallengeRatings.Keys)
                    {
                        if (challengeRating == BaseChallengeRating)
                            continue;
                        // sb.Append($"{BaseChallengeRating} {i} ");

                        for (int j = 1; j < 13; j++)
                        {
                            sb.Append($"{BaseChallengeRating} {i}");
                            permutations.Add(sb.ToString());
                            sb.Clear();

                        }
                    }
                    sb.Clear();
                }
            }

            return permutations;
        }

        private List<Dictionary<string, int>> UsingWhile(Challenge currentCR)
        {
            // If we're at CR 1/8, we want to start at CR 1/4
            var startingIndex = Constants.ChallengeRatings.Keys.ToList().IndexOf(currentCR.cr);

            // We're at CR 30
            if (startingIndex == Constants.ChallengeRatings.Keys.Count - 1)
            {
                return null;
            }

            var iterationIndex = startingIndex;

            var iterations = 1;

            // We'll set this ourselves
            var done = false;

            var results = new List<Dictionary<string, int>>();
            var line = new Dictionary<string, int>();

            var challengeRatingsList = Constants.ChallengeRatings.Keys.ToList();

            while (!done)
            {
                if (!line.ContainsKey(currentCR.cr))
                    line.Add(currentCR.cr, currentCR.quantity);

                for (int i = challengeRatingsList.IndexOf(currentCR.cr) + 1; i < 34; i++)
                {
                    var challengeRating = challengeRatingsList[i];

                    if (line.ContainsKey(challengeRating))
                    {
                        line[challengeRating] = i <= iterationIndex ? iterations : iterations - 1;

                        if (line.Values.Sum() == 34 * iterations)
                        {
                            iterations++;
                        }
                        continue;
                    }

                    // Add for the first time
                    line.Add(challengeRating, iterations);
                }

                // Add to the list of permutations
                results.Add(new Dictionary<string, int>(line));

                // increment
                iterationIndex++;

                done = results.Last().Values.All(q => q == 12);
            }

            return results;
        }

        private List<List<Challenge>> GetChallenges()
        {
            var results = new List<List<Challenge>>();

            foreach (var challengeRating in Constants.ChallengeRatings.Keys)
            {
                var outerIndex = Constants.ChallengeRatings.Keys.ToList().IndexOf(challengeRating);

                for (int i = 1; i < 13; i++)
                {
                    var temp = new List<Challenge>() { new Challenge { cr = challengeRating, quantity = i } };
                    results.Add(temp);

                    // Add CR1/8 1 to temp
                    // get last challenge element
                    // set quantity to (local last challenge element)quantity + 1
                    // Add CR1/8 2 to temp
                    // update last 
                    // Add CR1/4 1

                    var temp2 = new List<Challenge>();

                    foreach (var horizontalRaints in Constants.ChallengeRatings.Keys.Skip(outerIndex + 1))
                    {
                        temp2.Add(new Challenge { cr = horizontalRaints, quantity = 1 });

                        for (int j = 1; j < 12; j++)
                        {
                            var last = temp2.Last();
                            last.quantity = j + 1;
                            temp2.Add(last);
                        }
                    }

                    // var temp2 = new List<Challenge>(temp);
                    // for (int j = 1; j < 13; j++)
                    // {
                    //     foreach (var horizontalRatings in Constants.ChallengeRatings.Keys.Skip(outerIndex + 1))
                    //     {

                    //         temp2.Add(new Challenge { cr = horizontalRatings, quantity = j });
                    //     }
                    //     results.Add(temp2);
                    // }

                }
            }

            return results;
        }

        private decimal Multiplier(int numberOfMonsters)
        {
            switch (numberOfMonsters)
            {
                case int i when (numberOfMonsters == 1):
                    return 1m;
                case int i when (numberOfMonsters == 2):
                    return 1.5m;
                case int i when (numberOfMonsters >= 3 && numberOfMonsters <= 6):
                    return 2m;
                case int i when (numberOfMonsters >= 7 && numberOfMonsters <= 10):
                    return 2.5m;
                case int i when (numberOfMonsters >= 11 && numberOfMonsters <= 14):
                    return 3m;
                case int i when (numberOfMonsters >= 15):
                    return 4m;
                default:
                    return 1m;
            }
        }

        public decimal EvaluateAXP(Dictionary<string, int> monsters)
        {
            var total = 0;

            foreach (var monster in monsters)
            {
                total += monster.Value * Constants.ChallengeRatings[monster.Key];
            }

            return total * Multiplier(monsters.Values.Sum());
        }
    }
}