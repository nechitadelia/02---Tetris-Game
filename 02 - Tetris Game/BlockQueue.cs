using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class BlockQueue // the class is responsible with shuffling the blocks
	{
		//fields
		private readonly Block[] blocks =
		{
			new IBlock(),
			new JBlock(),
			new LBlock(),
			new OBlock(),
			new SBlock(),
			new TBlock(),
			new ZBlock()
		};

		private readonly Random random = new Random();

		//properties
		public Block NextBlock { get; private set; } // the next block in the queue

		//constructor
		public BlockQueue()
		{
			NextBlock = RandomBlock();
		}

		//methods
		private Block RandomBlock() //it returns a random block
		{
			return blocks[random.Next(blocks.Length)];
		}

		public Block GetAndUpdate() //it returns a new random block, different from the last one
		{
			Block block = NextBlock;

			do
			{
				NextBlock = RandomBlock();
			}
			while (block.Id == NextBlock.Id);

			return block;
		}
	}
}
