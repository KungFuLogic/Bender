// CogMan/Bender/Generic/TransformationStack.Bookmark.cs © 2006 Andreas Wickman
using IDisposable = System.IDisposable;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// This source module deinfes the bookmark class to be used
    /// in the transformation stack.
    /// </summary>
    public partial class TransformationStack<Operand>
        where Operand: struct
    {
        /// <summary>
        /// Represents some specific depth of the stack, that
        /// the stack is returned to when the bookmark is disposed.
        /// 
        /// I.e. the usage should be:
        /// 
        /// using (transformationStack.GetBookmark())
        /// {
        ///     // Do something involving more entries on the stack.
        /// }
        /// </summary>
        public class Bookmark: IDisposable
        {
            #region Object construction and destruction

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="arg_transformations"></param>
            internal Bookmark(TransformationStack<Operand> arg_transformations)
            {
                // Set members
                m_transformations = arg_transformations;
                m_entry = arg_transformations.TopEntry;     // null if empty stack
            }

            /// <summary>
            /// Recalls the bookmarked state of the stack and disposes this bookmark.
            /// </summary>
            public void Dispose()
            {
                Recall();
                m_entry = null;
                m_transformations = null;
            }

            #endregion

            #region State

            /// <summary>
            /// Holds the transformation stack of this bookmark.
            /// </summary>
            private TransformationStack<Operand> m_transformations;

            /// <summary>
            /// Holds the entry to reestablish as topmost.
            /// </summary>
            private Entry m_entry;

            #endregion

            #region Bookmark operation

            /// <summary>
            /// Recalls the bookmarked state of the stack.
            /// </summary>
            public void Recall()
            {
                while (true)
                {
                    // Get top entry
                    Entry topEntry = m_transformations.TopEntry;

                    // If null, the stack is empty
                    if (topEntry == null) return;

                    // If match we've reached our bookmark
                    if (topEntry == m_entry) return;

                    // Until then move next entry off the stack
                    m_transformations.RemoveTop();
                }
            }

            #endregion
        }
    }
}
