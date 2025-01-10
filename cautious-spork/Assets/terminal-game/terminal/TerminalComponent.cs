using System;
using TMPro;
using UnityEngine;

namespace terminal_game.terminal
{
    /// <summary>
    /// The in-world component for a terminal screen.
    /// </summary>
    public class TerminalComponent : MonoBehaviour
    {
        /// <summary>
        /// The screen width in chars.
        /// </summary>
        public int Width = 80;
        
        /// <summary>
        /// The screen height in chars.
        /// </summary>
        public int Height = 24;
        
        
        private TextMeshProUGUI textMesh;

        private void Awake()
        {
            /* Get Components */
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Update the tmpro to reflect the grid
        /// </summary>
        public void UpdateScreen(char[,] grid)
        {
            string total = "";
            for (int row = 0; row < Height; row++)
            {
                string line = "";
                for (int col = 0; col < Width; col++)
                {
                    line += grid[col, row];
                }

                total += line + "<br>";
            }
            
            textMesh.text = total;
        }
    }
}