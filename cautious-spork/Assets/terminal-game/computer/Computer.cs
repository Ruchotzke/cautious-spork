using System.Collections.Generic;
using terminal_game.tasks;
using terminal_game.terminal;

namespace terminal_game.computer
{
    /// <summary>
    /// The computer bringing together many different components.
    /// </summary>
    public class Computer
    {
        public static List<Computer> Computers = new List<Computer>();

        public OperatingSystem OS;

        /// <summary>
        /// Generate a new computer object.
        /// </summary>
        public Computer()
        {
            /* Save this computer to a master list */
            Computers.Add(this);
            
            /* Generate an OS */
            OS = new OperatingSystem();
        }

        /// <summary>
        /// Tick this computer simulation forward,
        /// </summary>
        /// <param name="delta"></param>
        public void Tick(float delta)
        {
            OS.Tick(delta);
        }
        
    }
}