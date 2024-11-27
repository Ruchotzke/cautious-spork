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
        }
        
        public Queue<Command> Commands;
        public TerminalComponent Screen;

        /// <summary>
        /// The extra work done that doesn't lead to a char being printed.
        /// </summary>
        private float interFrameWork = 0.0f;

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
                const float speed = 40f;
                interFrameWork += seconds;
                int max = Mathf.FloorToInt(interFrameWork * speed); /* N chars per second */
                interFrameWork -= max * 1.0f / speed;   /* Maintain leftover time for next frame */
                int curr = 0;
                while (Commands.Count > 0 && curr < max)
                {
                    /* Get current char */
                    Command c = Commands.Dequeue();
                
                    /* Print */
                    Screen.Grid[c.Col, c.Row] = c.Character;
                
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
    }
}