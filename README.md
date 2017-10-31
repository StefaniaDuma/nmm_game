# nmm_game
A Greedy solution to the Nine Men's Morris game
This is a desktop application for the **Nine Men's Morris game** developed with WPF and C# (with Visual Studio IDE).

## Rules
Two players move their nine stones on a board with 24 positions. The human plays with red stones and the computer moves blue stones. The human starts the game.

The game consists of three phases:
* phase 1: the players places their stones on the board, in any free position
* phase 2: given that the red player selects one of its stones to move, it can only be moved to a free, neighbour position; the opponent moves also in the same manner
* phase 3: once a player has three stones left, he can move it on any free position on the board (flying)

The game ends when one of the player has only two stones left or there are no possible movements available for him. After 60 moves accumulated by both players, the game ends in a draw.

## Implementation

The application supports the following languages: English, German and French. Every language is depicted by a button with a flag. 

Every stone is graphically represented by an Ellipse, that is either colored in Red (human player), Blue (computer player) or Yellow (free position). When an ellipse is clicked an event is fired up
which deals with one of the following actions:
* if it's phase 1 of the game, the click means a red stone should be placed
* if it's phase 2 of the game, a click on a red ellipse means that is the stone to be moved, followed by a second click on a position on the board meaning that the stone will be placed there
* if red formed a mill, a click on a blue stone means that the opponent stone will be removed

When a player formed a mill and selects one of the opponent's stone for removing, the program checks and gives warning whether the opponent stone is inside
a mill. In the case when all the opponent's stones are inside mills, the player is allowed to remove a stone inside a mill.

The board is represented in two ways:
- a bidirectional matrix **a** which can have four possible values:
* 0: free position
* 1: position occupied by red player
* 2: position occupied by blue player
* 8: "wall" or position where no player can move
- a vector of 24 integers which represent the 24 places in the board with available positions to move

A dangerous mill is considered an incomplete mill where one stone is missing, like in the picture below where a dangerous red mill is formed with stones 22 and 24.
![Dangerous red mill](https://github.com/StefaniaDuma/nmm_game/Images/dangerousMill.png | width=100)

I implemented a Greedy solution to the game where:
* the first move of the blue player is random
* if the game is in phase 1 check whether red will soon form a mill (two neighbour red stones) and whether blue will form soon a mill; 
completing blue mill has preference over blocking formation of red mill because after completing a blue mill, it will remove a red stone 
that is inside a dangerous/ incomplete red mill)
