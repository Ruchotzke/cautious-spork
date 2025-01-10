using System.Collections.Generic;
using System.Text;
using terminal_game.tasks;

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
        private int _currPos;
        
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
            _currPos = 0;
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
                        if (_currPos == 0) break;
                        
                        if (_currPos == _currLine.Length)
                        {
                            /* Just remove the last */
                            _currLine = _currLine.Substring(0, _currLine.Length - 1);
                        }
                        else
                        {
                            /* Merge two substrings */
                            _currLine = _currLine.Substring(0, _currPos - 1) + _currLine.Substring(_currPos);
                        }

                        _currPos -= 1;
                        _p.MvAddStr(_currPos, _numLines-1, " " + _currLine.Substring(_currPos));
                        break;
                        
                    default:
                        if (_currLine.Length > 0)
                        {
                            /* Use a stringbuilder to edit a specific index */
                            StringBuilder sb = new StringBuilder(_currLine);
                            if (_currPos != _currLine.Length)
                            {
                                sb[_currPos++] = c;
                            }
                            else
                            {
                                sb.Append(c);
                                _currPos++;
                            }

                            _currLine = sb.ToString();
                        }
                        else
                        {
                            /* Basic initial char */
                            _currLine = "" + c;
                            _currPos++;
                        }
                        
                        _p.MvAddChar(_currPos - 1, _numLines-1, c);
                        break;
                }
            }
        }

        /// <summary>
        /// Handle a carriage return.
        /// </summary>
        private void CarriageReturn()
        {
            _currPos = 0;
        }

        /// <summary>
        /// Handle a line feed.
        /// </summary>
        private void LineFeed()
        {
            /* Append this line to the feed (up to this char) */
            if (_currLine.Length > 0)
            {
                _lines.Add(_currLine.Substring(0, _currPos));
            
                /* Generate a fresh line (or continue the previous) */
                _currLine = _currPos != _currLine.Length ? _currLine.Substring(_currPos) : "";
            }
            else
            {
                _lines.Add("");
                _currLine = "";
            }
            
            /* Reset the cursor */
            _currPos = 0;
            
            /* Feed upwards */
            _p.ShiftUp();
        }
    }
}