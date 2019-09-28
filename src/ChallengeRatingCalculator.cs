using System;
using System.Collections.Generic;
using System.Linq;

namespace src
{
    public class ChallengeRatingCalculator
    {
        public ChallengeRatingCalculator (Difficulty difficulty, List<int> players)
        {
            if (players is null || players.Count <= 0)
                throw new ArgumentOutOfRangeException ("You must have at least 1 player");

            this.difficulty = difficulty;
            this.players = players;
        }
        
        private int DeadlyExperience = players

        private readonly List<int> players;

        private const Dictionary<string, int> challengeRatings = { { "CR0", 10 },
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

        private decimal Multiplier (int numberOfMonsters)
        {
            switch (numberOfMonsters)
            {
                case int i when (numberOfMonsters == 1):
                    return 1;
                case int i when (numberOfMonsters == 2):
                    return 1.5;
                case int i when (numberOfMonsters >= 3 && numberOfMonsters <= 3):
                    return 2;
                case int i when (numberOfMonsters >= 7 && numberOfMonsters <= 10):
                    return 2.5;
                case int i when (numberOfMonsters >= 11 && numberOfMonsters <= 14):
                    return 3;
                case int i when (numberOfMonsters >= 15):
                    return 4;
                default:
                    return 1;
            }
        }

        private Dictionary<int, ExperienceThreshold> RankMapping = { { 2, new ExperienceThreshold (150, 200) },
            { 3, new ExperienceThreshold (225, 400) },
            { 4, new ExperienceThreshold (375, 500) },
            { 5, new ExperienceThreshold (750, 1_100) },
            { 6, new ExperienceThreshold (900, 1_400) },
            { 7, new ExperienceThreshold (1_100, 1_700) },
            { 8, new ExperienceThreshold (1_400, 2_100) },
            { 9, new ExperienceThreshold (1_600, 2_400) },
            { 10, new ExperienceThreshold (1_900), 2_800 },
            { 11, new ExperienceThreshold (2_400, 3_600) },
            { 12, new ExperienceThreshold (3_000, 4_500) },
            { 13, new ExperienceThreshold (3_400, 5_100) },
            { 14, new ExperienceThreshold (3_800_5_700) },
            { 15, new ExperienceThreshold (4_300, 6_400) },
            { 16, new ExperienceThreshold (4_800, 7_200) },
            { 17, new ExperienceThreshold (5_900, 8_800) },
            { 18, new ExperienceThreshold (6_300, 9_500) },
            { 19, new ExperienceThreshold (7_300, 10_900) },
            { 20, new ExperienceThreshold (8_500, 12_700) }
        };

        private bool CanBeAdded (int maxPlayerExperience)
        {

            return false;
        }

        // public bool Fits (int maxAdjustedExperience, int minAdjustedExperience, int numberOfMonsters, int currentExperience, int monsterExp)
        // {
        //     var xp = currentExperience + monsterExp;
        //     return 
        // }
    }
}