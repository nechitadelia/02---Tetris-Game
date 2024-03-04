using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class OBlock : Block
	{
		//fields
		private readonly Position[][] tiles =
		{
			[ new(0,0), new(0,1), new(1,0), new(1,1)]
		};

		//properties
		public override int Id => 4; //the id of the O block
		protected override Position StartOffset => new Position(0,4); //the starting point of the block in the grid
		protected override Position[][] Tiles => tiles; //the array with the 4 position states
	}
}
