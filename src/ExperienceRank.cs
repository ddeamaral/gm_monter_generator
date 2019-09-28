namespace src
{
    public class ExperienceThreshold
    {
        public ExperienceThreshold(int Hard, int Deadly, int rankMinimumAdjustedExperience, int rankMaxiumumAdjustedExperience)
        {
            this.Hard = Hard;
            this.Deadly = Deadly;
            this.RankMinimumAdjustedExperience = rankMinimumAdjustedExperience;
            this.RankMaxiumumAdjustedExperience = rankMaxiumumAdjustedExperience;
        }

        public readonly int Hard;

        public readonly int Deadly;

        public readonly int RankMinimumAdjustedExperience;

        public readonly int RankMaxiumumAdjustedExperience;
    }
}