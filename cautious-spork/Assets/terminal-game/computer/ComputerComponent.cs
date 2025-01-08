using System;
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

        private void Update()
        {
            _computer.Tick(Time.deltaTime);
        }
    }
}