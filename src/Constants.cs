using System.Collections.Generic;
using System.IO;

namespace src
{
    internal class Constants
    {
        private static string Environment => Directory.GetCurrentDirectory();

        internal static string OutputPath(string filename = "")
        {
            return Path.Combine(Environment, filename);
        }

        // CR0 12 CR1/8 12 CR1/4
        internal static readonly Dictionary<string, int> ChallengeRatings = new Dictionary<string, int>()
        {  { "CR1/4", 50 }, { "CR1/2", 100 }, { "CR1", 200 }, { "CR2", 450 }, { "CR3", 700 }, { "CR4", 1_100 }, { "CR5", 1_800 }, { "CR6", 2_300 }, { "CR7", 2_900 }, { "CR8", 3_900 }, { "CR9", 5_000 }, { "CR10", 5_900 }, { "CR11", 7_200 }, { "CR12", 8_400 }, { "CR13", 10_000 }, { "CR14", 11_500 }, { "CR15", 13_000 }, { "CR16", 15_500 }, { "CR17", 18_000 }, { "CR18", 20_000 }, { "CR19", 22_000 }, { "CR20", 25_000 }, { "CR21", 30_000 }, { "CR22", 41_000 }, { "CR23", 50_000 }, { "CR24", 62_000 }, { "CR25", 75_000 }, { "CR26", 90_000 }, { "CR27", 105_000 }, { "CR28", 120_000 }, { "CR29", 135_000 }, { "CR30", 155_000 }
        };

        internal static readonly Dictionary<int, ExperienceThreshold> RankMapping = new Dictionary<int, ExperienceThreshold>()
        { { 2, new ExperienceThreshold(150, 200, 300, 600) }, { 3, new ExperienceThreshold(225, 400, 800, 3_600) }, { 4, new ExperienceThreshold(375, 500, 800, 3_600) }, { 5, new ExperienceThreshold(750, 1_100, 3600, 8600) }, { 6, new ExperienceThreshold(900, 1_400, 3600, 8600) }, { 7, new ExperienceThreshold(1_100, 1_700, 3600, 8600) }, { 8, new ExperienceThreshold(1_400, 2_100, 3600, 8600) }, { 9, new ExperienceThreshold(1_600, 2_400, 8_600, 20_800) }, { 10, new ExperienceThreshold(1_900, 2_800, 8_600, 20_800) }, { 11, new ExperienceThreshold(2_400, 3_600, 8_600, 20_800) }, { 12, new ExperienceThreshold(3_000, 4_500, 8_600, 20_800) }, { 13, new ExperienceThreshold(3_400, 5_100, 8_600, 20_800) }, { 14, new ExperienceThreshold(3_800, 5_700, 20_800, 38_400) }, { 15, new ExperienceThreshold(4_300, 6_400, 20_800, 38_400) }, { 16, new ExperienceThreshold(4_800, 7_200, 20_800, 38_400) }, { 17, new ExperienceThreshold(5_900, 8_800, 38_400, 56_200) }, { 18, new ExperienceThreshold(6_300, 9_500, 38_400, 56_200) }, { 19, new ExperienceThreshold(7_300, 10_900, 38_400, 56_200) }, { 20, new ExperienceThreshold(8_500, 12_700, 56_000, 92_000) }
        };
    }
}