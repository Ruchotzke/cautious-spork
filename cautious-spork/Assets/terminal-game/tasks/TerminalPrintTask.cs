using System.Collections.Generic;
using terminal_game.terminal;
using UnityEngine;

namespace terminal_game.tasks
{
    /// <summary>
    /// The terminal control task.
    /// </summary>
    public class TerminalPrintTask
    {
        /// <summary>
        /// A command used to print a character to the screen.
        /// </summary>
        public struct Command
        {
            public int Row, Col;
            public char Character;
            public bool ShiftUp;
        }
        
        public Queue<Command> Commands;
        public TerminalComponent Screen;
        
        /// <summary>
        /// The character grid of the screen.
        /// </summary>
        public char[,] Grid;

        /// <summary>
        /// The overlay grid on top of the screen.
        /// </summary>
        public char[,] OverlayGrid;
        
        private Vector2Int _cursor = Vector2Int.zero;

        public float CursorOnTime = 0.5f;
        public float CursorOffTime = 0.5f;
        private bool _isCursorOn = false;
        private float _cursorTimer = 0.0f;
        private Vector2Int _prevCursor;

        /// <summary>
        /// The extra work done that doesn't lead to a char being printed.
        /// </summary>
        private float _interFrameWork = 0.0f;

        public TerminalPrintTask(TerminalComponent screen)
        {
            Commands = new Queue<Command>();

            Grid = new char[screen.Width, screen.Height];
            OverlayGrid = new char[screen.Width, screen.Height];
            Screen = screen;
            
            Clear();
        }

        /// <summary>
        /// Push a given command to be printed.
        /// </summary>
        /// <param name="c"></param>
        public void PushCommand(Command c)
        {
            Commands.Enqueue(c);
        }

        /// <summary>
        /// Push a given command to be printed.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="character"></param>
        public void PushCommand(int col, int row, char character)
        {
            PushCommand(new Command { Row = row, Col = col, Character = character });
        }

        /// <summary>
        /// Work towards completion for a given number of seconds.
        /// </summary>
        /// <param name="seconds"></param>
        public void Work(float seconds)
        {
            /* Handle commands */
            bool updated = false;
            if (Commands.Count > 0)
            {
                const float speed = 240f;
                _interFrameWork += seconds;
                int max = Mathf.FloorToInt(_interFrameWork * speed); /* N chars per second */
                _interFrameWork -= max * 1.0f / speed;   /* Maintain leftover time for next frame */
                int curr = 0;
                while (Commands.Count > 0 && curr < max)
                {
                    /* Get current command */
                    Command c = Commands.Dequeue();

                    if (c.ShiftUp)
                    {
                        /* Shift up */
                        for (int row = 0; row < Screen.Height - 1; row++)
                        {
                            for (int col = 0; col < Screen.Width; col++)
                            {
                                Grid[col, row] = Grid[col, row+1];
                            }
                        }
                        
                        /* Clear bottom row */
                        for (int col = 0; col < Screen.Width; col++)
                        {
                            Grid[col, Screen.Height - 1] = ' ';
                        }
                    }
                    else
                    {
                        /* Print */
                        Grid[c.Col, c.Row] = c.Character;
                    }

                    updated = true;
                    curr += 1;
                }
                Screen.UpdateScreen(Grid, OverlayGrid);
            }
            
            /* If we made any changes, we should force the cursor on */
            if (updated)
            {
                _cursorTimer = 0.0f;
                _isCursorOn = false;
                OverlayGrid[_prevCursor.x, _prevCursor.y] = '\0';
            }
            
            /* Update the cursor */
            _cursorTimer -= seconds;
            if (_cursorTimer <= 0.0f)
            {
                _isCursorOn = !_isCursorOn;
                _cursorTimer = _isCursorOn ? CursorOnTime : CursorOffTime;
                if (_isCursorOn)
                {
                    _prevCursor = _cursor;
                    OverlayGrid[_prevCursor.x, _prevCursor.y] = '\u2588';
                }
                else
                {
                    OverlayGrid[_prevCursor.x, _prevCursor.y] = '\0';
                }
                Screen.UpdateScreen(Grid, OverlayGrid);
            }
        }

        public void Clear()
        {
            Commands.Clear();
            for (int row = 0; row < Screen.Height; row++)
            {
                for (int col = 0; col < Screen.Width; col++)
                {
                    Grid[col, row] = ' ';
                    OverlayGrid[col, row] = '\0';
                }
            }
            Screen.UpdateScreen(Grid, OverlayGrid);
        }
        
        
        
        /// <summary>
        /// Step the cursor forward, wrapping when needed.
        /// </summary>
        private void StepCursor()
        {
            _cursor.x += 1;
            if (_cursor.x == Screen.Width)
            {
                _cursor.x = 0;
                _cursor.y += 1;
                if (_cursor.y == Screen.Height)
                {
                    _cursor.y = 0;
                }
            }
        }

        /// <summary>
        /// Move the cursor and add a char.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="ch"></param>
        public void MvAddChar(int col, int row, char ch)
        {
            _cursor.x = col;
            _cursor.y = row;
            PushCommand(col % Screen.Width, row % Screen.Height, ch);
            StepCursor();
        }

        /// <summary>
        /// Move the cursor to the given position.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void Mv(int col, int row)
        {
            _cursor.x = col;
            _cursor.y = row;
        }

        /// <summary>
        /// Move the cursor and add a string.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="str"></param>
        public void MvAddStr(int col, int row, string str)
        {
            /* First add the first char */
            MvAddChar(col, row, str[0]);
            
            /* And then the rest */
            foreach (var c in str.Substring(1))
            {
                AddChar(c);
            }
        }

        /// <summary>
        /// Add a character at the current cursor position.
        /// </summary>
        /// <param name="ch"></param>
        public void AddChar(char ch)
        {
            PushCommand(_cursor.x % Screen.Width, _cursor.y % Screen.Height, ch);
            StepCursor();
        }

        /// <summary>
        /// Shift all lines upward, removing the top line.
        /// </summary>
        public void ShiftUp()
        {
            PushCommand(new TerminalPrintTask.Command(){ShiftUp = true});
        }
    }
}