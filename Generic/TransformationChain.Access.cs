// CogMan/Bender/Generic/TransformationChain.Access.cs © 2006 Andreas Wickman
using System.Collections.Generic;
using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// This source module deinfes chain entry access methods.
    /// </summary>
    public abstract partial class TransformationChain<Operand>
        where Operand: struct
    {
        #region Symbolic indexing

        /// <summary>
        /// Returns the index of the first entry with the given name.
        /// The search is case sensitive.
        /// </summary>
        /// <param name="arg_entryName"></param>
        /// <returns></returns>
        protected int GetFirstIndexOf(string arg_entryName)
        {
            // Scan for match
            int count = Length;
            for (int i = 0; i < count; i++)
            {
                // If match, return the index
                if (m_entries[i].m_name.Equals(arg_entryName)) return i;
            }

            // Not found, return -1
            return -1;
        }

        /// <summary>
        /// Returns the index of the last entry with the given name.
        /// The search is case sensitive.
        /// </summary>
        /// <param name="arg_entryName"></param>
        /// <returns></returns>
        protected int GetLastIndexOf(string arg_entryName)
        {
            // Scan for match
            int count = Length;
            for (int i = count - 1; i >= 0; i--)
            {
                // If match, return the index
                if (m_entries[i].m_name.Equals(arg_entryName)) return i;
            }

            // Not found, return -1
            return -1;
        }

        #endregion

        #region Add

        /// <summary>
        /// Appends the given entry to the chain.
        /// </summary>
        /// <param name="arg_entry"></param>
        protected void Add(Entry arg_entry)
        {
            // Add to collection
            m_entries.Add(arg_entry);

            // The chain transformation is now invalidated
            InvalidateTransformation();
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the given entry at the given index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <param name="arg_entry"></param>
        protected void Insert(int arg_index, Entry arg_entry)
        {
            // Insert
            m_entries.Insert(arg_index, arg_entry);

            // The chain transformation is now invalidated
            InvalidateTransformation();
        }

        #endregion

        #region Replace

        /// <summary>
        /// Replaces the entry at the given index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <param name="arg_entry"></param>
        protected void Replace(int arg_index, Entry arg_entry)
        {
            // Get current entry
            Entry oldEntry = m_entries[arg_index];

            // Replace
            m_entries[arg_index] = arg_entry;

            // Dispose old entry
            oldEntry.Dispose();

            // The chain transformation is now invalidated
            InvalidateTransformation();
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the entry at the given index.
        /// </summary>
        /// <param name="arg_index"></param>
        protected void Remove(int arg_index)
        {
            // Get entry
            Entry entry = m_entries[arg_index];

            // Remove from list
            m_entries.RemoveAt(arg_index);

            // Dispose entry
            entry.Dispose();

            // The chain transformation is now invalidated
            InvalidateTransformation();
        }

        /// <summary>
        /// Removes the last entry from the chain.
        /// </summary>
        protected void Remove()
        {
            // Validate
            if (Length == 0) return;

            // Get entry
            int index = Length - 1;
            Entry entry = m_entries[index];

            // Remove from list
            m_entries.RemoveAt(index);

            // Dispose entry
            entry.Dispose();

            // The chain transformation is changed (not invalidated)
            InvalidateTransformationDependers();
        }

        #endregion
    }
}
