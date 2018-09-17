using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScoringBowling.Tests
{
    [TestClass]
    public class ScoringBowlingTests
    {
        /// Test if the score if between 0 and 300 for a 10 frames game
        [TestMethod]
        public void ScoreBetween0and300Test()
        {
            int finalScore = 0;
            Game bowling = new Game(10);
            Game.autoPlay = true;
            bowling.PlayGame();

            finalScore = bowling.Score();

            Assert.IsTrue(0 <= finalScore && finalScore <= 300);
        }

        /// Test the max score for a 10 frames game
        [TestMethod]
        public void ScoreMaxTest()
        {
            int finalScore = 0;
            Game bowling = new Game(10, true, 10);
            Game.autoPlay = true;
            bowling.PlayGame();

            finalScore = bowling.Score();

            Assert.AreEqual(finalScore, 300);
        }

        /// Test the score when 5 pins are knocked down on each roll for a 10 frames game
        [TestMethod]
        public void ScoreWith5KnockedDownTest()
        {
            int finalScore = 0;
            Game bowling = new Game(10, true, 5);
            Game.autoPlay = true;
            bowling.PlayGame();

            finalScore = bowling.Score();

            Assert.AreEqual(finalScore, 150);
        }
    }
}
