using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class LBlock : Block
	{
		//fields
		private readonly Position[][] tiles =
		{
			[ new(0,2), new (1,0), new (1,1), new(1,2) ],
			[ new(0,1), new (1,1), new (2,1), new(2,2) ],
			[ new(1,0), new (1,1), new (1,2), new(2,0) ],
			[ new(0,0), new (0,1), new (1,1), new(2,1) ]
		};

		//properties
		public override int Id => 3; //the id of the L block
		protected override Position StartOffset => new Position(0, 3); //the starting point of the block in the grid
		protected override Position[][] Tiles => tiles; //the array with the 4 position states
	}
}
