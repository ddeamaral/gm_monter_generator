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
            var permutations = GetAllCRPermutations();

            using (var writer = new StreamWriter("/home/castiel/programming/gm_monter_generator/test.txt"))
            {
                foreach (var permutation in permutations)
                {
                    writer.WriteLine(permutation);
                }
            }


            // Evaluate the adjusted experience

            // Filter them by the range

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

                    sb.Append($"{BaseChallengeRating} {i} ");

                    for (int x = 0; x < Constants.ChallengeRatings.Keys.ToArray().Length; x++)
                    {

                    }

                    foreach (var challengeRating in Constants.ChallengeRatings.Keys.ToList().Skip(ci + 1))
                    {
                        sb.Append($"{challengeRating} {i} ");
                        permutations.Add(sb.ToString());
                    }
                    sb.Clear();
                }
            }

            return permutations;
        }

        public static List<List<string>> permutations(List<string> es)
        {

            List<List<string>> permutations = new List<List<string>>();

            if (es is null || !es.Any())
            {
                return permutations;
            }

            // We add the first element
            permutations.Add(new List<string>(new List<string>() { es.First() }));

            // Then, for all elements e in es (except from the first)
            for (int i = 1, len = es.Count; i < len; i++)
            {
                string e = es.ElementAt(i);

                // We take remove each list l from 'permutations'
                for (int j = permutations.Count - 1; j >= 0; j--)
                {
                    List<string> l = permutations.ElementAt(j);
                    permutations.RemoveAt(j);
                    //.remove(j);

                    // And adds a copy of l, with e inserted at index k for each position k in l
                    for (int k = l.Count; k >= 0; k--)
                    {
                        List<string> ts2 = new List<string>();
                        ts2.Insert(k, e);
                        permutations.Add(ts2);
                    }
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