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

