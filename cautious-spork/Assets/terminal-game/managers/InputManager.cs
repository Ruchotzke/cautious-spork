using System;
using System.Collections.Generic;
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
        
        /* Delegates */
        /// <summary>
        /// A delegate for handling character inputs.
        /// </summary>
        public delegate void HandleCharInput(char input);

        /// <summary>
        /// All char input handlers that need to receive char input from this input manager.
        /// </summary>
        public HandleCharInput CharInputHandlers;
        
        private void Awake()
        {
            /* Handle singleton */
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }


        private void Update()
        {
            /* Get the chars typed */
            if (CharInputHandlers != null)
            {
                foreach (char c in Input.inputString)
                {
                    CharInputHandlers(c);
                }
            }
        }
    }
}