using System.Collections;
using System.Collections.Generic;
using terminal_game.computer;
using UnityEngine;

namespace terminal_game.tasks
{
    /// <summary>
    /// The task used to handle input to the terminal screen.
    /// </summary>
    public class TerminalInputHandlerTask
    {
        public Queue<char> inputQueue;
        private OperatingSystem _os;

        public TerminalInputHandlerTask(OperatingSystem hook)
        {
            inputQueue = new Queue<char>();
            _os = hook;
        }
        
        public void Work(float seconds)
        {
            while (inputQueue.Count > 0)
            {
                char next = inputQueue.Dequeue();
                
                /* Unity considers \r a newline, so convvert those to linefeeds, as there's not a return key */
                if (next == '\r') next = '\n';
                
                _os.PushInput(next);

            }
        }
    }
}