# nmm_game
This is a desktop application for the **Nine Men's Morris game** developed with WPF and C# (with Visual Studio IDE).

## Rules
Two players move their nine stones on a board with 24 positions. The human plays with red stones and the computer moves blue stones. The human starts the game.

The game consists of three phases:
* phase 1: the players place their stones on the board, in any free position
* phase 2: given that the red player selects one of its stones to move, it can only be moved to a free, neighbour position; the opponent moves also in the same manner
* phase 3: once a player has three stones left, he can move it on any free position on the board (flying)

The game ends when one of the player has only two stones left or there are no possible movements available for him. After 70 moves accumulated by both players, the game ends in a draw.

## Implementation

The application supports the following languages: English, German and French. Every language is depicted by a button with a flag. 

Every stone is graphically represented by an Ellipse, that is either colored in Red (human player), Blue (computer player) or Yellow (free position). When an ellipse is clicked an event is fired up
which deals with one of the following actions:
* if it's phase 1 of the game, the click means a red stone should be placed
* if it's phase 2 or 3 of the game, a click on a red ellipse means that is the stone to be moved, followed by a second click on a position on the board meaning that the stone will be placed there
* if red formed a mill, a click on a blue stone means that the opponent stone will be removed

When a player formed a mill and selects one of the opponent's stone for removing, the program checks and gives warning whether the opponent stone is inside
a mill. In the case when all the opponent's stones are inside mills, the player is allowed to remove a stone inside a mill.

A dangerous mill is considered an incomplete mill where one stone is missing, like in the picture below where a dangerous red mill is formed with stones 22 and 24.

![Dangerous red mill](https://github.com/StefaniaDuma/nmm_game/blob/master/Images/dangerousMill.png)

I implemented a Greedy solution to the game where:
* the first move of the blue player is random
* check whether red will soon form a mill (two neighbour red stones) and whether blue will form soon a mill 
* if there are dangerous red and blue mills, completing a blue mill has preference over blocking the formation of a red mill because after completing a blue mill, it will remove a red stone 
that is inside a dangerous/ incomplete red mill)
* if there is an incomplete red mill and no incomplete blue mill, then move a blue stone to block the formation of red mill
* if there are no incomplete red mills and no incomplete blue mills, then move a blue stone in the vecinity of another blue stone (move according to the rules of each phase)
* when removing a red stone, choose one from inside an incomplete mill or one in the vecinity of an incomplete mill if available, else choose a random one

The advantage of using this greedy approach is that the opponent moves very fast (a timer with delay was added when blue moves so that the human eye can 
better perceive its move). The disadvantage is that the opponent doesn't always move in its best interest.

The executable file of the program can be found in the *bin/Debug* folder.
