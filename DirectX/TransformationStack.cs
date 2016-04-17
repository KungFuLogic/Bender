// CogMan/Bender/DirectX/TransformationStack.cs © 2006 Andreas Wickman
using Matrix = Microsoft.DirectX.Matrix;
using Vector3 = Microsoft.DirectX.Vector3;
using Device = Microsoft.DirectX.Direct3D.Device;

namespace CogMan.Bender.DirectX
{
    /// <summary>
    /// Transformation stack, adapted for DirectX
    /// </summary>
    public class TransformationStack:
        CogMan.Bender.Generic.TransformationStack<Matrix>,
        ITransformation
    {
        #region Abstract method implementations for DirectX

        /// <summary>
        /// Returns the identity operand
        /// The identity operand will have any other operand combine to itself.
        /// </summary>
        /// <param name="arg_operand"></param>
        protected override void GetIdentityOperand(out Matrix arg_operand)
        {
            arg_operand = Matrix.Identity;
        }

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
        public override void Combine(ref Matrix arg_other, out Matrix arg_result)
        {
            // An empty stack will combine a matrix to itself
            if (Length == 0)
            {
                arg_result = arg_other;
                return;
            }
            
            // Ensure values are up-to-date
            LazyRecalculation();

            // Combine
            arg_result = Matrix.Multiply(TopEntry.m_combinedOperand, arg_other);
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Sets the world transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceWorldTransformation(Device arg_device)
        {
            // An empty chain will set the identity matrix
            if (Length == 0)
            {
                arg_device.Transform.World = Matrix.Identity;
                return;
            }

            // Ensure values are up-to-date
            LazyRecalculation();

            // Set world transform
            arg_device.Transform.World = TopEntry.m_combinedOperand;
        }

        /// <summary>
        /// Sets the view transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceViewTransformation(Device arg_device)
        {
            // An empty chain will set the identity matrix
            if (Length == 0)
            {
                arg_device.Transform.View = Matrix.Identity;
                return;
            }

            // Ensure values are up-to-date
            LazyRecalculation();

            // Set view transform
            arg_device.Transform.View = TopEntry.m_combinedOperand;
        }

        /// <summary>
        /// Sets the projection transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceProjectionTransformation(Device arg_device)
        {
            // An empty chain will set the identity matrix
            if (Length == 0)
            {
                arg_device.Transform.Projection = Matrix.Identity;
                return;
            }

            // Ensure values are up-to-date
            LazyRecalculation();

            // Set projection transform
            arg_device.Transform.Projection = TopEntry.m_combinedOperand;
        }

        #endregion
    }
}
