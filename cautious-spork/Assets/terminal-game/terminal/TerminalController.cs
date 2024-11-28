using System;
using terminal_game.tasks;
using UnityEngine;

namespace terminal_game.terminal
{
    /// <summary>
    /// The controller for the terminal.
    /// </summary>
    public class TerminalController : MonoBehaviour
    {
        private TerminalPrintTask _printTask;
        private TerminalComponent _terminalComponent;
        
        private Vector2Int _cursor = Vector2Int.zero;

        private void Start()
        {
            /* Get references */
            _terminalComponent = FindObjectOfType<TerminalComponent>();
            _printTask = new TerminalPrintTask();
            _printTask.Screen = _terminalComponent;
            
            /* Write some chars! */
            ClearScreen();
            for (int i = 0; i < 500; i++)
            {
                AddChar('o');
            }
            ShiftUp();
            ShiftUp();
        }

        private void Update()
        {
            /* Tick the print task */
            _printTask.Work(Time.deltaTime);
        }

        /// <summary>
        /// Step the cursor forward, wrapping when needed.
        /// </summary>
        private void StepCursor()
        {
            _cursor.x += 1;
            if (_cursor.x == _terminalComponent.Width)
            {
                _cursor.x = 0;
                _cursor.y += 1;
                if (_cursor.y == _terminalComponent.Height)
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
            _printTask.PushCommand(col % _terminalComponent.Width, row % _terminalComponent.Height, ch);
            StepCursor();
        }

        /// <summary>
        /// Add a character at the current cursor position.
        /// </summary>
        /// <param name="ch"></param>
        public void AddChar(char ch)
        {
            _printTask.PushCommand(_cursor.x % _terminalComponent.Width, _cursor.y % _terminalComponent.Height, ch);
            StepCursor();
        }

        /// <summary>
        /// Clear the screen (immediately clears the screen and empties the print queue)
        /// </summary>
        public void ClearScreen()
        {
            _printTask.Clear();
        }

        /// <summary>
        /// Shift all lines upward, removing the top line.
        /// </summary>
        public void ShiftUp()
        {
            _printTask.PushCommand(new TerminalPrintTask.Command(){ShiftUp = true});
        }
    }
}