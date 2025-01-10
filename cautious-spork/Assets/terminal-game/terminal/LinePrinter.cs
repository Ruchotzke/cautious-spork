using System.Collections.Generic;
using System.Text;
using terminal_game.tasks;
using UnityEngine;

namespace terminal_game.terminal
{
    /// <summary>
    /// A helper used to manage the state of a screen and handle wrapping/line printing.
    /// </summary>
    public class LinePrinter
    {

        /// <summary>
        /// The number of chars on a line.
        /// </summary>
        private int _lineLength;
        
        /// <summary>
        /// The number of lines to be displayed on the screen.
        /// </summary>
        private int _numLines;
        
        /// <summary>
        /// The print task.
        /// </summary>
        private TerminalPrintTask _p;

        /// <summary>
        /// All lines stored for this printer.
        /// </summary>
        private List<string> _lines;

        /// <summary>
        /// The current line being edited.
        /// </summary>
        private string _currLine;

        /// <summary>
        /// The position of the cursor on the current line.
        /// </summary>
        private int CurrPos;
        
        /// <summary>
        /// Construct a new lineprinter.
        /// </summary>
        /// <param name="lineLength">The number of chars to be displayed on a line</param>
        /// <param name="maxLines">The maximum number of lines on the screen.</param>
        /// <param name="printer">The print task used to update the screen.</param>
        public LinePrinter(int lineLength, int maxLines, TerminalPrintTask printer)
        {
            _lineLength = lineLength;
            _numLines = maxLines;
            _p = printer;
            _lines = new List<string>();
            _currLine = "";
            CurrPos = 0;
        }

        /// <summary>
        /// Append the given text to the prompt.
        /// </summary>
        /// <param name="str"></param>
        public void Append(string str)
        {
            foreach (var c in str)
            {
                switch (c)
                {
                    case '\r':
                        /* Handle a carriage return */
                        CarriageReturn();
                        break;
                    case '\n':
                        /* Handle a line feed */
                        LineFeed();
                        break;
                    case '\b':
                        /* Handle a backspace */
                        if (CurrPos == 0) break;
                        
                        if (CurrPos == _currLine.Length)
                        {
                            /* Just remove the last */
                            _currLine = _currLine.Substring(0, _currLine.Length - 1);
                        }
                        else
                        {
                            /* Merge two substrings */
                            _currLine = _currLine.Substring(0, CurrPos - 1) + _currLine.Substring(CurrPos);
                        }

                        CurrPos -= 1;
                        _p.MvAddStr(CurrPos, _numLines-1, " " + _currLine.Substring(CurrPos));
                        break;
                        
                    default:
                        if (_currLine.Length > 0)
                        {
                            /* Use a stringbuilder to edit a specific index */
                            StringBuilder sb = new StringBuilder(_currLine);
                            if (CurrPos != _currLine.Length)
                            {
                                sb[CurrPos++] = c;
                            }
                            else
                            {
                                sb.Append(c);
                                CurrPos++;
                            }

                            _currLine = sb.ToString();
                        }
                        else
                        {
                            /* Basic initial char */
                            _currLine = "" + c;
                            CurrPos++;
                        }
                        
                        _p.MvAddChar(CurrPos - 1, _numLines-1, c);
                        break;
                }
            }
        }

        /// <summary>
        /// Handle a carriage return.
        /// </summary>
        private void CarriageReturn()
        {
            CurrPos = 0;
        }

        /// <summary>
        /// Handle a line feed.
        /// </summary>
        private void LineFeed()
        {
            /* Append this line to the feed (up to this char) */
            if (_currLine.Length > 0)
            {
                _lines.Add(_currLine.Substring(0, CurrPos));
            
                /* Generate a fresh line (or continue the previous) */
                _currLine = CurrPos != _currLine.Length ? _currLine.Substring(CurrPos) : "";
            }
            else
            {
                _lines.Add("");
                _currLine = "";
            }
            
            /* Reset the cursor */
            CurrPos = 0;
            
            /* Feed upwards */
            _p.ShiftUp();
        }
    }
}