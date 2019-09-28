namespace src
{
    public class ExperienceThreshold
    {
        public ExperienceThreshold (int Hard, int Deadly)
        {
            this.Hard = Hard;
            this.Deadly = Deadly;
        }

        public readonly int Hard { get; set; }

        public readonly int Deadly { get; set; }
    }
}