// CogMan/Bender/Generic/TransformationChain.cs © 2006 Andreas Wickman
using System.Collections.Generic;
using IDisposable = System.IDisposable;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// Represents a chain of transformations.
    /// This in itself is also a transformation.
    /// </summary>
    public abstract partial class TransformationChain<Operand>: Transformation<Operand>
        where Operand: struct
    {
        #region Object construction and destruction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TransformationChain()
        {
            // Init members
            m_entries = new List<Entry>();
            m_invalidatedOperandCombination = false;
        }

        /// <summary>
        /// Disposes the resources held by this stack.
        /// </summary>
        public override void Dispose()
        {
            // Dispose all entries
            for (int i = 0; i < Length; i++) m_entries[i].Dispose();

            // Remove entries
            m_entries.Clear();

            // Clear
            m_entries = null;

            // Let base class dispose
            base.Dispose();
        }

        #endregion

        #region Chain elements

        /// <summary>
        /// Holds the entries in the chain.
        /// </summary>
        private List<Entry> m_entries;

        /// <summary>
        /// Returns the entry array.
        /// </summary>
        protected List<Entry> Entries
        {
            get
            {
                return m_entries;
            }
        }

        /// <summary>
        /// Returns the number of entries in the chain.
        /// </summary>
        protected int Length
        {
            get
            {
                return m_entries.Count;
            }
        }

        #endregion

        #region Abstract methods that need implementation

        /// <summary>
        /// Returns the identity operand
        /// The identity operand will have any other operand combine to itself.
        /// </summary>
        /// <param name="arg_operand"></param>
        protected abstract void GetIdentityOperand(out Operand arg_operand);

        #endregion

        #region Method overrides

        /// <summary>
        /// Returns the combined operand of the chain.
        /// This will normally be a matrix.
        /// </summary>
        /// <param name="arg_result"></param>
        public override void GetOperand(out Operand arg_result)
        {
            // Return identity operand on empty stack.
            if (Length == 0)
            {
                // Use identity operand
                GetIdentityOperand(out arg_result);
                return;
            }

            // Recalculate operand combination (if needed)
            LazyRecalculation();

            // Get top entry
            Entry entry = m_entries[Length - 1];

            // Copy combined transformation operand
            entry.Transformation.GetOperand(out arg_result);
        }

        #endregion

        #region Lazy Depender pattern

        /// <summary>
        /// Signals the chain as invalidated.
        /// </summary>
        protected virtual void InvalidateTransformation()
        {
            // The chain transformation is now invalidated
            m_invalidatedOperandCombination = true;

            // Fire event
            InvalidateTransformationDependers();
        }

        /// <summary>
        /// Holds true if the combined operand is invalidated and needs recalculation.
        /// </summary>
        private bool m_invalidatedOperandCombination;

        /// <summary>
        /// If any entry has been modified, this method recalculates
        /// the combined transformation operands.
        ///
        /// Actually the chain combined operand is the combined operand
        /// of the last entry.
        /// 
        /// This method needs to be called before using the combined chain operand.
        /// 
        /// This method will leave the chain combined operand and all the entry
        /// combined operands up to date.
        /// </summary>
        protected void LazyRecalculation()
        {
            // Validate
            if (!m_invalidatedOperandCombination) return;

            // To keep track of when to start modify update
            bool modify = false;

            // The rest of the entries performs combinations with the transformation before.
            for (int i = 0; i < Length; i++)
            {
                Entry entry = m_entries[i];
                if (modify || entry.m_invalidatedCombinedOperand)
                {
                    // Start modification (may already have begun)
                    modify = true;

                    if (i == 0)
                    {
                        // Get start operand from first entry
                        entry.Transformation.GetOperand(out entry.m_combinedOperand);
                    }
                    else
                    {
                        // Combine all the subsequent transformations
                        Entry baseEntry = m_entries[i - 1];
                        entry.Transformation.Combine(
                            ref baseEntry.m_combinedOperand,
                            out entry.m_combinedOperand);
                    }

                    // Entry is not invalidated anymore.
                    entry.m_invalidatedCombinedOperand = false;
                }
            }

            // Stack is not invalidated anymore
            m_invalidatedOperandCombination = false;
        }

        #endregion
    }
}
