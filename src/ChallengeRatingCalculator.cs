using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using gm_monster;

namespace src
{

    public class ChallengeRatingCalculator
    {
        public ChallengeRatingCalculator(EncounterGenerationRequest request)
        {
            if (request is null || request.Players is null || request.Players.Count <= 0)
                throw new ArgumentOutOfRangeException("You must have at least 1 player");

            this.Difficulty = request.Difficulty;
            this.Players = request.Players;
            this.AssumedPartyRank = DeterminePartyRank(request.Rank);
            this.MaximumMonsters = request.Players.Count * 2;
            this.EncounterGenerationRequest = request;
        }

        private readonly int AssumedPartyRank;

        private readonly int MaximumMonsters;

        private readonly EncounterGenerationRequest EncounterGenerationRequest;

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

            if (EncounterGenerationRequest.MaximumAdjustedExperience > 0 && EncounterGenerationRequest.MinimumAdjustedExperience > 0)
            {
                rankMaximum = EncounterGenerationRequest.MaximumAdjustedExperience;
                minimumDifficultyExperience = EncounterGenerationRequest.MinimumAdjustedExperience;
            }

            Console.WriteLine("Calculated values for supplied player count:");
            Console.WriteLine($"Adjusted Experience Range: ({minimumDifficultyExperience} adj xp) - ({rankMaximum} adj xp)");

            GenerateEncounters(minimumDifficultyExperience, rankMaximum);

            return true;
        }

        internal void GenerateEncounters(int minimumAdjustedExperience, int maximumAdjustedExperience)
        {
            // Get all possible combinations
            var monsterMatchupPermutations = FetchAllStringifiedPermutations();

            var validEncounters = TransformIntoValidEncounters(monsterMatchupPermutations);

            WriteMacroScript(validEncounters);
        }

        public IEnumerable<Encounter> TransformIntoValidEncounters(HashSet<string> monsterMatchupPermutations)
        {
            // Mutate into monster dictionaries
            var encounters = TransmogrifyIntoEncounters(monsterMatchupPermutations);

            // run them all through the adjxp calculator
            return encounters
                .Select(encounter => new Encounter { Monsters = encounter, AdjustedExperience = EvaluateAXP(encounter) })
                .Where(encounter => encounter.Monsters.Values.Sum() <= MaximumMonsters
                && encounter.AdjustedExperience >= EncounterGenerationRequest.MinimumAdjustedExperience
                && encounter.AdjustedExperience <= EncounterGenerationRequest.MaximumAdjustedExperience);
        }

        private void WriteMacroScript(IEnumerable<Encounter> validEncounters)
        {
            // format strings
            var output = GenerateMacroScript(validEncounters.OrderBy(encounter => encounter.AdjustedExperience).ToArray());

            using (var streamWriter = new StreamWriter(Constants.OutputPath("macro.txt")))
            {
                streamWriter.Write(output);
            }
        }

        private string GenerateMacroScript(Encounter[] validEncounters)
        {
            var macroText = new StringBuilder();
            macroText.AppendLine("!import-table --DylansIncredibleMonsterGenerator --hide");

            for (int i = 0; i < validEncounters.Length; i++)
            {
                macroText.AppendLine($"!import-table-item --DylansIncredibleMonsterGenerator --AXP: {validEncounters[i].AdjustedExperience} Monsters:{string.Join(' ', validEncounters[i].Monsters.Where(m => m.Value > 0).Select(m => $"{m.Key}x{Math.Floor((decimal)m.Value)}"))} --1 --");
            }

            return macroText.ToString();
        }

        private IEnumerable<Dictionary<string, int>> TransmogrifyIntoEncounters(HashSet<string> monsterMatchupPermutations)
        {
            // split string into arrays ([CR0, 1, CR1/4, 1...])
            var valueArrays = monsterMatchupPermutations.Select(monsterMatchup => monsterMatchup.Split(' ').ToArray());

            // get a collection of anonymous types
            var zippedUp = valueArrays.Select((item, index) => new { values = item.Where((cr, i) => i % 2 != 0), keys = item.Where((cr, i) => i % 2 == 0) });

            return zippedUp.Select(matches => matches.keys.Zip(matches.values, (key, value) => new KeyValuePair<string, int>(key, int.Parse(value))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        private HashSet<string> FetchAllStringifiedPermutations()
        {
            var allPossiblePermutations = new HashSet<string>();

            foreach (var challengeRating in Constants.ChallengeRatings.Keys)
            {
                for (int i = 1; i < 13; i++)
                {
                    var horizontalPermutation = UsingWhile(new Challenge { cr = challengeRating, quantity = i });
                    allPossiblePermutations.UnionWith(horizontalPermutation);
                }
            }

            return allPossiblePermutations;
        }

        private HashSet<string> UsingWhile(Challenge currentCR)
        {
            // If we're at CR 1/8, we want to start at CR 1/4
            var startingIndex = Constants.ChallengeRatings.Keys.ToList().IndexOf(currentCR.cr);

            // We're at CR 30
            if (startingIndex == Constants.ChallengeRatings.Keys.Count - 1)
            {
                return new HashSet<string>();
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

                for (int i = challengeRatingsList.IndexOf(currentCR.cr) + 1; i < 32; i++)
                {
                    var challengeRating = challengeRatingsList[i];

                    if (line.ContainsKey(challengeRating))
                    {
                        line[challengeRating] = i <= iterationIndex ? iterations : iterations - 1;
                        results.Add(new Dictionary<string, int>(line));

                        if (line.Values.Sum() == (line.Count - 1) * iterations + currentCR.quantity)
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

                done = results.Last().All(q => q.Value == MaximumMonsters || q.Key == currentCR.cr);
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