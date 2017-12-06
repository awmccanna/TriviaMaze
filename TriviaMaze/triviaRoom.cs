using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class triviaRoom
	{
		private Boolean north, south, east, west;



		public Boolean North { get { return north; } set { north = value; } }
		public Boolean South { get { return south; } set { south = value; } }
		public Boolean East { get { return east; } set { east = value; } }
		public Boolean West { get { return west; } set { west = value; } }

		public triviaRoom()
		{
			north = true;
			south = true;
			east = true;
			west = true;
		}
		


	}
}
