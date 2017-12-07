using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TriviaMaze
{
	public partial class Main : Form
	{
		private Graphics g;
		private SQLiteConnection dbConn;
		private SQLiteCommand dbCmd;
		private SQLiteDataReader dbDataReader;
		private triviaRoom[][] myMaze;
		private PictureBox[][] myBoxes;
		private Player thePlayer;
		private Pen closedPen = new Pen(Color.Red, 10);
		private Pen openPen = new Pen(Color.Green, 10);
		private Pen correctPen = new Pen(Color.RoyalBlue, 10);
		private static QuestionFactory qFac = new QuestionFactory();
		private Question cur = new Question(-1);
		private string direction = "";


		public Main()
		{
			thePlayer = new Player();
			InitializeComponent();
			createMaze();
			setUpBoxes();
			drawMaze();
			
		}

#region Create Maze
		private void createMaze()
		{
			myMaze = new triviaRoom[5][];
			for (int i = 0; i < 5; i++)
			{
				myMaze[i] = new triviaRoom[5];
			}
			for (int row = 0; row < 5; row++)
			{
				for (int col = 0; col < 5; col++)
				{
					triviaRoom temp = new triviaRoom();
					if (row == 0)
					{
						temp.North = false;
					}
					else if (row == 4)
					{
						temp.South = false;
					}
					if (col == 0)
					{
						temp.West = false;
					}
					else if (col == 4)
					{
						temp.East = false;
					}
					temp.Row = row;
					temp.Column = col;
					myMaze[row][col] = temp;
				}
			}

			/*
			 * Setting up Questions for the squares, this is the logic to get shared questions at each of the borders.
			 * North and West will grab previous questions, South and East will generate new questions
			 */ 

			for (int row = 0; row < 5; row++)
			{
				for (int col = 0; col < 5; col++)
				{
					if(row == 0)
					{
						if(col == 0)//Top left corner
						{
							myMaze[row][col].QEast = qFac.GetQuestion();
							myMaze[row][col].QSouth = qFac.GetQuestion();
						}
						else if(col == 4)//top right corner
						{
							myMaze[row][col].QWest = myMaze[row][col-1].QEast;
							myMaze[row][col].QSouth = qFac.GetQuestion();
						}
						else//top row
						{
							myMaze[row][col].QEast = qFac.GetQuestion();
							myMaze[row][col].QWest = myMaze[row][col - 1].QEast;
							myMaze[row][col].QSouth = qFac.GetQuestion();
						}
					}
					else if(row == 4)
					{
						if (col == 0)//Bottom left corner
						{
							myMaze[row][col].QEast = qFac.GetQuestion();
							myMaze[row][col].QNorth = myMaze[row-1][col].QSouth;
						}
						else if (col == 4)//Bottom right corner
						{
							myMaze[row][col].QWest = myMaze[row][col - 1].QEast;
							myMaze[row][col].QNorth = myMaze[row - 1][col].QSouth;
						}
						else//Bottom row
						{
							myMaze[row][col].QEast = qFac.GetQuestion();
							myMaze[row][col].QWest = myMaze[row][col - 1].QEast;
							myMaze[row][col].QNorth = myMaze[row - 1][col].QSouth;
						}
					}
					else if(col == 0)//Left side
					{
						myMaze[row][col].QEast = qFac.GetQuestion();
						myMaze[row][col].QNorth = myMaze[row - 1][col].QSouth;
						myMaze[row][col].QSouth = qFac.GetQuestion();
					}
					else if(col == 4)//Right side
					{
						myMaze[row][col].QNorth = myMaze[row - 1][col].QSouth;
						myMaze[row][col].QSouth = qFac.GetQuestion();
						myMaze[row][col].QWest = myMaze[row][col - 1].QEast;
					}
					else//Somewhere in the middle then
					{
						myMaze[row][col].QNorth = myMaze[row - 1][col].QSouth;
						myMaze[row][col].QSouth = qFac.GetQuestion();
						myMaze[row][col].QEast = qFac.GetQuestion();
						myMaze[row][col].QWest = myMaze[row][col - 1].QEast;
					}


				}

			}

		}



		private void setUpBoxes()
		{
			myBoxes = new PictureBox[5][];
			for (int i = 0; i < 5; i++)
			{
				myBoxes[i] = new PictureBox[5];
			}

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					String boxName = "box" + i + j;
					myBoxes[i][j] = ((PictureBox)this.panel1.Controls[boxName]);
				}
			}
		}
