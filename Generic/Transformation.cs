// CogMan/Bender/Generic/Transformation.cs © 2006 Andreas Wickman
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// A default base implementation of ITransformation.
    /// Derived classes need implement the abstract methods.
    /// </summary>
    public abstract class Transformation<Operand>: ITransformation<Operand>
        where Operand: struct
    {
        #region Abstract methods that need implementation

        /// <summary>
        /// Returns the operand of this transformation.
        /// This is normally a matrix.
        /// </summary>
        /// <param name="arg_result"></param>
        public abstract void GetOperand(out Operand arg_result);

        /// <summary>
        /// Combines this transformation operand with the other (given operand) and stores
        /// the outcome in the result operand.
        /// 
        /// This will normally be matrix multiplication.
        /// 
        /// The multiplication will be:
        /// 
        ///     result = this * other
        /// 
        /// </summary>
        /// <param name="arg_other"></param>
        /// <param name="arg_result"></param>
        public abstract void Combine(ref Operand arg_other, out Operand arg_result);

        #endregion

        #region Dependency pattern

        /// <summary>
        /// The event that should fires on transformation events.
        /// The transformation stack entries will listen to this event
        /// in order to be notified when the stack transformation needs
        /// to be recalculated.
        /// </summary>
        public event EventHandler TransformationChanged;

        /// <summary>
        /// Invalidates all dependers on this transformation.
        /// </summary>
        protected virtual void InvalidateTransformationDependers()
        {
            // If no listeners, make an early exit
            if (TransformationChanged == null) return;

            // Fire
            TransformationChanged(this, new EventArgs());
        }

        /// <summary>
        /// Disposes all resources held by this transformation.
        /// </summary>
        public virtual void Dispose()
        {
            // Unregister all remaining event listeners
            EventHandler[] eventListeners =
                (EventHandler[])TransformationChanged.GetInvocationList();
            foreach (EventHandler eventListener in eventListeners)
            {
                TransformationChanged -= eventListener;
            }
        }

        #endregion
    }
}
