// CogMan/Bender/DirectX/Transformation.cs © 2006 Andreas Wickman
using Matrix = Microsoft.DirectX.Matrix;
using Device = Microsoft.DirectX.Direct3D.Device;

namespace CogMan.Bender.DirectX
{
    /// <summary>
    /// A DirectX implementation of ITransformation.
    /// Derived classes need implement the abstract methods.
    /// </summary>
    public abstract class Transformation:
        CogMan.Bender.Generic.Transformation<Matrix>,
        ITransformation
    {
        #region Redefinition of methods that need be implemented

        /// <summary>
        /// Returns the matrix of this transformation.
        /// </summary>
        /// <param name="arg_result"></param>
        public abstract override void GetOperand(out Matrix arg_result);

        /// <summary>
        /// Combines this transformation matrix with the other (given matrix) and stores
        /// the outcome in the result matrix.
        /// 
        /// The multiplication will be:
        /// 
        ///     result = this * other
        /// 
        /// </summary>
        /// <param name="arg_other"></param>
        /// <param name="arg_result"></param>
        public abstract override void Combine(ref Matrix arg_other, out Matrix arg_result);

        #endregion

        #region Utility methods

        /// <summary>
        /// Sets the world transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceWorldTransformation(Device arg_device)
        {
            // Get matrix
            Matrix worldTransformation;
            GetOperand(out worldTransformation);
            arg_device.Transform.World = worldTransformation;
        }

        /// <summary>
        /// Sets the view transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceViewTransformation(Device arg_device)
        {
            // Get matrix
            Matrix viewTransformation;
            GetOperand(out viewTransformation);
            arg_device.Transform.View = viewTransformation;
        }

        /// <summary>
        /// Sets the projection transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public virtual void SetAsDeviceProjectionTransformation(Device arg_device)
        {
            // Get matrix
            Matrix projectionTransformation;
            GetOperand(out projectionTransformation);
            arg_device.Transform.Projection = projectionTransformation;
        }

        #endregion
    }
}
