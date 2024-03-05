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

				for (int i = 0; i < 2; i++)
				{
					currentBlock.Move(1, 0);

					if (!BlockFits())
					{
						currentBlock.Move(-1, 0);
					}
				}
			}
		}
		public GameGrid GameGrid { get; }
		public BlockQueue BlockQueue { get; }
		public bool GameOver { get; private set; }
		public int Score { get; private set; }
		public Block HeldBlock { get; private set; }
		public bool CanHold {  get; private set; }

		//constructor
		public GameState()
		{
			GameGrid = new GameGrid(22, 10); //initialising the game grid with 22 rows and 10 columns
			BlockQueue = new BlockQueue(); //initialising the blockqueue
			CurrentBlock = BlockQueue.GetAndUpdate(); //using the blockqueue to return a random block, unique from the last one
			CanHold = true;
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

		public void HoldBlock()
		{
			if (!CanHold)
			{
				return;
			}
			
			if (HeldBlock == null)
			{
				HeldBlock = CurrentBlock;
				CurrentBlock = BlockQueue.GetAndUpdate();
			}
			else
			{
				Block tmp = CurrentBlock;
				CurrentBlock = HeldBlock;
				HeldBlock = tmp;
			}

			CanHold = false;
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

			Score += GameGrid.ClearFullRows();

			if (IsGameOver())
			{
				GameOver = true;
			}
			else
			{
				CurrentBlock = BlockQueue.GetAndUpdate();
				CanHold = true;
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

		private int TileDropDistance(Position p) //it calculates how many rows the current block can be moved down
		{
			int drop = 0;

			while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
			{
				drop++;
			}

			return drop;
		}

		public int BlockDropDistance() //it invokes the TileDropDistance() for every tile in the current block, returning the minimum distance a block can be moved
		{
			int drop = GameGrid.Rows;

			foreach (Position p in CurrentBlock.TilePositions())
			{
				drop = System.Math.Min(drop, TileDropDistance(p));
			}

			return drop;
		}

		public void DropBlock() //it moves the block down as many rows as possible and places it in the grid
		{
			CurrentBlock.Move(BlockDropDistance(), 0);
			PlaceBlock();
		}

	}
}
