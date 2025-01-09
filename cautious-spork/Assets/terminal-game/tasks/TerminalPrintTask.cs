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
        
        private Vector2Int _cursor = Vector2Int.zero;

        /// <summary>
        /// The extra work done that doesn't lead to a char being printed.
        /// </summary>
        private float _interFrameWork = 0.0f;

        public TerminalPrintTask()
        {
            Commands = new Queue<Command>();
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
            if (Commands.Count > 0)
            {
                const float speed = 240f;
                _interFrameWork += seconds;
                int max = Mathf.FloorToInt(_interFrameWork * speed); /* N chars per second */
                _interFrameWork -= max * 1.0f / speed;   /* Maintain leftover time for next frame */
                int curr = 0;
                while (Commands.Count > 0 && curr < max)
                {
                    /* Get current char */
                    Command c = Commands.Dequeue();

                    if (c.ShiftUp)
                    {
                        /* Shift up */
                        for (int row = 0; row < Screen.Height - 1; row++)
                        {
                            for (int col = 0; col < Screen.Width; col++)
                            {
                                Screen.Grid[col, row] = Screen.Grid[col, row+1];
                            }
                        }
                        
                        /* Clear bottom row */
                        for (int col = 0; col < Screen.Width; col++)
                        {
                            Screen.Grid[col, Screen.Height - 1] = ' ';
                        }
                    }
                    else
                    {
                        /* Print */
                        Screen.Grid[c.Col, c.Row] = c.Character;
                    }
                
                    
                
                    curr += 1;
                }
                Screen.UpdateScreen();
            }
            
        }

        public void Clear()
        {
            Commands.Clear();
            Screen.ClearScreen();
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
            PushCommand(col % Screen.Width, row % Screen.Height, ch);
            StepCursor();
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
        /// Clear the screen (immediately clears the screen and empties the print queue)
        /// </summary>
        public void ClearScreen()
        {
            Clear();
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