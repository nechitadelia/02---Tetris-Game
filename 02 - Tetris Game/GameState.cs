using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class GameState
	{
		//fields
		private Block currentBlock;

		//properties
		public Block CurrentBlock
		{
			get
			{
				return currentBlock;
			}
			private set
			{
				currentBlock = value;
				currentBlock.Reset();
			}
		}
		public GameGrid GameGrid { get; }
		public BlockQueue BlockQueue { get; }
		public bool GameOver { get; private set; }

		//constructor
		public GameState()
		{
			GameGrid = new GameGrid(22, 10); //initialising the game grid with 22 rows and 10 columns
			BlockQueue = new BlockQueue(); //initialising the blockqueue
			CurrentBlock = BlockQueue.GetAndUpdate(); //using the blockqueue to return a random block, unique from the last one
		}

		//methods
		private bool BlockFits() //it checks if the current block is in a legal position or not (if it is inside the grid and if it doesn't overlap another tile)
		{
			foreach(Position p in CurrentBlock.TilePositions())
			{
				if (!GameGrid.IsEmpty(p.Row, p.Column))
				{
					return false;
				}
			}

			return true;
		}

		public void RotateBlockCW() //it rotates a block only if it's possible from where it is
		{
			CurrentBlock.RotateCW();

			if (!BlockFits())
			{
				CurrentBlock.RotateCCW();
			}
		}
		public void RotateBlockCCW() //it rotates a block only if it's possible from where it is
		{
			CurrentBlock.RotateCCW();

			if (!BlockFits())
			{
				CurrentBlock.RotateCW();
			}
		}

		public void MoveBlockLeft() //it moves a block to the left only if it's possible
		{
			CurrentBlock.Move(0, -1);

			if (!BlockFits())
			{
				CurrentBlock.Move(0, 1);
			}
		}

		public void MoveBlockRight() //it moves a block to the right only if it's possible
		{
			CurrentBlock.Move(0, 1);

			if (!BlockFits())
			{
				CurrentBlock.Move(0, -1);
			}
		}

		private bool IsGameOver() //if one of the 2 top rows are not empty, then the game is over
		{
			return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
		}

		private void PlaceBlock() //it is called when the current block can't be moved down
		{
			foreach (Position p in CurrentBlock.TilePositions())
			{
				GameGrid[p.Row, p.Column] = CurrentBlock.Id; // this calls the indexer to set the value of the block, so the block cells can be coloured
			}

			GameGrid.ClearFullRows();

			if (IsGameOver())
			{
				GameOver = true;
			}
			else
			{
				CurrentBlock = BlockQueue.GetAndUpdate();
			}
		}

		public void MoveBlockDown() //it moves down a block only if it's possible
		{
			CurrentBlock.Move(1, 0);

			if (!BlockFits())
			{
				CurrentBlock.Move(-1, 0);
				PlaceBlock();
			}
		}

	}
}
