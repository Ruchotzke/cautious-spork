using System.Collections.Generic;
using terminal_game.computer;
using UnityEngine;

namespace terminal_game.managers
{
    /// <summary>
    /// The general manager for the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the game manager.
        /// </summary>
        public static GameManager Instance = null;
        
        private void Awake()
        {
            /* Handle singleton */
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }
    }
}