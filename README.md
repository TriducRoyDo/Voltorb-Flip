# Voltorb-Flip
Minesweeper/Picross hybrid created in C#/.net. Based off a minigame featured in Pokemon HGSS.

![alt text](https://cdn.bulbagarden.net/upload/0/02/Voltorb_Flip.png)

How the original game looks.

![alt text](https://github.com/TriducRoyDo/Voltorb-Flip/blob/master/Voltorb%20Flip/demo.jpg)

How my current version looks like. 

## How To Play
- Each level consists of a 5x5 board of cards. 

- Each card is either **x1**, **x2**, **x3**, or **BOOM!** Flipping any of the multiplier cards (x1, x2, x3) will increase your current level score by that much (if your score is 0, it will be increased to 1 before multiplying) while BOOM! will get you a game over and set your current level score to 0. 

- The level is cleared when the player reveals all x2/x3 cards on the board. The game can also automatically prematurely end if the player hits a BOOM! card instead.

- At the end every column and row is a set of two numbers which are value-bomb indicators. The **top number is the total value of the multiplier cards** in said row/column while the **bottom number is the number of bombs** in that row/column. Use this to your advantage to make educated guesses on which rows/columns are more dangerous than others to flip cards on.

![alt text](https://github.com/TriducRoyDo/Voltorb-Flip/blob/master/Voltorb%20Flip/revealed_example.jpg)

*For example, look at this board where all the cards are revealed. In the first row, left to right, if you add them up you will get 5 (1 + 1 + 1 + 1 + 1) and 0 bombs. Indeed the value-bomb indicator at the end of the row (blue) reads 5 - 0. Similarly, if you look at the first column top to bottom, you'll see the values in that column add up to 6 (1 + 2 + 2 + 1) and there is one bomb in the column, adding up to 6 - 1 on the first column value-bomb indicator (also blue).* 

- Higher levels increase the number of x2/x3 cards on the board but also increase the number of bomb cards.

- When the player gets game over by hitting the bomb, they may be demoted to a lower level if the total number of multiplier cards they revealed is less then the current level's number, in that case they will be demoted to the level number equal to how many multiplier cards they did reveal in that level before flipping a bomb. For example, say in level 3, I flip 4 multiplier cards then hit a bomb, I will not be demoted since I flipped more multiplier cards than the current level's number. But if I am in level 7 and flip 2 multiplier cards before hitting a bomb, 2 is less than level 7 so I will be demoted, and since I flipped only 2 multilpier cards I will be demoted all the way back to level 2!

- Clicking the button on the bottom right activates memo mode. While memo mode is on, clicking on unrevealed cards will not flip them. Instead it will mark those cards white (and clicking a marked card will unmark it). While this does not affect gameplay, it can help players track where they think bombs are on the board or which rows/columns are not worth clicking. Once you're done labeling, click the memo mode button again to start revealing cards again. Note that any marks you do in memo mode will persist even with memo mode off. 

- Tip: Flipping a x1 card does nothing to increase your score or get you closer to clearing a level. If you deduce a card can only be either a x1 or bomb based on the column/row indicators, it's probably a good idea to not even bother flipping it!

- Tip 2: When starting a level, if you see any rows are columns where the bomb number (the bottom number on value-bomb indicators) is 0 that means that row/column is completely safe and you should flip all the cards in that row/column first. Even if all the cards are x1, these will still count as multiplier cards flipped for the level which helps if you later hit a bomb and prevent any big level demotions.

- Secret: If a player clears 5 levels in a row, each level flipping at least 8 multiplier cards before winning, then they will be promoted straight to the max level 8!

## Additional resources
- https://bulbapedia.bulbagarden.net/wiki/Voltorb_Flip - Explains the game's mechanics and some possible strategies, also reveals the possible number of bombs/x2/x3 cards that can appear per level. 

- http://voltorbflip.com/ - A voltorb flip solver. While this was created for the original Voltorb Flip minigame in Pokemon HGSS, since my game is mechnically identical to the original, this should work for it as well. Note that while some levels can be "solved" with logic and deduction, the game still has a healthy amount of luck involved such that there will always be a point where the player has to flip a card with uncertainity (for example a new board where all rows/columns have at least 1 bomb), thus the game cannot be solved by a computer every time and this solver's algorithm is not necessarily the optimal or safest way to play the game either. 
