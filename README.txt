+-----------------+
| Scoring Bowling |
+-----------------+
Bowling score algorithm.

[Getting Started]
Execute ./ScoreBowling/bin/Debug/ScoringBowling.exe to launch the Game :
Press P to play manually or any key to simulate a game.
Manual mode : use the space bar to throw the ball.
	
[Code]
God mode : Strikes or spares guaranteed !
You choose the number of knocked down pins in the first roll and get a spare afterwards (unless you chose 10 to only have strikes).
You can use the god mode thanks to the Game constructor.
Ex : Game bowling = new Game(frame, true, 10); // only strikes 

[Tests]
3 tests can be ran :
- Test the max score of a 10 frames game : 300 expected
- Test if the score is between 0 and 300 for a 10 frames game
- Test the score when 5 pins are knocked down on each roll for a 10 frames game