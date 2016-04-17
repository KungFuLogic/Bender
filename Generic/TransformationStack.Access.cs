// CogMan/Bender/Generic/TransformationStack.Access.cs © 2006 Andreas Wickman
using System.Collections.Generic;
using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// This source module deinfes stack entry access methods.
    /// </summary>
    public partial class TransformationStack<Operand>
        where Operand: struct
    {
        #region Entry collection properties

        /// <summary>
        /// Returns the number of transformations on the stack.
        /// </summary>
        public int Count
        {
            get
            {
                return Length;
            }
        }

        /// <summary>
        /// Returns the top entry of the stack.
        /// Returns null if the stack is empty.
        /// </summary>
        protected Entry TopEntry
        {
            get
            {
                if (Length == 0) return null;
                return Entries[Length - 1];
            }
        }

        #endregion

        #region Stack entry access

        /// <summary>
        /// Returns the stack depth index of the entry with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        protected int GetStackIndex(string arg_name)
        {
            return Count - GetLastIndexOf(arg_name) - 1;
        }

        /// <summary>
        /// Returns the entry at the given depth index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <returns></returns>
        protected Entry GetStackEntry(int arg_index)
        {
            return Entries[Count - arg_index - 1];
        }

        /// <summary>
        /// Returns the topmost entry with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        protected Entry GetStackEntry(string arg_name)
        {
            return GetStackEntry(GetStackIndex(arg_name));
        }

        #endregion

        #region Get

        /// <summary>
        /// Returns the topmost transformation with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        public ITransformation<Operand> Get(string arg_name)
        {
            // Get index
            int index = GetLastIndexOf(arg_name);

            // Return transformation
            return Entries[index].Transformation;
        }

        #endregion

        #region Push

        /// <summary>
        /// Moves a transformation onto the stack.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <param name="arg_transformation"></param>
        public void Push(
            string arg_name,
            ITransformation<Operand> arg_transformation)
        {
            // Create entry
            Entry entry = new Entry(this, arg_name, arg_transformation);

            // Push onto stack
            Add(entry);
        }

        #endregion

        #region Pop

        /// <summary>
        /// Pops the top transformation off the stack.
        /// </summary>
        /// <returns></returns>
        public ITransformation<Operand> Pop()
        {
            // Validate
            if (Count == 0)
            {
                throw new IndexOutOfRangeException(
                    "Empty transformation stack was popped");
            }

            // Get transformation
            ITransformation<Operand> transformation = TopEntry.Transformation;

            // Remove
            Remove();

            // Done
            return transformation;
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the given named transformation at the given depth index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <param name="arg_name"></param>
        /// <param name="arg_transformation"></param>
        public void Insert(
            int arg_index,
            string arg_name,
            ITransformation<Operand> arg_transformation)
        {
            // Create entry
            Entry entry = new Entry(this, arg_name, arg_transformation);

            // Insert (at reversed index, conservation adjusted)
            Insert(Count - arg_index, entry);
        }

        #endregion

        #region Replace

        /// <summary>
        /// Replaces the transformation at the given depth index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <param name="arg_transformation"></param>
        public void Replace(int arg_index, ITransformation<Operand> arg_transformation)
        {
            // Get entry
            Entry entry = GetStackEntry(arg_index);

            // Replace transformation
            entry.Transformation = arg_transformation;
        }

        /// <summary>
        /// Replaces the topmost transformation with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <param name="arg_transformation"></param>
        public void Replace(string arg_name, ITransformation<Operand> arg_transformation)
        {
            // Get topmost entry with the given name
            Entry entry = GetStackEntry(arg_name);

            // Replace transformation
            entry.Transformation = arg_transformation;
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the top transformation from the stack.
        /// </summary>
        public void RemoveTop()
        {
            Remove();
        }

        #endregion
    }
}
