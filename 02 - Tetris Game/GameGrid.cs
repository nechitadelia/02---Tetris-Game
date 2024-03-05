using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___Tetris_Game
{
	public class GameGrid
	{
		//fields
		private readonly int[,] grid;

		//properties
		public int Rows { get; }
		public int Columns { get; }

		//indexer
		public int this[int r, int c]
		{
			get { return grid[r, c]; }
			set { grid[r, c] = value; }
		}

		//constructor
		public GameGrid(int rows, int columns)
		{
			Rows = rows; 
			Columns = columns;
			grid = new int[rows, columns];
		}

		//methods
		public bool IsInside(int r, int c) //it checks if a given cell is inside the grid or not
		{
			return r >= 0 && r < Rows && c >= 0 && c < Columns;
		}

		public bool IsEmpty(int r, int c) //it checks if a given cell is empty or not
		{
			return IsInside(r, c) && grid[r, c] == 0;
		}

		public bool IsRowFull(int r) //it checks if an entire row is full
		{
			for (int c = 0; c < Columns; c++)
			{
				if (grid[r, c] == 0)
				{
					return false;
				}
			}

			return true;
		}

		public bool IsRowEmpty(int r) //it checks if an entire row is empty
		{
			for (int c = 0; c < Columns; c++)
			{
				if (grid[r, c] != 0)
				{
					return false;
				}
			}

			return true;
		}

		private void ClearRow(int r) //it clears a row
		{
			for (int c = 0; c < Columns; c++)
			{
				grid[r, c] = 0;
			}
		}

		private void MoveRowDown(int r, int numRows) //it moves a row down by a certain number of rows
		{
			for (int c = 0; c < Columns; c++)
			{
				grid[r + numRows, c] = grid[r, c];
				grid[r, c] = 0;
			}
		}

		public int ClearFullRows() //it clears a full row and moves it down
		{
			int cleared = 0;

			for (int r = Rows - 1; r >= 0; r--)
			{
				if (IsRowFull(r))
				{
					ClearRow(r);
					cleared++;
				}
				else if (cleared > 0)
				{
					MoveRowDown(r, cleared);
				}
			}

			return cleared; //this helps calculating the score of the player
		}
	}
}
