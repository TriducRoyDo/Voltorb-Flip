# Voltorb-Flip
Minesweeper/Picross hybrid created in C#/.net. Based off a minigame featured in Pokemon HGSS.

![alt text](https://cdn.bulbagarden.net/upload/0/02/Voltorb_Flip.png)

How the original game looks.

![alt text](https://github.com/TriducRoyDo/Voltorb-Flip/blob/master/Voltorb%20Flip/prototype1.jpg)

How my current prototype looks like. 

# How To Play
-Each level consists of a 5x5 board of cards. 

-Each card is either **x1**, **x2**, **x3**, or **BOOM!** Flipping any of the multiplier cards will increase your current level score by that much (if your score is 0, it will be increased to 1 before multiplying) while BOOM! will get you a game over and set your current level score to 0. 

-At the end every column and row is a set of two numbers which are value-bomb indicators. The **top number is the total value of the multiplier cards** in said row/column while the **bottom number is the number of bombs** in that row/column. Use this to your advantage to make educated guesses on which rows/columns are more dangerous than others to flip cards on.

-The level is cleared when the player reveals all x2/x3 cards on the board. The game can also automatically prematurely end if the player hits a BOOM! card instead.

-Higher levels increase the number of x2/x3 cards on the board but also increase the number of bomb cards.

Tip: Flipping a x1 card does nothing to increase your score or get you closer to clearing a level. If you deduce a card can only be either a x1 or bomb based on the column/row indicators, it's probably a good idea to not even bother flipping it!
