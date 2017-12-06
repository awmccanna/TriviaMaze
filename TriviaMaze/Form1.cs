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
		private SQLiteConnection dbConn;
		private SQLiteCommand dbCmd;
		private SQLiteDataReader dbDataReader;
		private triviaRoom[][] myMaze;
		public Main()
		{
			InitializeComponent();
			createMaze();
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
						temp.South = false;
					}


					myMaze[row][col] = temp;
				}
			}
		}

		private void drawMaze()
		{
			throw new NotImplementedException();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}




}
