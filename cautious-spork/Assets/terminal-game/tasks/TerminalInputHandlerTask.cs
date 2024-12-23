using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terminal_game.tasks
{
    /// <summary>
    /// The task used to handle input to the terminal screen.
    /// </summary>
    public class TerminalInputHandlerTask
    {
        public Queue<char> inputQueue;

        public TerminalInputHandlerTask()
        {
            inputQueue = new Queue<char>();
        }
        
        public void Work(float seconds)
        {
            while (inputQueue.Count > 0)
            {
                char next = inputQueue.Dequeue();
                if (next == '\n' || next == '\r')
                {
                    Debug.Log("return");
                }
                else if (next == '\b')
                {
                    Debug.Log("backspace");
                }
                else
                {
                    Debug.Log(next);
                }
                
            }
        }
    }
}