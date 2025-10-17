using System;

namespace Week3ArraysSorting
{
    /// <summary>
    /// Board Game implementation for Assignment 2 Part A
    /// Demonstrates multi-dimensional arrays with interactive gameplay
    /// 
    /// Learning Focus: 
    /// - Multi-dimensional array manipulation (char[,])
    /// - Console rendering and user input
    /// - Game state management and win detection
    /// 
    /// Choose ONE game to implement:
    /// - Tic-Tac-Toe (3x3 grid)
    /// - Connect Four (6x7 grid with gravity)
    /// - Or something else creative using a 2D array! (I need to be able to understand the rules from your instructions)
    /// </summary>
    public class BoardGame
    {
        // Connect Four constants and fields
        private const int ROWS = 6;
        private const int COLUMNS = 7;
        private char[,] board = new char[ROWS, COLUMNS];
        private char currentPlayer;
        private bool gameOver;
        private string winner;
        public BoardGame()
        {

            Console.WriteLine("=== CONNECT FOUR ===");
            // Connect Four Board using a 6x7 grid, tokens to be used my players and gravity mechanic

            currentPlayer = 'X'; // X always starts
            gameOver = false;
            winner = "";
        }

        /// <summary>
        /// Main game loop - handles the complete game session
        /// TODO: Implement the full game experience
        /// </summary>
        public void StartGame()
        {
            Console.Clear();
            Console.WriteLine("=== CONNECT FOUR ===");
            Console.WriteLine();

            // TODO: Display game instructions
            DisplayInstructions();

            // TODO: Implement main game loop
            bool playAgain = true;

            while (playAgain)
            {
                // TODO: Reset game state for new game
                InitializeNewGame();

                // TODO: Play one complete game
                PlayOneGame();

                // TODO: Ask if player wants to play again
                playAgain = AskPlayAgain();
            }

            Console.WriteLine("Thanks for playing!");
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }
        private void DisplayInstructions()
        {
            Console.WriteLine("CONNECT FOUR RULES:");
            Console.WriteLine(" - Two players take turns dropping tokens into columns (X and O).");
            Console.WriteLine(" - Enter a column number (0-6) when prompted.");
            Console.WriteLine(" - Tokens fall to the lowest available row in the chosen column.");
            Console.WriteLine(" - First player to get 4 in a row (horizontally, vertically, or diagonally) wins!");
            Console.WriteLine(" - If the board fills up with no winner, the game is a draw.");
            Console.WriteLine(" - To quit the game at any time, type 'q' when prompted for a column.");
            Console.WriteLine();
        }
        private void InitializeNewGame()
        {
            // Reset the board and game state
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLUMNS; c++)
                {
                    board[r, c] = ' '; // Empty space
                }
            }
            currentPlayer = 'X';
            gameOver = false;
            winner = "";
            
            Console.WriteLine("TODO: Initialize new game state");
        }
        private void PlayOneGame()
        {
            // Main loop for a single game session
            while (!gameOver)
            {
                RenderBoard();
                GetPlayerMove();
                CheckWinCondition();
                if (!gameOver)
                {
                    SwitchPlayer();
                }
            }
            
            RenderBoard();
            if (winner != "")
            {
                Console.WriteLine($"Player {winner} wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }
        }
        private void RenderBoard()
        {
            Console.Clear();
            Console.WriteLine("CONNECT FOUR BOARD:");
            Console.WriteLine();

            // Column labels
            Console.Write("  ");
            for (int c = 0; c < COLUMNS; c++)
            {
                Console.Write($" {c} ");
            }
            Console.WriteLine();

            // Board rows
            for (int r = 0; r < ROWS; r++)
            {
                Console.Write($"{r} |"); // Row label
                for (int c = 0; c < COLUMNS; c++)
                {
                    Console.Write($" {board[r, c]} |");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        private void GetPlayerMove()
        {
            Console.WriteLine("TODO: Get and validate player move");

            bool validMove = false;
            while (!validMove)
            {
                Console.Write($"Player {currentPlayer}, choose a column (0-{COLUMNS - 1}): ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int col))
                {
                    Console.WriteLine("Invalid input! Enter a number.");
                    continue;
                }

                if (col < 0 || col >= COLUMNS)
                {
                    Console.WriteLine("Column out of range!");
                    continue;
                }

                if (board[0, col] != ' ')
                {
                    Console.WriteLine("Column is full! Choose another.");
                    continue;
                }

                DropToken(col, currentPlayer);
                validMove = true;
            }
        }
        
        private void DropToken(int column, char player)
        {
            // Drop token to lowest available row
            for (int r = ROWS - 1; r >= 0; r--)
            {
                if (board[r, column] == ' ')
                {
                    board[r, column] = player;
                    break;
                }
            }
        }

        private void CheckWinCondition()
        {
            // Check horizontal
            for (int r = 0; r < ROWS; r++)
                for (int c = 0; c <= COLUMNS - 4; c++)
                    if (board[r, c] != ' ' &&
                        board[r, c] == board[r, c + 1] &&
                        board[r, c] == board[r, c + 2] &&
                        board[r, c] == board[r, c + 3])
                    {
                        gameOver = true;
                        winner = currentPlayer.ToString();
                        return;
                    }

            // Check vertical
            for (int c = 0; c < COLUMNS; c++)
                for (int r = 0; r <= ROWS - 4; r++)
                    if (board[r, c] != ' ' &&
                        board[r, c] == board[r + 1, c] &&
                        board[r, c] == board[r + 2, c] &&
                        board[r, c] == board[r + 3, c])
                    {
                        gameOver = true;
                        winner = currentPlayer.ToString();
                        return;
                    }

            // Check diagonal (top-left to bottom-right)
            for (int r = 0; r <= ROWS - 4; r++)
                for (int c = 0; c <= COLUMNS - 4; c++)
                    if (board[r, c] != ' ' &&
                        board[r, c] == board[r + 1, c + 1] &&
                        board[r, c] == board[r + 2, c + 2] &&
                        board[r, c] == board[r + 3, c + 3])
                    {
                        gameOver = true;
                        winner = currentPlayer.ToString();
                        return;
                    }

            // Check diagonal (bottom-left to top-right)
            for (int r = 3; r < ROWS; r++)
                for (int c = 0; c <= COLUMNS - 4; c++)
                    if (board[r, c] != ' ' &&
                        board[r, c] == board[r - 1, c + 1] &&
                        board[r, c] == board[r - 2, c + 2] &&
                        board[r, c] == board[r - 3, c + 3])
                    {
                        gameOver = true;
                        winner = currentPlayer.ToString();
                        return;
                    }

            // Check for draw
            bool full = true;
            for (int c = 0; c < COLUMNS; c++)
                if (board[0, c] == ' ')
                    full = false;

            if (full)
                gameOver = true; // draw, winner remains ""
        }
        private bool AskPlayAgain()
        {
            
            while (true)
            {
                Console.Write("Play again? (y/n): ");
                string input = (Console.ReadLine() ?? "").Trim().ToLower();
                if (input == "y" || input == "yes")
                    return true;
                else if (input == "n" || input == "no")
                    return false;
                else
                    Console.WriteLine("Invalid input, please enter y or n.");
            }
            
            // Placeholder - always return false for now
            // return false; // comment out to see if needed
        }
        private void SwitchPlayer()
        {         
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }
        
        // TODO: Add helper methods as needed
        // Examples:
        // - IsValidMove(int row, int col)
        // - IsBoardFull()
        // - CheckRow(int row, char player)
        // - CheckColumn(int col, char player)
        // - CheckDiagonals(char player)
        // - DropToken(int column, char player) // For Connect Four
    }
}