using System;

namespace ScoringBowling
{
    class Program
    {
        static void Main(string[] args)
        {
            int frame = 10;     // Number of frames for the Bowling game (defined for the whole game)

            Game bowling = new Game(frame);     // Game bowling = new Game(frame, true, 10); // God Mode with only strikes

            bowling.PlayGame();
            Console.WriteLine("FINAL SCORE : " + bowling.Score());
        }
    }
}