#endregion


#region Drawing
		private void drawMaze()
		{
			foreach (PictureBox[] p in myBoxes)
			{
				foreach (PictureBox b in p)
				{
					b.BackColor = Color.RoyalBlue;
				}
			}
			panel1.BackColor = Color.Black;
		}

		private void drawBorders()
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					drawWallNorth(i, j, myMaze[i][j].North);
					drawWallSouth(i, j, myMaze[i][j].South);
					drawWallEast(i, j, myMaze[i][j].East);
					drawWallWest(i, j, myMaze[i][j].West);
					
				}
			}
		}

		private void drawWallNorth(int i, int j, Boolean open)
		{
			g = myBoxes[i][j].CreateGraphics();
			if(myMaze[i][j].QNorth != null && myMaze[i][j].QNorth.Correct)
			{
				g.DrawLine(correctPen, 0, 0, 160, 0);
			}
			else if (open)
			{
				g.DrawLine(openPen, 0, 0, 160, 0);
			}
			else
			{
				g.DrawLine(closedPen, 0, 0, 160, 0);
			}
			
		}
		private void drawWallSouth(int i, int j, Boolean open)
		{
			g = myBoxes[i][j].CreateGraphics();
			if(myMaze[i][j].QSouth != null && myMaze[i][j].QSouth.Correct)
			{
				g.DrawLine(correctPen, 0, 100, 160, 100);
			}
			else if(open)
			{
				g.DrawLine(openPen, 0, 100, 160, 100);
			}
			else
			{
				g.DrawLine(closedPen, 0, 100, 160, 100);
			}
		}
		private void drawWallEast(int i, int j, Boolean open)
		{
			g = myBoxes[i][j].CreateGraphics();
			if (myMaze[i][j].QEast != null && myMaze[i][j].QEast.Correct)
			{
				g.DrawLine(correctPen, 160, 0, 160, 100);
			}
			else if (open)
			{
				g.DrawLine(openPen, 160, 0, 160, 100);
			}
			else
			{
				g.DrawLine(closedPen, 160, 0, 160, 100);
			}
			
		}
		private void drawWallWest(int i, int j, Boolean open)
		{
			g = myBoxes[i][j].CreateGraphics();
			if (myMaze[i][j].QWest != null && myMaze[i][j].QWest.Correct)
			{
				g.DrawLine(correctPen, 0, 0, 0, 100);
			}
			else if (open)
			{
				g.DrawLine(openPen, 0, 0, 0, 100);
			}
			else
			{
				g.DrawLine(closedPen, 0, 0, 0, 100);
			}
			
		}
		private void drawWalls(int i, int j)
		{
			drawWallNorth(i, j, myMaze[i][j].North);
			drawWallSouth(i, j, myMaze[i][j].South);
			drawWallEast(i, j, myMaze[i][j].East);
			drawWallWest(i, j, myMaze[i][j].West);
		}

		#endregion

		private void updatePlayer(int v1, int v2)
		{
			Pen coverPen = new Pen(Color.RoyalBlue, 10);
			g = myBoxes[thePlayer.Row][thePlayer.Column].CreateGraphics();
			g.DrawEllipse(coverPen, 50, 25, 50, 50);
			drawWalls(thePlayer.Row, thePlayer.Column);


			thePlayer.Row = v1;
			thePlayer.Column = v2;
			Pen playerPen = new Pen(Color.Gold, 10);

			g = myBoxes[thePlayer.Row][thePlayer.Column].CreateGraphics();
			g.DrawEllipse(playerPen, 50, 25, 50, 50);

			drawWalls(v1, v2);
			setValidMoves();
			btnSubmit.Enabled = false;
			checkForWin();
		}

		private void checkForWin()
		{
			if(thePlayer.Row == 4 && thePlayer.Column == 4)
			{
				disableButtons();
				btnStart.Enabled = false;
				txtAlert.Text = "Winner!";

				MessageBoxButtons btns = MessageBoxButtons.YesNo;
				DialogResult result = MessageBox.Show("Play again?", "Winner!" ,btns);
				if(result == DialogResult.Yes)
				{
					startNewGame();
				}

			}
		}

		private void startNewGame()
		{
			btnReset_Click(this, new EventArgs());
			btnStart_Click(this, new EventArgs());
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

#region Movement Buttons
		private void btnUp_Click(object sender, EventArgs e)
		{
			movePlayerUp();
		}
		private void btnDown_Click(object sender, EventArgs e)
		{
			movePlayerDown();
		}
		private void btnRight_Click(object sender, EventArgs e)
		{
			movePlayerRight();
		}
		private void btnLeft_Click(object sender, EventArgs e)
		{
			movePlayerLeft();
		}
		private void disableButtons()
		{
			btnUp.Enabled = false;
			btnDown.Enabled = false;
			btnLeft.Enabled = false;
			btnRight.Enabled = false;
		}


		//logic is: if the question has not been attempted, move the player to the wall, ask the question, and set the question as attempted
		//if they get the question right, move the player to the appropriate room and set the question as correct, and color the wall blue
		//otherwise, set the question as incorrect, color the wall red, and reset the player
		// if the question has been attempted and was correct, just move the player
		private void movePlayerUp()
		{
			if(!myMaze[thePlayer.Row][thePlayer.Column].QNorth.Attempted)
			{
				myMaze[thePlayer.Row][thePlayer.Column].QNorth.Attempted = true;
				cur = myMaze[thePlayer.Row][thePlayer.Column].QNorth;
				direction = "up";
				setQuestionField();
				disableButtons();
			}
			else
			{
				if(myMaze[thePlayer.Row][thePlayer.Column].QNorth.Correct)
				{
					updatePlayer(thePlayer.Row - 1, thePlayer.Column);
				}
				else
				{
					myMaze[thePlayer.Row][thePlayer.Column].North = false;
					updatePlayer(thePlayer.Row, thePlayer.Column);
				}
			}
		}

		private void movePlayerDown()
		{
			if (!myMaze[thePlayer.Row][thePlayer.Column].QSouth.Attempted)
			{
				myMaze[thePlayer.Row][thePlayer.Column].QSouth.Attempted = true;
				cur = myMaze[thePlayer.Row][thePlayer.Column].QSouth;
				direction = "down";
				setQuestionField();
				disableButtons();
			}
			else
			{
				if (myMaze[thePlayer.Row][thePlayer.Column].QSouth.Correct)
				{
					updatePlayer(thePlayer.Row + 1, thePlayer.Column);
				}
				else
				{
					myMaze[thePlayer.Row][thePlayer.Column].South = false;
					updatePlayer(thePlayer.Row, thePlayer.Column);
				}
			}
		}
		private void movePlayerRight()
		{
			if (!myMaze[thePlayer.Row][thePlayer.Column].QEast.Attempted)
			{
				myMaze[thePlayer.Row][thePlayer.Column].QEast.Attempted = true;
				cur = myMaze[thePlayer.Row][thePlayer.Column].QEast;
				direction = "right";
				setQuestionField();
				disableButtons();
			}
			else
			{
				if (myMaze[thePlayer.Row][thePlayer.Column].QEast.Correct)
				{
					updatePlayer(thePlayer.Row, thePlayer.Column + 1);
				}
				else
				{
					myMaze[thePlayer.Row][thePlayer.Column].East = false;
					updatePlayer(thePlayer.Row, thePlayer.Column);
				}
			}
		}
		private void movePlayerLeft()
		{
			if (!myMaze[thePlayer.Row][thePlayer.Column].QWest.Attempted)
			{
				myMaze[thePlayer.Row][thePlayer.Column].QWest.Attempted = true;
				cur = myMaze[thePlayer.Row][thePlayer.Column].QWest;
				direction = "left";
				setQuestionField();
				disableButtons();
			}
			else
			{
				if (myMaze[thePlayer.Row][thePlayer.Column].QWest.Correct)
				{
					updatePlayer(thePlayer.Row, thePlayer.Column - 1);
				}
				else
				{
					myMaze[thePlayer.Row][thePlayer.Column].West = false;
					updatePlayer(thePlayer.Row, thePlayer.Column);
				}
			}
		}



		#endregion

		

		private void setQuestionField()
		{
			txtQuestion.Text = cur.TheQuestion;
			Random gen = new Random();
			int temp = gen.Next();
			if (temp%4 == 0)
			{
				btnA.Text = cur.Answer;
				btnB.Text = cur.Opt1;
				btnC.Text = cur.Opt2;
				btnD.Text = cur.Opt3;
			}
			else if (temp % 4 == 1)
			{
				btnB.Text = cur.Answer;
				btnA.Text = cur.Opt1;
				btnD.Text = cur.Opt2;
				btnC.Text = cur.Opt3;
			}
			else if (temp % 4 == 2)
			{
				btnC.Text = cur.Answer;
				btnB.Text = cur.Opt1;
				btnA.Text = cur.Opt2;
				btnD.Text = cur.Opt3;
			}
			else if (temp % 4 == 3)
			{
				btnD.Text = cur.Answer;
				btnC.Text = cur.Opt1;
				btnB.Text = cur.Opt2;
				btnA.Text = cur.Opt3;
			}
			btnSubmit.Enabled = true;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			btnSubmit.Enabled = false;
			drawBorders();
			updatePlayer(0,0);
			setValidMoves();
			btnStart.Enabled = false;
		}

		private void setValidMoves()
		{
			triviaRoom curRoom = myMaze[thePlayer.Row][thePlayer.Column];

			if (curRoom.North)
				btnUp.Enabled = true;
			else
				btnUp.Enabled = false;

			if (curRoom.South)
				btnDown.Enabled = true;
			else
				btnDown.Enabled = false;

			if (curRoom.East)
				btnRight.Enabled = true;
			else
				btnRight.Enabled = false;

			if (curRoom.West)
				btnLeft.Enabled = true;
			else
				btnLeft.Enabled = false;
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			btnStart.Enabled = true;
			btnSubmit.Enabled = false;
			qFac.reset();
			foreach(PictureBox[] pa in myBoxes)
			{
				foreach(PictureBox p in pa)
				{
					p.Image = null;
				}
			}
			createMaze();
			thePlayer.Column = 0;
			thePlayer.Row = 0;
		}

		private void txtAlert_TextChanged(object sender, EventArgs e)
		{

		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			String txt = "";
			bool answer = false;
			if(btnA.Checked)
			{
				txt = btnA.Text;
				answer = true;
			}
			else if(btnB.Checked)
			{
				txt = btnB.Text;
				answer = true;
			}
			else if(btnC.Checked)
			{
				txt = btnC.Text;
				answer = true;
			}
			else if(btnD.Checked)
			{
				txt = btnD.Text;
				answer = true;
			}


			if(answer)
			{
				if(txt == cur.Answer)
				{
					cur.Correct = true;
					txtAlert.Text = "Correct!";
				}
				else
				{
					txtAlert.Text = "Incorrect! Answer was: \n" + cur.Answer;
				}
			}

			if(direction == "up")
			{
				movePlayerUp();
			}
			else if(direction == "down")
			{
				movePlayerDown();
			}
			else if(direction == "left")
			{
				movePlayerLeft();
			}
			else if(direction == "right")
			{
				movePlayerRight();
			}


		}

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Wasn't implemented in time, sorry");
		}

		private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Wasn't implemented in time, sorry");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("A Trivia Maze by Alex McCanna.\nThe goal is to make it to the bottom right corner.\nMove squares by correctly" +
				" answering trivia questions.");
		}

		private void directionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("To begin the game, press \"Start Game\"\n" +
				"To restart, press \"Reset Board\" and \"Start Game\" again\n" +
				"Move using the buttons on the screen");
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{

			MessageBoxButtons btns = MessageBoxButtons.YesNoCancel;
			DialogResult result = MessageBox.Show("Are you sure?", "", btns);

			if (result == DialogResult.Cancel)
			{
				e.Cancel = true;
			}

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
