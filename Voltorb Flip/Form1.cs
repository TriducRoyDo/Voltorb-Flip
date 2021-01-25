using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Voltorb_Flip {
    public partial class Form1 : Form {

        // plays a loud boom sound when a bomb card is clicked.
        private SoundPlayer soundBoom = new SoundPlayer("boom.wav");
        Random random = new Random();
        // this holds the possible card distributions possible in level 1. The tuple is (num of x2 cards, num of x3 cards, and num of bombs)
        // and the rest of the 25 cards that are neither of the 3 are just x1.
        List<(int, int, int)> level1PossibleValues = new List<(int, int, int)>(){(3, 1, 6), (0, 3, 6), (5, 0, 6), (2, 2, 6), (4, 1, 6)};
        int score = 0;

        // used for tracking when the game is done. Game finishes when player clicks a bomb card or gets all
        // the x2 and x3 cards in a level.
        bool gameOver = false;
        int numOf2CardsLeft = 0;
        int numOf3CardsLeft = 0;
        public Form1() {
            InitializeComponent();
            AssignIconsToSquares();
        }


      
        private void AssignIconsToSquares() {
            // The TableLayoutPanel has 25 labels, and the icon list has 25 icons,
            // based on the level, we pull a possible combination of 2, 3, and bomb cards from
            // the appropriate tuple list and place them into appropriate tracking variables.
            var lvl1Values = level1PossibleValues[random.Next(level1PossibleValues.Count)];
            int numOf2Cards = lvl1Values.Item1;
            int numOf3Cards = lvl1Values.Item2;
            int numOfBombs = lvl1Values.Item3;
            // num of x1 cards is every card that is not x2, x3, or a bomb.
            int numOf1Cards = 25 - (numOf2Cards + numOf3Cards + numOfBombs);

            // these are for tracking the x2 and x3 cards during actual gameplay to determine level clears.
            numOf2CardsLeft = lvl1Values.Item1;
            numOf3CardsLeft = lvl1Values.Item2;

            // go through each of the 25 labels and randomly assign them x1, x2, x3, or bomb.
            // valueset checks that the value we roll hasn't already been assigned the max amount of times
            // based on the numofXCards variables above, if it has, just roll again.
            foreach (Control control in tableLayoutPanel1.Controls) {
                Label iconLabel = control as Label;

                //ignore the labels containing card/bomb counts and the 36th unused title
                if (iconLabel.Name == "label1_1" || iconLabel.Name == "label1_2" || iconLabel.Name == "label1_3" || iconLabel.Name == "label1_4" || iconLabel.Name == "label1_5"
                    || iconLabel.Name == "label2_1" || iconLabel.Name == "label2_2" || iconLabel.Name == "label2_3" || iconLabel.Name == "label2_4" || iconLabel.Name == "label2_5"
                    || iconLabel.Name == "label36") {
                }
                else {
                    bool valueSet = false;
                    if (iconLabel != null) {
                        while (!valueSet) {
                            int randomNum = random.Next(4);
                            switch (randomNum) {
                                case 0:
                                    if (numOf2Cards > 0) {
                                        iconLabel.Text = "x2";
                                        numOf2Cards--;
                                        valueSet = true;
                                    }
                                    break;
                                case 1:
                                    if (numOf3Cards > 0) {
                                        iconLabel.Text = "x3";
                                        numOf3Cards--;
                                        valueSet = true;
                                    }
                                    break;
                                case 2:
                                    if (numOfBombs > 0) {
                                        iconLabel.Text = "BOOM!";
                                        numOfBombs--;
                                        valueSet = true;
                                    }
                                    break;
                                case 3:
                                    if (numOf1Cards > 0) {
                                        iconLabel.Text = "x1";
                                        numOf1Cards--;
                                        valueSet = true;
                                    }
                                    break;
                            }
                        }
                        // then hide the card's value by changing text color to match background color.
                        iconLabel.ForeColor = iconLabel.BackColor;
                    }
                }
            }
            calculateCardBombIndicators();
        }

        private void label_Click(object sender, EventArgs e) {
            // The timer is only on after two non-matching 
            // icons have been shown to the player, 
            // so ignore any clicks if the timer is running
            //if (timer1.Enabled == true)
                //return;

            Label clickedLabel = sender as Label;
            Console.WriteLine(clickedLabel.Name + " clicked.");

            // If player touched a bomb and lost, ignore all clicks.
            if (gameOver) {
                Console.WriteLine(clickedLabel.Name + " click ignored bc game over.");
                return;
            }

            if (clickedLabel != null) {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                clickedLabel.ForeColor = Color.Black;
                calculateScore(clickedLabel.Text);

                // Check to see if the player won
                CheckForWinner();

                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                // timer1.Start();
            }
        }

        /// <summary>
        /// This timer is started when the player clicks 
        /// two icons that don't match,
        /// so it counts three quarters of a second 
        /// and then turns itself off and hides both icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void timer1_Tick(object sender, EventArgs e) {
            /*// Stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked 
            // so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;*/
        }
        

       // checks if user found all x2 & x3 cards. If they did, they cleared the level.
        private void CheckForWinner() {
            Console.WriteLine($"2cards: {numOf2CardsLeft}. 3cards: {numOf3CardsLeft}");
            if (numOf2CardsLeft == 0 && numOf3CardsLeft == 0) {
                gameOver = true;
                var result = MessageBox.Show("You found all the x2 and x3 cards! Click OK to go next level.", "Congratulations", MessageBoxButtons.OK);
                if(result == DialogResult.OK) {
                    AssignIconsToSquares();
                    gameOver = false;
                }
                //Close();
            }
        }

        // changes score based on card revealed's label text, also changes the score label.
        // for x1, x2, x3 if the score is 0 then it's changed to 1 then multiplied.
        public void calculateScore(string value) {
            Console.WriteLine("Score before new calc: " + score);
            if (value.Equals("x2")) {
                if(score == 0) {
                    score = 1;
                }
                score *= 2;
                numOf2CardsLeft--;
                Console.WriteLine("x2. Score is now: " + score);
            } 
            else if (value.Equals("x3")) {
                if (score == 0) {
                    score = 1;
                }
                score *= 3;
                numOf3CardsLeft--;
                Console.WriteLine("x3. Score is now: " + score);
            }
            // if you hit the bomb you get a game over.
            else if (value.Equals("BOOM!")) {
                soundBoom.Play();
                score *= 0;
                Console.WriteLine("x0. Score is now: " + score);
                gameOver = true;
                var result = MessageBox.Show("Game over! Click ok to restart the level.", "BOOM!", MessageBoxButtons.OK);
                if (result == DialogResult.OK) {
                    AssignIconsToSquares();
                    gameOver = false;
                }
            }
            else {
                if (score == 0) {
                    score = 1;
                }
                Console.WriteLine($"x1, no change. Score {score}");
            }
            scoreLabel.Text = "Score: " + score.ToString();
        }

        void calculateCardBombIndicators() {
            calculateRows();
            calculateColumns();
        }


        int valueOfCard(String card) {
            if (card.Equals("x2")) {
                return 2;
            }
            else if (card.Equals("x3")) {
                return 3;
            }
            // if you hit the bomb you get a game over.
            else if (card.Equals("BOOM!")) {
                return 0;
            }
            else {
                return 1;
            }
        }
        
        // just brute force counts each row, label by label since I couldn't figure out a clever way
        // to loop through the labels. 
        private void calculateRows() {
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            int sum4 = 0;
            int sum5 = 0;
            int numBombs1 = 0;
            int numBombs2 = 0;
            int numBombs3 = 0;
            int numBombs4 = 0;
            int numBombs5 = 0;
            if (valueOfCard(label1.Text) > 0) {
                sum1 += valueOfCard(label1.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label2.Text) > 0) {
                sum1 += valueOfCard(label2.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label3.Text) > 0) {
                sum1 += valueOfCard(label3.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label4.Text) > 0) {
                sum1 += valueOfCard(label4.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label5.Text) > 0) {
                sum1 += valueOfCard(label5.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label6.Text) > 0) {
                sum2 += valueOfCard(label6.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label7.Text) > 0) {
                sum2 += valueOfCard(label7.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label8.Text) > 0) {
                sum2 += valueOfCard(label8.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label9.Text) > 0) {
                sum2 += valueOfCard(label9.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label10.Text) > 0) {
                sum2 += valueOfCard(label10.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label11.Text) > 0) {
                sum3 += valueOfCard(label11.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label12.Text) > 0) {
                sum3 += valueOfCard(label12.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label13.Text) > 0) {
                sum3 += valueOfCard(label13.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label14.Text) > 0) {
                sum3 += valueOfCard(label14.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label15.Text) > 0) {
                sum3 += valueOfCard(label15.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label16.Text) > 0) {
                sum4 += valueOfCard(label16.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label18.Text) > 0) {
                sum4 += valueOfCard(label18.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label19.Text) > 0) {
                sum4 += valueOfCard(label19.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label20.Text) > 0) {
                sum4 += valueOfCard(label20.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label17.Text) > 0) {
                sum4 += valueOfCard(label17.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label21.Text) > 0) {
                sum5 += valueOfCard(label21.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label22.Text) > 0) {
                sum5 += valueOfCard(label22.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label23.Text) > 0) {
                sum5 += valueOfCard(label23.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label24.Text) > 0) {
                sum5 += valueOfCard(label24.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label25.Text) > 0) {
                sum5 += valueOfCard(label25.Text);
            }
            else {
                numBombs5++;
            }
            label1_1.Text = sum1 + "\n" + numBombs1;
            label1_2.Text = sum2 + "\n" + numBombs2;
            label1_3.Text = sum3 + "\n" + numBombs3;
            label1_4.Text = sum4 + "\n" + numBombs4;
            label1_5.Text = sum5 + "\n" + numBombs5;

        }
        // also brute force counting
        private void calculateColumns() {
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            int sum4 = 0;
            int sum5 = 0;
            int numBombs1 = 0;
            int numBombs2 = 0;
            int numBombs3 = 0;
            int numBombs4 = 0;
            int numBombs5 = 0;
            if (valueOfCard(label1.Text) > 0) {
                sum1 += valueOfCard(label1.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label2.Text) > 0) {
                sum2 += valueOfCard(label2.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label3.Text) > 0) {
                sum3 += valueOfCard(label3.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label4.Text) > 0) {
                sum4 += valueOfCard(label4.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label5.Text) > 0) {
                sum5 += valueOfCard(label5.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label6.Text) > 0) {
                sum1 += valueOfCard(label6.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label7.Text) > 0) {
                sum2 += valueOfCard(label7.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label8.Text) > 0) {
                sum3 += valueOfCard(label8.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label9.Text) > 0) {
                sum4 += valueOfCard(label9.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label10.Text) > 0) {
                sum5 += valueOfCard(label10.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label11.Text) > 0) {
                sum1 += valueOfCard(label11.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label12.Text) > 0) {
                sum2 += valueOfCard(label12.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label13.Text) > 0) {
                sum3 += valueOfCard(label13.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label14.Text) > 0) {
                sum4 += valueOfCard(label14.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label15.Text) > 0) {
                sum5 += valueOfCard(label15.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label16.Text) > 0) {
                sum1 += valueOfCard(label16.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label17.Text) > 0) {
                sum2 += valueOfCard(label17.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label18.Text) > 0) {
                sum3 += valueOfCard(label18.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label19.Text) > 0) {
                sum4 += valueOfCard(label19.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label20.Text) > 0) {
                sum5 += valueOfCard(label20.Text);
            }
            else {
                numBombs5++;
            }
            if (valueOfCard(label21.Text) > 0) {
                sum1 += valueOfCard(label21.Text);
            }
            else {
                numBombs1++;
            }
            if (valueOfCard(label22.Text) > 0) {
                sum2 += valueOfCard(label22.Text);
            }
            else {
                numBombs2++;
            }
            if (valueOfCard(label23.Text) > 0) {
                sum3 += valueOfCard(label23.Text);
            }
            else {
                numBombs3++;
            }
            if (valueOfCard(label24.Text) > 0) {
                sum4 += valueOfCard(label24.Text);
            }
            else {
                numBombs4++;
            }
            if (valueOfCard(label25.Text) > 0) {
                sum5 += valueOfCard(label25.Text);
            }
            else {
                numBombs5++;
            }
            label2_1.Text = sum1 + "\n" + numBombs1;
            label2_2.Text = sum2 + "\n" + numBombs2;
            label2_3.Text = sum3 + "\n" + numBombs3;
            label2_4.Text = sum4 + "\n" + numBombs4;
            label2_5.Text = sum5 + "\n" + numBombs5;
        }
    }
}
