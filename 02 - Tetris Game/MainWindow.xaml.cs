using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02___Tetris_Game
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//the array containing the tile images, in the order of their id
		private readonly ImageSource[] tileImages =
		{
			new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
		};

		//the array containing the block images, in the order of their id
		private readonly ImageSource[] blockImages =
		{
			new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
			new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
		};

		//2 dimensional array of image controls (there is 1 image control for every cell in the game grid)
		private readonly Image[,] imageControls;
		private readonly int maxDelay = 1000;
		private readonly int minDelay = 75;
		private readonly int delayDecrease = 25;

		private GameState gameState = new GameState();

		//the constructor
		public MainWindow()
		{
			InitializeComponent();
			imageControls = SetUpGameCanvas(gameState.GameGrid);
		}

		private Image[,] SetUpGameCanvas(GameGrid grid) //setting up the image controls grid correctly on the canvas (one image for every cell in the game grid)
		{
			Image[,] imageControls = new Image[grid.Rows, grid.Columns]; //the image grid has 22 rows and 10 columns, just like the game grid
			int cellSize = 25; //25 pixels for each visible cell

			for (int r = 0; r < grid.Rows; r++)
			{
				for (int c = 0; c < grid.Columns; c++)
				{
					Image imageControl = new Image
					{
						Width = cellSize,
						Height = cellSize
					};

					Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); //the -2 is to push up the top 2 hidden rows, so they will not be inside the canvas
					Canvas.SetLeft(imageControl, c * cellSize);
					GameCanvas.Children.Add(imageControl); //making the image a child of the canvas
					imageControls[r, c] = imageControl; //adding the image to the array
				}
			}

			return imageControls;
		}

		private void DrawGrid(GameGrid grid) //drawing the game grid
		{
			for (int r = 0; r < grid.Rows; r++)
			{
				for (int c = 0; c < grid.Columns; c++)
				{
					int id = grid[r, c]; //getting the stored id for each position
					imageControls[r, c].Opacity = 1; //resetting the opacity of the current block
					imageControls[r, c].Source = tileImages[id]; //setting the source of the image for this position using the id
				}
			}
		}

		private void DrawBlock(Block block) //looping throught the tile positions and update the image sources
		{
			foreach (Position p in block.TilePositions())
			{
				imageControls[p.Row, p.Column].Opacity = 1;
				imageControls[p.Row, p.Column].Source = tileImages[block.Id];
			}
		}

		private void DrawNextBlock(BlockQueue blockQueue) //the method previews the next block (on the right side of the canvas)
		{
			Block next = blockQueue.NextBlock;
			NextImage.Source = blockImages[next.Id];
		}

		private void DrawHeldBlock(Block heldBlock)
		{
			if (heldBlock == null)
			{
				HoldImage.Source = blockImages[0];
			}
			else
			{
				HoldImage.Source = blockImages[heldBlock.Id];
			}
		}

		private void DrawGhostBlock(Block block) //it helps you see where a block would fall
		{
			int dropDistance = gameState.BlockDropDistance();

			foreach (Position p in block.TilePositions())
			{
				imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
				imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
			}
		}

		private void Draw(GameState gameState) //it draws both the grid and the current block
		{
			DrawGrid(gameState.GameGrid);
			DrawGhostBlock(gameState.CurrentBlock);
			DrawBlock(gameState.CurrentBlock);
			DrawNextBlock(gameState.BlockQueue);
			DrawHeldBlock(gameState.HeldBlock);
			ScoreText.Text = $"Score: {gameState.Score}";
		}

		private async Task GameLoop() 
		{
			Draw(gameState);

			while (!gameState.GameOver) //it adds a game loop for the blocks, while the game is not over. Each block stays in position for the delay value
			{
				int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
				await Task.Delay(delay);
				gameState.MoveBlockDown();
				Draw(gameState);
			}

			GameOverMenu.Visibility = Visibility.Visible;
			FinalScoreText.Text = $"Score: {gameState.Score}";
		}


		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (gameState.GameOver)
			{
				return;
			}

			switch (e.Key)
			{
				case Key.Left:
					gameState.MoveBlockLeft();
					break;
				case Key.Right:
					gameState.MoveBlockRight();
					break;
				case Key.Down:
					gameState.MoveBlockDown();
					break;
				case Key.Up:
					gameState.RotateBlockCW();
					break;
				case Key.Z:
					gameState.RotateBlockCCW();
					break;
				case Key.C:
					gameState.HoldBlock();
					break;
				case Key.Space:
					gameState.DropBlock();
					break;
				default:
					return; //the default case ensures that we only redraw if the player presses a key that actually dows something 
			}

			Draw(gameState);
		}

		private async void GameCanvas_Loaded(object sender, RoutedEventArgs e) //starting the game loop when the canvas has loaded
		{
			await GameLoop();
		}

		private async void PlayAgain_Click(object sender, RoutedEventArgs e)
		{
			gameState = new GameState();
			GameOverMenu.Visibility = Visibility.Hidden;
			await GameLoop();
		}
	}
}