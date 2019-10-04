using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace src
{
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
            var x = GetAllCRPermutations();

            using (var writer = new StreamWriter("/home/castiel/programming/gm_monter_generator/test.txt"))
            {
                foreach (var permutation in x)
                {
                    writer.WriteLine(permutation);
                }
            }

            // Evaluate the adjusted experience
            var permutations = x.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

            var formatted = new List<Tuple<Dictionary<string, int>, decimal>>();

            foreach (var permutation in permutations)
            {
                var encounter = new Dictionary<string, int>();

                for (int i = 0; i < permutation.Length; i++)
                {
                    encounter.Add(permutation[i], int.Parse(permutation[i + 1]));
                    i++;
                }

                var adjustedExperience = EvaluateAXP(encounter);
                var validEncounter = adjustedExperience >= minimumAdjustedExperience && adjustedExperience <= maximumAdjustedExperience;
                if (validEncounter)
                    formatted.Add(new Tuple<Dictionary<string, int>, decimal>(encounter, adjustedExperience));
            }

            // Filter them by the range
            using (var writer = new StreamWriter("/home/castiel/programming/gm_monter_generator/output.txt"))
            {
                foreach (var p in formatted)
                {
                    var result = string.Join(',', p.Item1.Select(kvp => $"{kvp.Key} x{kvp.Value}").ToArray());
                    writer.WriteLine(result);
                }
            }
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
                            sb.Append($"{BaseChallengeRating} {i} {challengeRating} {j} ");
                            permutations.Add(sb.ToString());
                            sb.Clear();
                        }
                    }
                    sb.Clear();
                }
            }

            return permutations;
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