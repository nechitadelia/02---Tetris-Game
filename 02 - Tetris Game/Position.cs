using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class Position //the class represents the position of a cell in the grid
	{
		//properties
		public int Row { get; set; }
		public int Column { get; set; }

		public Position(int row, int column)
		{
			Row = row;
			Column = column;
		}
	}
}
