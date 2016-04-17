// CogMan/Bender/Generic/ITransformation.cs © 2006 Andreas Wickman
using IDisposable = System.IDisposable;
using EventHandler = System.EventHandler;

namespace CogMan.Bender.Generic
{
    /// <summary>
    /// Defines the interface of a transformation
    /// (as the transformation stack expects it).
    /// </summary>
    public interface ITransformation<Operand>: IDisposable
        where Operand: struct
    {
        /// <summary>
        /// Returns the operand of this transformation.
        /// This will normally be a matrix.
        /// </summary>
        /// <param name="arg_result"></param>
        void GetOperand(out Operand arg_result);

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
        void Combine(ref Operand arg_other, out Operand arg_result);

        /// <summary>
        /// The event that should fire when a transformation is changed.
        /// The transformation chain entries will listen to this event
        /// in order to be notified when the combined transformation needs
        /// to be recalculated.
        /// </summary>
        event EventHandler TransformationChanged;
    }
}
