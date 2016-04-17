// CogMan/Bender/Generic/TransformationStack.cs © 2006 Andreas Wickman
using System.Collections.Generic;
using IDisposable = System.IDisposable;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// Keeps track of transformations during rendering recursion.
    /// </summary>
    public abstract partial class TransformationStack<Operand>: TransformationChain<Operand>
        where Operand: struct
    {
        /// <summary>
        /// Returns a bookmark for the current position.
        /// </summary>
        /// <returns></returns>
        public Bookmark GetBookmark()
        {
            return new Bookmark(this);
        }
    }
}
