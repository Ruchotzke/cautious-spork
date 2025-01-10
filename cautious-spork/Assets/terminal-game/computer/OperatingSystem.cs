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
        public LinePrinter LinePrinter;
        
        /* Input */
        public TerminalInputHandlerTask InputTask;

        public OperatingSystem()
        {
            /* Generate the default tasks */
            InputTask = new TerminalInputHandlerTask(this);
        }

        /// <summary>
        /// Initialize the OS components.
        /// </summary>
        public void Initialize()
        {
            /* Bind the default tasks to their components */
            TerminalPrintTask = new TerminalPrintTask(GameObject.FindObjectOfType<TerminalComponent>());
            InputManager.Instance.CharInputHandlers += (input => { InputTask.inputQueue.Enqueue(input); });
            
            /* Create a screen handler */
            LinePrinter = new LinePrinter(TerminalPrintTask.Screen.Width, TerminalPrintTask.Screen.Height,
                TerminalPrintTask);
            
            LinePrinter.Append("Hello World!");
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

        /// <summary>
        /// Push keyboard input to the OS.
        /// </summary>
        /// <param name="c"></param>
        public void PushInput(char c)
        {
            LinePrinter.Append("" + c);
        }
    }
}