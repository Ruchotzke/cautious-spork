using terminal_game.managers;
using terminal_game.tasks;
using terminal_game.terminal;
using UnityEngine;

namespace terminal_game.computer
{
    /// <summary>
    /// The piece of the computer responsible for managing resources
    /// </summary>
    public class OperatingSystem
    {
        /* Tasks */
        /* Terminal */
        public TerminalPrintTask TerminalPrintTask;
        
        /* Input */
        public TerminalInputHandlerTask InputTask;

        public OperatingSystem()
        {
            /* Generate the default tasks */
            TerminalPrintTask = new TerminalPrintTask();
            InputTask = new TerminalInputHandlerTask();
        }

        /// <summary>
        /// Initialize the OS components.
        /// </summary>
        public void Initialize()
        {
            /* Bind the default tasks to their components */
            TerminalPrintTask.Screen = GameObject.FindObjectOfType<TerminalComponent>();
            InputManager.Instance.CharInputHandlers += (input => { InputTask.inputQueue.Enqueue(input); });
            
            TerminalPrintTask.MvAddChar(5,5,'a');
        }

        /// <summary>
        /// Tick the OS forward in time.
        /// </summary>
        /// <param name="seconds"></param>
        public void Tick(float seconds)
        {
            TerminalPrintTask.Work(seconds);
            InputTask.Work(seconds);
        }
    }
}