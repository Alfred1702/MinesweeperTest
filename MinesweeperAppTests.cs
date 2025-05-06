using Xunit;
using MinesweeperApp;
using System.Runtime.CompilerServices;

namespace MinesweeperAppTests
{
    public class MinesweeperGameTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        //This is to test on the size and mines given are correct
        public void TestGameInitialization(int size)
        {
            //Arange
            int expectedMines = (size * size * 35) / 100;


            if (size <= 0)
            {
                // Act & Assert: for invalid size, expect an exception or handle it
                Assert.Throws<ArgumentException>(() => new MinesweeperGame(size, expectedMines));
            }
            else
            {
                //Act
                var game = new MinesweeperGame(size, expectedMines);

                // Assert
                Assert.Equal(size, game.Size);
                Assert.Equal(expectedMines, game.NumMines);
            }
            
        }

        [Theory]
        [InlineData(-1)] // Invalid size, should throw exception
        [InlineData(0)] // Invalid size, should throw exception
        [InlineData(2)] // Valid size
        //This is to test that size are correct with mines or without mines
        public void TestMinePlacement(int size)
        {

            // Arrange
            int expectedMines = (size * size * 35) / 100;

            if (size <= 0)
            {
                // Act & Assert: for invalid size, expect an exception or handle it
                Assert.Throws<ArgumentException>(() => new MinesweeperGame(size, expectedMines));
            }
            else
            {
                // Act: Create the game with the valid grid size and expected number of mines
                var game = new MinesweeperGame(size, expectedMines);

                // For a 1x1 grid, we expect no mines to be placed
                if (size == 1)
                {
                    bool foundMine = false;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (game.Grid[i, j].IsMine)
                            {
                                foundMine = true;
                                break;
                            }
                        }
                    }
                    // Assert: Ensure no mines are placed for a 1x1 grid
                    Assert.False(foundMine, "A mine should not be placed on a 1x1 grid.");
                }
                else
                {
                    // For larger grids, ensure that mines are placed if expectedMines > 0
                    bool foundMine = false;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (game.Grid[i, j].IsMine)
                            {
                                foundMine = true;
                                break;
                            }
                        }
                    }

                    // Assert: No mines should be placed if expectedMines is 0
                    if (expectedMines > 0)
                    {
                        Assert.True(foundMine, "Expected mines to be placed but none were found.");
                    }
                    else
                    {
                        Assert.False(foundMine, "Expected no mines to be placed, but some were found.");
                    }
                }

            }
        }

        [Theory]
        [InlineData(-1)] // Invalid size, should throw exception
        [InlineData(0)] // Invalid size, should throw exception
        [InlineData(2)] // Valid size
        //This is to test GameOver after mine revealed
        public void TestGameOverAfterMineRevealed(int size)
        {
            // Arrange
            int expectedMines = (size * size * 35) / 100;

            if (size <= 0)
            {
                // Act & Assert: for invalid size, expect an exception or handle it
                Assert.Throws<ArgumentException>(() => new MinesweeperGame(size, expectedMines));
            }
            else
            {
                // Act: Create the game with the valid grid size and expected number of mines
                var game = new MinesweeperGame(size, expectedMines);

                // Act: Reveal any square and check if it's a mine
                bool mineRevealed = false;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (game.Grid[i, j].IsMine)
                        {
                            game.Reveal(i, j); // Reveal the mine
                            mineRevealed = true;
                            break;
                        }
                    }
                    if (mineRevealed) break;
                }

                // Assert: The game should be over after revealing a mine
                Assert.True(game.GameOver, "The game should end when a mine is revealed.");
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void TestGameWon(int size)
        {
            // Arrange
            int expectedMines = (size * size * 35) / 100;
            var game = new MinesweeperGame(size, expectedMines);

            // Act: Reveal all non-mine cells
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!game.Grid[i, j].IsMine)
                    {
                        game.Reveal(i, j); // Reveal a non-mine cell
                    }
                }
            }

            // Assert: Game should be won if all non-mine cells are revealed
            Assert.True(game.GameWon, "The game should be won when all non-mine cells are revealed.");
        }

        [Fact]
        //To test on same cell that has been revealed 
        public void TestRevealingAlreadyRevealedCell()
        {
            // Arrange
            var game = new MinesweeperGame(5, 5);
            var row = 0;
            var col = 0;

            // Act: Reveal the same cell twice
            game.Reveal(row, col);
            bool firstReveal = game.Grid[row, col].IsRevealed;

            game.Reveal(row, col);
            bool secondReveal = game.Grid[row, col].IsRevealed;

            // Assert: The cell should remain revealed after the second reveal attempt (no change in state)
            Assert.True(firstReveal, "The cell should be revealed after the first reveal.");
            Assert.True(secondReveal, "The cell should remain revealed after the second reveal.");
        }
    }
}