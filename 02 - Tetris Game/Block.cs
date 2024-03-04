using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public abstract class Block
	{
		//fields
		private int rotationState; // the current rotation state
		private Position offset; //the current offset

		//properties
		protected abstract Position[][] Tiles { get; } //a 2 dimensional position array which contains the 4 position states of the blocks
		protected abstract Position StartOffset { get; } //this is the position where a block starts in a grid
		public abstract int Id { get; } //this distinguises the blocks

		//constructor
		public Block()
		{
			offset = new Position(StartOffset.Row, StartOffset.Column);
		}

		//methods
		public IEnumerable<Position> TilePositions() //the method loops over the tile positions in the current rotation state and adds the row offset and column offset, returning the grid positions occupied by the block
		{
			foreach (Position p in Tiles[rotationState])
			{
				yield return new Position(p.Row + offset.Row, p.Column + offset.Column); //"yield + return" keywords return a list of IEnumerable elements
			}
		}

		public void RotateCW() //it rotates the block 90 degrees clockwise
		{
			rotationState = (rotationState + 1) % Tiles.Length;
		}

		public void RotateCCW() //it rotates the block 90 degrees counterclockwise
		{
			if(rotationState == 0)
			{
				rotationState = Tiles.Length - 1;
			}
			else
			{
				rotationState--;
			}
		}

		public void Move(int rows, int columns) //it moves the block by a given number of rows and columns
		{
			offset.Row += rows;
			offset.Column += columns;
		}

		public void Reset() //it resets the rotation and position of a block
		{
			rotationState = 0;
			offset.Row = StartOffset.Row;
			offset.Column = StartOffset.Column;
		}
	}
}
