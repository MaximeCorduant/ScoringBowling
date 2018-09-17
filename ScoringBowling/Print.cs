using System;
using System.Collections.Generic;

namespace ScoringBowling
{
    public static class Print
    {
        /// Game Start - print the instructions
        public static void Instructions()
        {
            Console.WriteLine("Press [P] to Play or any key to simulate a game");

            if (Game.autoPlay == false && Console.ReadKey().Key == ConsoleKey.P)
            {
                    Game.autoPlay = false;
                    Console.WriteLine("\n[ Use the Space bar to throw the ball ]");
            } else
            {
                Game.autoPlay = true;
            }

            Console.WriteLine("\n");
        }

        /// Print the number of pins knocked down
        public static void Throw(int p_pins, bool p_spare = false)
        {
            Console.Write("Throw... ");

            if (p_spare)
            {
                Console.Write(" Spare !\n");
            }
            else
            {
                switch (p_pins)
                {
                    case 0:
                        Console.Write(" Gutter !\n");
                        break;
                    case 1:
                        Console.Write(p_pins + " pin knocked down\n");
                        break;
                    case 10:
                        Console.Write("STRIKE !\n");
                        break;
                    default:
                        Console.Write(p_pins + " pins knocked down\n");
                        break;
                }
            }
        }

        /// Draw the whole scoreboard
        public static void DrawScore(List<List<int>> p_score, List<int> p_frameScore, int p_nbRoll)
        {
            int frameScore = 0;
            int currentFrame = 0;

            foreach (List<int> frame in p_score)
            {
                PrintThrows(frame, currentFrame, p_nbRoll);
                currentFrame++;
            }
            Console.Write("| \n");

            for (int i = 0; i < p_frameScore.Count; i++)
            {
                frameScore += p_frameScore[i];
                PrintFrameScore(frameScore, i, p_nbRoll);
            }

            Console.Write("| \n\n");
        }

        /// Print the first line of the scoreboard
        private static void PrintThrows(List<int> p_frame, int p_currentFrame, int p_nbRoll)
        {
            string throws = "";

            // 3 rolls case
            if (p_nbRoll == 3 && p_currentFrame == Game.nbFrame - 1)
            {
                // Spare case 1
                if (p_frame[0] + p_frame[1] == 10 && p_frame[1] != 0)
                {
                    throws = "| " + p_frame[0] + "  " + "/" + "  " + p_frame[2] + " ";
                }
                // Spare case 2
                else if (p_frame[1] + p_frame[2] == 10 && p_frame[2] != 0)
                {
                    throws = "| " + p_frame[0] + "  " + p_frame[1] + "  " + "/" + " ";
                }
                else
                {
                    throws = "| " + p_frame[0] + "  " + p_frame[1] + "  " + p_frame[2] + " ";
                }

                Console.Write(throws.Replace("10", "X"));
            }
            else
            {
                // Spare case
                if (p_frame[0] + p_frame[1] == 10 && p_frame[1] != 0)
                {
                    throws = "| " + p_frame[0] + "   " + "/" + " ";
                }
                else
                {
                    // Strike case
                    if (p_frame[0] == 10)
                    {
                        throws = "| " + p_frame[0] + "   " + "-" + " ";
                    }
                    else
                    {
                        throws = "| " + p_frame[0] + "   " + p_frame[1] + " ";
                    }

                }
                Console.Write(throws.Replace("10", "X"));
            }
        }

        ///  Print the second line of the scoreboard
        private static void PrintFrameScore(int p_score, int p_currentFrame, int p_nbRoll)
        {
            if (p_nbRoll == 3 && p_currentFrame == Game.nbFrame - 1)
            {
                switch (p_score.ToString().Length)
                {
                    case 1:
                        Console.Write("|    " + p_score + "    ");
                        break;

                    case 2:
                        Console.Write("|    " + p_score + "   ");
                        break;

                    default:
                        Console.Write("|   " + p_score + "   ");
                        break;
                }
            }
            else
            {
                switch (p_score.ToString().Length)
                {
                    case 1:
                        Console.Write("|   " + p_score + "   ");
                        break;
                    case 2:
                        Console.Write("|   " + p_score + "  ");
                        break;
                    default:
                        Console.Write("|  " + p_score + "  ");
                        break;
                }
            }
        }
    }  
}

