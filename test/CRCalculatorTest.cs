using System.Collections.Generic;
using System.Linq;
using gm_monster;
using NUnit.Framework;
using src;

namespace gm_monster_test
{
    internal class CRCalculatorTest
    {
        [Test]
        public void GivenFiveLevelFivePlayersWithDeadlyRank_WhenInvoked_ThenGeneratesAllPossibleChallengeRatingOutcomes()
        {
            // Arrange

            var encounterRequest = new EncounterGenerationRequest()
            {
                Players = new List<int>() { 5, 5, 5, 5 },
                Difficulty = Difficulty.Deadly,
                Rank = 'D',
                MaximumAdjustedExperience = 8400,
                MinimumAdjustedExperience = 3600
            };
            var challengeRatingCalculator = new ChallengeRatingCalculator(encounterRequest);

            HashSet<KeyValuePair<string, int>> test = new HashSet<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("CR2", 3) };

            // Act
            var result = challengeRatingCalculator.GenerateChallengeRatingOutcomes();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GivenFivePlayers_WhenCalculated_ReturnsExpectedAXP()
        {
            // Arrange

            var encounterRequest = new EncounterGenerationRequest()
            {
                Players = new List<int>() { 5, 5, 5, 5 },
                Difficulty = Difficulty.Deadly,
                Rank = 'D',
                MaximumAdjustedExperience = 8400,
                MinimumAdjustedExperience = 3600
            };
            var challengeRatingCalculator = new ChallengeRatingCalculator(encounterRequest);

            // CR = Key, Quantity = value
            var inputOne = new Dictionary<string, int>() { { "CR1", 1 } };
            var inputTwo = new Dictionary<string, int>() { { "CR1", 2 } };
            var inputThreeToSix = new Dictionary<string, int>() { { "CR1", 3 } };
            var inputSevenToTen = new Dictionary<string, int>() { { "CR5", 2 }, { "CR2", 5 } };
            var inputElevenToFourteen = new Dictionary<string, int>() { { "CR1", 11 } };
            var inputFifteenOrMore = new Dictionary<string, int>() { { "CR1", 15 } };

            var expectedOne = 200;
            var expectedTwo = 200 * 2 * 1.5;
            var expectedThreeToSix = 200 * 3 * 2;
            var expectedSevenToTen = 5850 * 2.5;
            var expectedElevenToFourteen = 200 * 11 * 3;
            var expectedFifteenOrMore = 200 * 15 * 4;

            // Act
            var resultOne = challengeRatingCalculator.EvaluateAXP(inputOne);
            var resultTwo = challengeRatingCalculator.EvaluateAXP(inputTwo);
            var resultThreeToSix = challengeRatingCalculator.EvaluateAXP(inputThreeToSix);
            var resultSevenToTen = challengeRatingCalculator.EvaluateAXP(inputSevenToTen);
            var resultElevenToFourteen = challengeRatingCalculator.EvaluateAXP(inputElevenToFourteen);
            var resultFifteenOrMore = challengeRatingCalculator.EvaluateAXP(inputFifteenOrMore);

            // Assert
            Assert.AreEqual(expectedOne, resultOne, "one monster range multiplier invalid calculation");
            Assert.AreEqual(expectedTwo, resultTwo, "two monster range multiplier invalid calculation");
            Assert.AreEqual(expectedThreeToSix, resultThreeToSix, "three to six monster range multiplier invalid calculation");
            Assert.AreEqual(expectedSevenToTen, resultSevenToTen, "seven to ten monster range multiplier invalid calculation");
            Assert.AreEqual(expectedElevenToFourteen, resultElevenToFourteen, "eleven to fourteen monster range multiplier invalid calculation");
            Assert.AreEqual(expectedFifteenOrMore, resultFifteenOrMore, "fifteen or more monster range multiplier invalid calculation");
        }

        [Test]
        public void GivenValidNumberOfPlayers_WhenEvaluated_ReturnsValidEncountersFilteredByNumberOfPlayers()
        {
            // Arrange
            var encounterRequest = new EncounterGenerationRequest()
            {
                Players = new List<int>() { 5, 5, 5, 5 },
                Difficulty = Difficulty.Deadly,
                Rank = 'D',
                MaximumAdjustedExperience = 8400,
                MinimumAdjustedExperience = 3600
            };
            var challengeRatingCalculator = new ChallengeRatingCalculator(encounterRequest);

            HashSet<string> testMatchups = new HashSet<string>()
            {
                "CR4 3",
                "CR1/4 20"
            };

            // Act
            var result = challengeRatingCalculator.TransformIntoValidEncounters(testMatchups).ToList();

            // Assert
            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.Any(encounter => encounter.Monsters.ContainsKey("CR4") && encounter.Monsters["CR4"] == 3));
        }
    }
}