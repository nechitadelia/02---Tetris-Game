using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class IBlock : Block
	{
		//fields
		private readonly Position[][] tiles = 
		{
			[ new(1,0), new (1,1), new (1,2), new(1,3) ],
			[ new(0,2), new (1,2), new (2,2), new(3,2) ],
			[ new(2,0), new (2,1), new (2,2), new(2,3) ],
			[ new(0,1), new (1,1), new (2,1), new(3,1) ]
		};

		//properties
		public override int Id => 1; //the id of the I block
		protected override Position StartOffset => new Position(-1, 3); //the starting point of the block in the grid
		protected override Position[][] Tiles => tiles; //the array with the 4 position states
	}
}
