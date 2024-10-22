using System;

namespace DFRLib.Exceptions
{
    /// <summary>
    /// Exception thrown from ByteDiff-related functions.
    /// </summary>
    public class ByteDiffException : Exception
    {
        public ByteDiffException()
        {
        }

        public ByteDiffException(string message) : base(message)
        {
        }
    }
}
