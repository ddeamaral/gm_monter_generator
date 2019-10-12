namespace src
{
    internal class ExperienceThreshold
    {
        internal ExperienceThreshold(int Hard, int Deadly, int rankMinimumAdjustedExperience, int rankMaxiumumAdjustedExperience)
        {
            this.Hard = Hard;
            this.Deadly = Deadly;
            this.RankMinimumAdjustedExperience = rankMinimumAdjustedExperience;
            this.RankMaxiumumAdjustedExperience = rankMaxiumumAdjustedExperience;
        }

        internal readonly int Hard;

        internal readonly int Deadly;

        internal readonly int RankMinimumAdjustedExperience;

        internal readonly int RankMaxiumumAdjustedExperience;
    }
}