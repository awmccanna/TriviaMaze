using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class triviaRoom
	{
		private bool north, south, east, west;
		private int row, col;
		private Question qNorth, qSouth, qEast, qWest;


		public int Row { get => row; set => row = value; }
		public int Column { get => col; set => col = value; }
		public bool North { get => north; set => north = value; }
		public bool South { get => south; set => south = value; }
		public bool East { get => east; set => east = value; }
		public bool West { get => west; set => west = value; }
		internal Question QNorth { get => qNorth; set => qNorth = value; }
		internal Question QSouth { get => qSouth; set => qSouth = value; }
		internal Question QEast { get => qEast; set => qEast = value; }
		internal Question QWest { get => qWest; set => qWest = value; }

		public triviaRoom()
		{
			North = true;
			South = true;
			East = true;
			West = true;
			Row = 0;
			Column = 0;
			
		}
		







	}
}
