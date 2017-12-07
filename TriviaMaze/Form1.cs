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
		private static QuestionFactory qFac = new QuestionFactory();



		public Main()
		{
			thePlayer = new Player();
			InitializeComponent();
			createMaze();
			setUpBoxes();
			drawMaze();
		}


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

		private void drawMaze()
		{
			foreach (PictureBox[] p in myBoxes)
			{
				foreach (PictureBox b in p)
				{
					b.BackColor = Color.RoyalBlue;
					
				}
			}
			this.panel1.BackColor = Color.Black;
		}

		private void drawBorders()
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (!myMaze[i][j].North)
					{
						drawWallNorth(i, j, false);
					}
					if (!myMaze[i][j].South)
					{
						drawWallSouth(i, j, false);
					}
					if (!myMaze[i][j].East)
					{
						drawWallEast(i, j, false);
					}
					if (!myMaze[i][j].West)
					{
						drawWallWest(i, j, false);
					}
				}
			}
		}

		private void updatePlayer(int v1, int v2)
		{
			thePlayer.Row = v1;
			thePlayer.Column = v2;
			Pen playerPen = new Pen(Color.Gold, 10);
			g = myBoxes[thePlayer.Row][thePlayer.Column].CreateGraphics();
			g.DrawEllipse(playerPen, 50, 25, 50, 50);

			drawOpenRooms(v1, v2);
		}

		private void drawOpenRooms(int v1, int v2)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (myMaze[i][j].North)
					{
						drawWallNorth(i, j, true);
					}
					if (myMaze[i][j].South)
					{
						drawWallSouth(i, j, true);
					}
					if (myMaze[i][j].East)
					{
						drawWallEast(i, j, true);
					}
					if (myMaze[i][j].West)
					{
						drawWallWest(i, j, true);
					}
				}
			}
		}

		private void drawWallNorth(int i, int j, Boolean open)
		{
			g = myBoxes[i][j].CreateGraphics();
			if (open)
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
			if(open)
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
			if (open)
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
			if (open)
			{
				g.DrawLine(openPen, 0, 0, 0, 100);
			}
			else
			{
				g.DrawLine(closedPen, 0, 0, 0, 100);
			}
			
		}

		

		

		

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

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

		private void movePlayerUp()
		{
			throw new NotImplementedException();
		}
		private void movePlayerDown()
		{
			throw new NotImplementedException();
		}
		private void movePlayerRight()
		{
			throw new NotImplementedException();
		}
		private void movePlayerLeft()
		{
			throw new NotImplementedException();
		}




		private void btnStart_Click(object sender, EventArgs e)
		{
			drawBorders();
			updatePlayer(0,0);
			setValidMoves();
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
	}




}
