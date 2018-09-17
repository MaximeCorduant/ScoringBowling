using System;
using System.Collections.Generic;
using System.Threading;


namespace ScoringBowling
{
    public class Game
    {
        public static bool autoPlay = false ;   // True : Game will play itself / False : The user throws the ball with the "space" bar
        public static int nbFrame;              // Number of frames for the Bowling game (defined for the whole game in Program.cs)

        private int _pinsStart = 10;            // Number of pins at the beginning of a frame (defined for the whole game)
        private int _pinsLeft;                  // Number of pins left
        private int _pins;                      // Number of pins knocked down
        private int _currentFrame;              // Current frame of the game

        private List<List<int>> _totalList;     // List of frames including a list of the throw scores
        private List<int> _frameList;           // List of the scores of a frame
        private List<int> _scoreList;           // List of the total score of each frame
        private int _finalScore;                // Final score
        private int _nbRolls;                   // Number of rolls in the current frame (between 1 and 3)
        
        private bool _previousSpare;            // Indicates if there was a spare on the PREVIOUS frame
        private bool _previousStrike;           // Indicates if there was a strike on the PREVIOUS frame
        private bool _strike;                   // Indicates if there is a strike now
        private bool _spare;                    // Indicates if there was a spare now

        private bool _godMode = false;          // Strikes or spares guaranteed : you choose the number of knocked down pins in the first roll and get a spare afterwards (unless you chose 10 to only have strikes)
        private int _pinsWanted;                // Fixed number of pins used in the god mode


        // CONSTRUCTORS
        /// Classic constructor
        public Game(int p_frames)
        {
            nbFrame        = p_frames;
            _totalList      = new List<List<int>>();
            _scoreList      = new List<int>();
        }

        /// God mode constructor
        public Game(int p_frames, bool p_godMode, int p_pinsWanted)
        {
            nbFrame = p_frames;
            _totalList = new List<List<int>>();
            _scoreList = new List<int>();

            _godMode = p_godMode;
            _pinsWanted = p_pinsWanted;
        }


        // METHODS
        /// Play the whole game
        public void PlayGame()
        {
            Print.Instructions();

            for (int i = 0; i < nbFrame; i++)
            {
                PlayFrame(i);
            }

            _finalScore = GetScore();
        }

        /// Initiates the PlayFrame method variables
        private void InitPlayFrame(int p_frame)
        {
            _nbRolls = 0;
            _currentFrame = p_frame;
            _frameList = new List<int>();
            _pinsLeft = _pinsStart;
            _previousSpare = _spare;
            _previousStrike = _strike;
            _spare = false;
            _strike = false;
        }

        /// Play a frame of the game : includes different number of rolls
        /// 1 roll in case of a strike
        /// 2 rolls generally
        /// 3 rolls in case of a spare or a strike in the last frame
        private void PlayFrame(int p_frame)
        {
            InitPlayFrame(p_frame);
            int bonus = 0;
            int frameResult = 0;

            // 1st throw
            ThrowBall();

            // 2nd throw (happens if there is no strike on the previous roll OR if it's the last frame)
            if (!_strike || (p_frame == nbFrame-1))
            {
                if (_strike) { _pinsLeft = _pinsStart; }
                ThrowBall();
            }

            // Add scores (scores of each roll and score of the frame) to the lists
            frameResult += _frameList[0] + _frameList[1];
            _scoreList.Add(frameResult);
            _totalList.Add(_frameList);

            // Bonus Spare / Strike
            if (p_frame > 0)
            {
                bonus += GetSpareBonus(p_frame) + GetStrikeBonus(p_frame);
                _scoreList[p_frame - 1] += bonus;
            }
  
            // Last frame case : strike or spare --> 3rd throw
            if (p_frame == nbFrame - 1 && (_spare || _strike))
            {
                if (_pinsLeft == 0) { _pinsLeft = _pinsStart; }
                ThrowBall();

                _scoreList[_currentFrame] += _frameList[2];
            }

            Print.DrawScore(_totalList, _scoreList, _nbRolls);
        }

        /// Throwing the ball :
        /// - Makes you play with the space bar if the game in not in auto mode.
        /// - Generates randomly the number of pins knocked down and call the Roll methods (or take the chosen number of pins in the god mode)
        private void ThrowBall()
        {
            _nbRolls++;

            if (! autoPlay)
            {
                while (Console.ReadKey().Key != ConsoleKey.Spacebar) { }
            }
            else
            {
                // The random number is based on the clock system. However, the game execution is almost instantaneous, therefore, the random function almost always returns the same number without the "sleep()" function.
                Thread.Sleep(6);
            }

            // Random number of pins knocked down
            Random rnd = new Random();
            _pins = rnd.Next(_pinsLeft + 1);

            // God Mode
            if (_godMode) {
                if (_nbRolls == 1 || _nbRolls == 3)
                {
                    _pins = _pinsWanted;
                } else
                {
                    _pins = _pinsLeft;
                }
            }

            Roll(_pins);
        }

        /// Rolling ball :
        /// Decrease the numbers of pins left and add the score to the list iFrameList
        private void Roll(int p_pins)
        {
            _frameList.Add(p_pins);
            _pinsLeft -= p_pins;

            // Spare case
            if (_nbRolls > 1 && _pinsLeft == 0 && p_pins !=10)
            {
                _spare = true;
                Print.Throw(p_pins, _spare);
            }
            else
            {
                // Strike case
                if (p_pins == 10)
                {
                    _strike = true;
                    if (_currentFrame != nbFrame-1)
                    {
                        _frameList.Add(0);  // Virtual second throw
                    }
                }
                Print.Throw(p_pins);
            }
        }

        /// Getting the score :
        /// Returns the current score by summing the scores of each played frame (from the list _scoreList)
        public int GetScore()
        {
            int result = 0;
            foreach (int score in _scoreList)
            {
                result += score;
            }

            return result;
        }

        /// Return the finale score
        public int Score()
        {
            return _finalScore;
        }

        /// Getting the strike bonus :
        /// Returns the bonus based on the 2 balls rolled after the strike
        private int GetStrikeBonus(int p_frame)
        {
            int strikeBonus = 0;

            if (_previousStrike)
            {
                // Case of 2 strikes in a row
                if (_strike && _nbRolls != 3)
                {
                    strikeBonus = _totalList[p_frame - 1][0] + _totalList[p_frame][0];  
                } else
                {
                    strikeBonus = _totalList[p_frame][0] + _totalList[p_frame][1];
                }
                
                _previousStrike = false;
            }

            return strikeBonus;
        }

        /// Getting the spare bonus :
        /// Returns the bonus based on the ball rolled after the spare
        private int GetSpareBonus(int p_frame)
        {
            int spareBonus = 0;

            if (_previousSpare)
            {
                spareBonus = _totalList[p_frame][0];
                _previousSpare = false;
            }

            return spareBonus;
        }
    }
}
