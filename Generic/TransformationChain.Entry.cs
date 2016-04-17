// CogMan/Bender/Generic/TransformationChain.Entry.cs © 2006 Andreas Wickman
using IDisposable = System.IDisposable;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// This source module deinfes the entry class to be used
    /// in the transformation chain.
    /// </summary>
    public abstract partial class TransformationChain<Operand>
        where Operand: struct
    {
        /// <summary>
        /// Represents a chain entry. Holds a name, a transformation,
        /// a combined transformation operand and an indicator of whether
        /// the combined operand is up-to-date.
        /// </summary>
        protected class Entry: IDisposable
        {
            #region Object construction and destruction

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="arg_chain"></param>
            /// <param name="arg_name"></param>
            /// <param name="arg_transformation"></param>
            internal Entry(
                TransformationChain<Operand> arg_chain,
                string arg_name,
                ITransformation<Operand> arg_transformation)
            {
                // Set members
                m_chain = arg_chain;
                m_name = arg_name;
                m_transformation = arg_transformation;
                m_invalidatedCombinedOperand = true;

                // The entry will listen on the object
                m_transformationChangedHandler = new EventHandler(OnTransformationChanged);
                arg_transformation.TransformationChanged += m_transformationChangedHandler;
            }

            /// <summary>
            /// Disposes resources held in this entry.
            /// </summary>
            public void Dispose()
            {
                // Release references
                m_transformation.TransformationChanged -= m_transformationChangedHandler;
                m_transformationChangedHandler = null;
                m_transformation = null;
                m_chain = null;
            }

            #endregion

            #region State

            /// <summary>
            /// Holds a reference to the chain that contains this entry.
            /// </summary>
            private TransformationChain<Operand> m_chain;

            /// <summary>
            /// Holds the name of the entry.
            /// </summary>
            internal string m_name;

            /// <summary>
            /// Holds the transformation.
            /// </summary>
            public ITransformation<Operand> m_transformation;

            /// <summary>
            /// Exposes the transformation for access.
            /// </summary>
            public ITransformation<Operand> Transformation
            {
                get
                {
                    return m_transformation;
                }
                set
                {
                    // Remove event listening on old object
                    m_transformation.TransformationChanged -= m_transformationChangedHandler;

                    // Replace object
                    m_transformation = value;

                    // Establish event listening on new object
                    value.TransformationChanged += m_transformationChangedHandler;

                    // The combined entry operand is now invalidated
                    m_invalidatedCombinedOperand = true;

                    // And so is the combined chain operand
                    m_chain.m_invalidatedOperandCombination = true;
                }
            }

            #endregion

            #region Lazy Depender pattern

            /// <summary>
            /// Holds true if the combined operand needs recalculating
            /// </summary>
            public bool m_invalidatedCombinedOperand;

            /// <summary>
            /// Holds the combined operand.
            /// </summary>
            public Operand m_combinedOperand;

            /// <summary>
            /// Holds the event handler for this entry.
            /// This reference is kept here to remove it on dispose.
            /// </summary>
            private EventHandler m_transformationChangedHandler;

            /// <summary>
            /// Handler for events (that occur on transformation state change).
            /// </summary>
            /// <param name="arg_source"></param>
            /// <param name="arg_eventArgs"></param>
            private void OnTransformationChanged(object arg_source, EventArgs arg_eventArgs)
            {
                // The combined entry operand is invalidated
                m_invalidatedCombinedOperand = true;

                // The combined chain operand is invalidated
                m_chain.m_invalidatedOperandCombination = true;

                // Propagate transformation state change.
                m_chain.InvalidateTransformationDependers();
            }

            #endregion
        }
    }
}
