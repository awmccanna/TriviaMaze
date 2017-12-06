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
			for(int i = 0; i < 5; i++)
			{
				myMaze[i] = new triviaRoom[5];
			}
			for(int row = 0; row < 5; row++)
			{
				for(int col = 0; col < 5; col++)
				{
					triviaRoom temp = new triviaRoom();
					if(row == 0)
					{
						temp.North = false;
					}
					else if(row == 4)
					{
						temp.South = false;
					}

					if(col == 0)
					{
						temp.West = false;
					}
					else if(col == 4)
					{
						temp.East = false;
					}
					myMaze[row][col] = temp;
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
			
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			drawBorders();
			updatePlayer(0,0);
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
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
	}




}
