using System;
using System.Collections.Generic;
using System.Linq;

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
            var rankMaximum = RankMapping[AssumedPartyRank].RankMaxiumumAdjustedExperience;
            var minimumDifficultyExperience = Players
                    .Select(player => Difficulty == Difficulty.Deadly ? RankMapping[player].Deadly : RankMapping[player].Hard)
                    .Sum();

            GenerateEncounters(minimumDifficultyExperience, rankMaximum);

            return true;
        }

        public void GenerateEncounters(int minimumAdjustedExperience, int maximumAdjustedExperience)
        {
            // Figure out boundaries for one cr level
            var sameCREncounters = GetAllSameCRLevels(minimumAdjustedExperience, maximumAdjustedExperience, "CR0");

            // Figure out combinations of all mixed cr levels

            // Merge them

        }

        public Dictionary<string, int> GetAllSameCRLevels(int minimumAdjustedExperience, int maximumAdjustedExperience, string challengeRating)
        {
            var exceeded = false;
            var monsterCount = 0;
            var monsters = new Dictionary<string, int>();

            while (!exceeded)
            {
                monsterCount++;

                var adjustedExperience = EvaluateAXP(new Dictionary<string, int>() { { challengeRating, monsterCount } });
                Console.WriteLine($"{challengeRating} x{monsterCount} = {adjustedExperience}xp");

                if (adjustedExperience >= minimumAdjustedExperience && adjustedExperience <= maximumAdjustedExperience)
                {
                    Console.WriteLine("Added");
                    monsters.Add($"{challengeRating} x{monsterCount}", monsterCount);
                }

                exceeded = adjustedExperience > maximumAdjustedExperience;
            }

            return monsters;
        }

        private readonly Dictionary<string, int> challengeRatings = new Dictionary<string, int>() {
                { "CR0", 10 },
                { "CR1/8", 25 },
                { "CR1/4", 50 },
                { "CR1/2", 100 },
                { "CR1", 200 },
                { "CR2", 450 },
                { "CR3", 700 },
                { "CR4", 1_100 },
                { "CR5", 1_800 },
                { "CR6", 2_300 },
                { "CR7", 2_900 },
                { "CR8", 3_900 },
                { "CR9", 5_000 },
                { "CR10", 5_900 },
                { "CR11", 7_200 },
                { "CR12", 8_400 },
                { "CR13", 10_000 },
                { "CR14", 11_500 },
                { "CR15", 13_000 },
                { "CR16", 15_500 },
                { "CR17", 18_000 },
                { "CR18", 20_000 },
                { "CR19", 22_000 },
                { "CR20", 25_000 },
                { "CR21", 30_000 },
                { "CR22", 41_000 },
                { "CR23", 50_000 },
                { "CR24", 62_000 },
                { "CR25", 75_000 },
                { "CR26", 90_000 },
                { "CR27", 105_000 },
                { "CR28", 120_000 },
                { "CR29", 135_000 },
                { "CR30", 155_000 }
            };


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

        private readonly Dictionary<int, ExperienceThreshold> RankMapping = new Dictionary<int, ExperienceThreshold>()
        {
            { 2, new ExperienceThreshold(150, 200, 300, 600) },
            { 3, new ExperienceThreshold(225, 400, 800, 3_600) },
            { 4, new ExperienceThreshold(375, 500, 800, 3_600) },
            { 5, new ExperienceThreshold(750, 1_100, 3600, 8600) },
            { 6, new ExperienceThreshold(900, 1_400, 3600, 8600) },
            { 7, new ExperienceThreshold(1_100, 1_700, 3600, 8600) },
            { 8, new ExperienceThreshold(1_400, 2_100, 3600, 8600) },
            { 9, new ExperienceThreshold(1_600, 2_400, 8_600, 20_800) },
            { 10, new ExperienceThreshold(1_900, 2_800, 8_600, 20_800) },
            { 11, new ExperienceThreshold(2_400, 3_600, 8_600, 20_800) },
            { 12, new ExperienceThreshold(3_000, 4_500, 8_600, 20_800) },
            { 13, new ExperienceThreshold(3_400, 5_100, 8_600, 20_800) },
            { 14, new ExperienceThreshold(3_800, 5_700, 20_800, 38_400) },
            { 15, new ExperienceThreshold(4_300, 6_400, 20_800, 38_400) },
            { 16, new ExperienceThreshold(4_800, 7_200, 20_800, 38_400) },
            { 17, new ExperienceThreshold(5_900, 8_800, 38_400, 56_200) },
            { 18, new ExperienceThreshold(6_300, 9_500, 38_400, 56_200) },
            { 19, new ExperienceThreshold(7_300, 10_900, 38_400, 56_200) },
            { 20, new ExperienceThreshold(8_500, 12_700, 56_000, 92_000) }
        };

        public decimal EvaluateAXP(Dictionary<string, int> monsters)
        {
            var total = 0;

            foreach (var monster in monsters)
            {
                total += monster.Value * challengeRatings[monster.Key];
            }

            return total * Multiplier(monsters.Values.Sum());
        }
    }
}