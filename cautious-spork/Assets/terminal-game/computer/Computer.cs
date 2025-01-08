using System.Collections.Generic;
using terminal_game.tasks;

namespace terminal_game.computer
{
    /// <summary>
    /// The computer bringing together many different components.
    /// </summary>
    public class Computer
    {
        public static List<Computer> Computers = new List<Computer>();
        
        public TerminalInputHandlerTask InputHandlerTask;
        public TerminalPrintTask PrintTask;

        /// <summary>
        /// Generate a new computer object.
        /// </summary>
        public Computer()
        {
            Computers.Add(this);
        }

        /// <summary>
        /// Tick this computer simulation forward,
        /// </summary>
        /// <param name="delta"></param>
        public void Tick(float delta)
        {
            InputHandlerTask?.Work(delta);
            PrintTask?.Work(delta);
        }
        
    }
}