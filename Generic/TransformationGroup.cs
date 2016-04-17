// CogMan/Bender/Generic/TransformationGroup.cs © 2006 Andreas Wickman
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using IEnumerable = System.Collections.IEnumerable;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// Represents a group of transformations.
    /// </summary>
    /// <typeparam name="Operand"></typeparam>
    public abstract class TransformationGroup<Operand>:
        TransformationChain<Operand>,
        IEnumerable<ITransformation<Operand>>
        where Operand: struct
    {
        #region Entry collection properties

        /// <summary>
        /// Returns the number of transformations in the group.
        /// </summary>
        public int Count
        {
            get
            {
                return Length;
            }
        }

        /// <summary>
        /// Returns a list of the entry names.
        /// </summary>
        public IList<string> EntryNames
        {
            get
            {
                // Create and populate list of names
                string[] names = new string[Count];
                for (int i = 0; i < Count; i++)
                {
                    names[i] = Entries[i].m_name;
                }

                // Return as list
                return names;
            }
        }

        #endregion

        #region Entry access

        /// <summary>
        /// Returns the first entry with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        protected Entry GetEntry(string arg_name)
        {
            return Entries[GetFirstIndexOf(arg_name)];
        }

        /// <summary>
        /// Adds a named transformation to the group.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <param name="arg_transformation"></param>
        public void Add(string arg_name, ITransformation<Operand> arg_transformation)
        {
            // Create entry
            Entry entry = new Entry(this, arg_name, arg_transformation);

            // Add entry
            Add(entry);
        }

        /// <summary>
        /// Removes the transformation with the given name.
        /// </summary>
        /// <param name="arg_name"></param>
        public void Remove(string arg_name)
        {
            // Get index
            int index = GetFirstIndexOf(arg_name);

            // Remove
            Remove(index);
        }

        /// <summary>
        /// Removes the transformation at the given index.
        /// </summary>
        /// <param name="arg_index"></param>
        public void RemoveAt(int arg_index)
        {
            // Remove
            Remove(arg_index);
        }

        /// <summary>
        /// Inserts the given named transformation at the given index.
        /// </summary>
        /// <param name="arg_index"></param>
        /// <param name="arg_name"></param>
        /// <param name="arg_transformation"></param>
        public void InsertAt(
            int arg_index,
            string arg_name,
            ITransformation<Operand> arg_transformation)
        {
            // Create entry
            Entry entry = new Entry(this, arg_name, arg_transformation);

            // Insert entry
            Insert(arg_index, entry);
        }

        #endregion

        #region Indexed transformation access

        /// <summary>
        /// Indexer for the contained transformations.
        /// Indexed by name.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        public ITransformation<Operand> this[string arg_name]
        {
            get
            {
                // Return transformation of entry
                return GetEntry(arg_name).Transformation;
            }
            set
            {
                // Replace transformation of entry
                GetEntry(arg_name).Transformation = value;
            }
        }

        /// <summary>
        /// Indexer for the contained transformations.
        /// Zero-based index.
        /// </summary>
        /// <param name="arg_name"></param>
        /// <returns></returns>
        public ITransformation<Operand> this[int arg_index]
        {
            get
            {
                // Return transformation of entry
                return Entries[arg_index].Transformation;
            }
            set
            {
                // Replace transformation of entry
                Entries[arg_index].Transformation = value;
            }
        }

        #endregion

        #region Enumerable transformation access

        /// <summary>
        /// Provides a typed enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ITransformation<Operand>> GetEnumerator()
        {
            // Create and populate list of transformations
            ITransformation<Operand>[] transformations = new ITransformation<Operand>[Count];
            for (int i = 0; i < Count; i++)
            {
                // Get entry
                Entry entry = Entries[i];

                // Store transformation in array
                transformations[i] = entry.Transformation;
            }
            return ((IList<ITransformation<Operand>>)transformations).GetEnumerator();
        }

        /// <summary>
        /// Provides an untyped enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // Create and populate list of transformations
            ITransformation<Operand>[] transformations = new ITransformation<Operand>[Count];
            for (int i = 0; i < Count; i++)
            {
                // Get entry
                Entry entry = Entries[i];

                // Store transformation in array
                transformations[i] = entry.Transformation;
            }
            return transformations.GetEnumerator();
        }

        #endregion
    }
}
