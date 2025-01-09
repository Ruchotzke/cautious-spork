using System;
using terminal_game.terminal;
using UnityEngine;

namespace terminal_game.computer
{
    /// <summary>
    /// The in-game component used to spawn a computer.
    /// </summary>
    public class ComputerComponent : MonoBehaviour
    {

        private Computer _computer;
        
        private void Awake()
        {
            _computer = new Computer();
        }

        private void Start()
        {
            /* Connect the terminal component */
            _computer.PrintTask.Screen = GameObject.FindObjectOfType<TerminalComponent>();
            _computer.PrintTask.MvAddChar(0, 0, 'a');
        }

        private void Update()
        {
            _computer.Tick(Time.deltaTime);
        }
    }
}