using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var x = UsingWhile(new Challenge { cr = "CR0", quantity = 1 });


            using (var writer = new StreamWriter(@"D:\Development\VSCode\gm_monter_generator\test.txt"))
            {
                foreach (var perm in x)
                {
                    writer.WriteLine(String.Join(' ', perm));
                }
            }
        }

        private HashSet<string> UsingWhile(Challenge currentCR)
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

            var results = new HashSet<Dictionary<string, int>>();
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
                        results.Add(new Dictionary<string, int>(line));

                        if (line.Values.Sum() == 33 * iterations + currentCR.quantity)
                        {
                            iterations++;
                        }

                        continue;
                    }

                    // Add for the first time
                    line.Add(challengeRating, iterations - 1);
                }

                // Add to the list of permutations
                results.Add(new Dictionary<string, int>(line));

                // increment
                iterationIndex++;

                done = results.Last().All(q => q.Value == 12 || q.Key == currentCR.cr);
            }

            return new HashSet<string>(results.Select(dict => Stringified(dict)));
        }

        private string Stringified(Dictionary<string, int> value) => string.Join(' ', value.Select(kvp => $"{kvp.Key} {kvp.Value}"));

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