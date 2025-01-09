using System;
using terminal_game.computer;
using terminal_game.tasks;
using UnityEngine;

namespace terminal_game.managers
{
    /// <summary>
    /// The input manager for the game.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the input manageer.
        /// </summary>
        public static InputManager Instance = null;

        TerminalInputHandlerTask terminalInputHandler;
        
        private void Awake()
        {
            /* Handle singleton */
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;

            terminalInputHandler = new TerminalInputHandlerTask();
        }

        private void Start()
        {
            Computer.Computers[0].InputHandlerTask = terminalInputHandler;
        }


        private void Update()
        {
            /* Get the chars typed */
            foreach (char c in Input.inputString)
            {
                terminalInputHandler.inputQueue.Enqueue(c);
            }
        }
    }
}