3D Sokoban Game
=====================================
=====================================

This 3D Sokoban game was created by Rodolfo Gomez, Azeem Mir, Carlos Rivera, Alex Najera. It was created using Unity version 2021.3.18f1.

---------------------------------------------------------------------
Running the game
1. Navigate to the following folder: \CS4361_ProjectFinalSubmission_Team9\CS4361_ProjectSourceCode_Team9
2. In the folder, simply select Sokoban.exe and run the program. The program is built to be compatible with Windows 64-bit. 	
3. If you are running on MacOS, navigate to the .\CS4361_ProjectFinalSubmission_Team9\CS4361_ProjectSourceCode_Team9\macOS.app\Contents\MacOS folder and run the Sokoban file in that folder.



Gameplay
=====================================

Objective
-----------------------------------------------------------------------
The objective of the game is to maneuver the Forklift around the warehouse map to strategically move and place the crates onto the pallets. In order to win each level, the player must move the crates around without getting them stuck alongside the walls. You can only make one move at a time, and boxes can only be pushed, not pulled. If a crate is stuck along a wall and the player cannot move it away, the player lost the level and must reset and try again. 

Controls
-------------------------------------------------------------------------
W --> Move up (Also rotates and orients the forklift to face up if facing left or right, or moves the forklift backwards and in the up direction if it is facing down)
A  --> Move left (rotates and orients the forklift in the left direction if it is facing either up or down, or moves it backwards in the left direction if the forklift is facing in the right direction)
S  --> Moves down (rotates and orients the forklift in the down direction if facing left or right, or moves the forklift backwards in the down direction if facing up)
D  --> Moves right (rotates and orients the forklift in the right direction if it is facing either up or down, or moves it backwards in the right direction if the forklift is facing in the left direction)

Note: Per the game rules, the forklift cannot move if the desired direction is blocked by a wall. The forklift also cannot move if a crate exists in the tile next to the base of the forklift in the desired move direction.

R --> Reset (Resets the level in the case that an incorrect move is made)
N --> When the player wins a level, a pop up screen will show up and prompt the user to press 'N' to proceed to the next level (please allow a few seconds for the screen to display) 

Good luck and have fun!