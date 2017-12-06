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
		public int Row { get { return row; } set { row = value; } }
		public int Column { get { return col; } set { col = value; } }

		public Player()
		{
			row = 0;
			col = 0;
		}


	}
}
