using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class Player
	{
		private int row, col;


		public Player()
		{
			Row = 0;
			Column = 0;
		}

		public int Row { get => row; set => row = value; }
		public int Column { get => col; set => col = value; }
	}
}
