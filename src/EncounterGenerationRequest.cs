using System.Collections.Generic;
using src;

namespace gm_monster
{
    public class EncounterGenerationRequest
    {

        public EncounterGenerationRequest()
        {
            Players = new List<int>();
        }

        public EncounterGenerationRequest(List<int> players, Difficulty difficulty, char rank, int minimumAdjustedExperience, int maximumAdjustedExperience)
        {
            this.Players = players;
            this.Difficulty = difficulty;
            this.Rank = rank;
            this.MinimumAdjustedExperience = minimumAdjustedExperience;
            this.MaximumAdjustedExperience = maximumAdjustedExperience;

        }
        public List<int> Players { get; set; }

        public Difficulty Difficulty { get; set; }

        public char Rank { get; set; }

        public int MinimumAdjustedExperience { get; set; }

        public int MaximumAdjustedExperience { get; set; }
    }
}