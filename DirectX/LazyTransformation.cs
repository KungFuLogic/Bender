// CogMan/Bender/DirectX/LazyTransformation.cs © 2006 Andreas Wickman
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;
using Matrix = Microsoft.DirectX.Matrix;
using Device = Microsoft.DirectX.Direct3D.Device;

namespace CogMan.Bender.DirectX
{
    /// <summary>
    /// Defines a matrix transformation where the matrix is depending
    /// on some other object or conditions. The matrix will be recalculated lazily.
    /// </summary>
    public abstract class LazyTransformation: Transformation
    {
        #region Object construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LazyTransformation()
        {
            // Initialize the matrix
            m_matrix = Matrix.Identity;
            m_invalidatedMatrix = true;

            // Setup the event delegate
            m_invalidatedTransformationHandler = new EventHandler(OnInvalidatedTransformation);
        }

        #endregion

        #region Transformation method overrides

        /// <summary>
        /// Returns the matrix. Recalculates the matrix if needed.
        /// </summary>
        /// <param name="arg_result"></param>
        public override void GetOperand(out Matrix arg_result)
        {
            // Ensure matrix is up-to-date
            LazyRecalculation();

            // Set output
            arg_result = m_matrix;
        }

        /// <summary>
        /// Multiplies the local with the given matrix. Recalculates the local matrix if needed.
        /// </summary>
        /// <param name="arg_other"></param>
        /// <param name="arg_result"></param>
        public override void Combine(ref Matrix arg_other, out Matrix arg_result)
        {
            // Ensure matrix is up-to-date
            LazyRecalculation();

            // Multiply
            arg_result = Matrix.Multiply(m_matrix, arg_other);
        }

        #endregion

        #region Transformation matrix

        /// <summary>
        /// Holds the local transformation matrix.
        /// </summary>
        private Matrix m_matrix;

        /// <summary>
        /// Accesses the local transformation matrix.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                // Ensure matrix is up-to-date
                LazyRecalculation();

                // Return the local matrix.
                return m_matrix;
            }
            set
            {
                // Set matrix
                m_matrix = value;

                // Extract properties
                ExtractProperties(ref value);

                // Clear flag (avoid recalculation)
                m_invalidatedMatrix = false;

                // Fire event
                InvalidateTransformationDependers();
            }
        }

        #endregion

        #region Abstract methods that need implementation

        /// <summary>
        /// Extracts relevant spatial properties from the given matrix.
        /// </summary>
        protected abstract void ExtractProperties(ref Matrix arg_matrix);

        /// <summary>
        /// Recalculates the matrix of this transformation.
        /// </summary>
        /// <param name="arg_matrix"></param>
        protected abstract void Recalculate(ref Matrix arg_matrix);

        #endregion

        #region Lazy Depender Pattern

        /// <summary>
        /// Holds true if the local transformation matrix is not up to date.
        /// </summary>
        private bool m_invalidatedMatrix;

        /// <summary>
        /// If needed, this method will recalculate the local transformation matrix.
        /// </summary>
        private void LazyRecalculation()
        {
            // Validate
            if (!m_invalidatedMatrix) return;

            // Recalculate
            Recalculate(ref m_matrix);

            // Clear flag
            m_invalidatedMatrix = false;
        }

        /// <summary>
        /// Holds the event handler for this entry.
        /// This reference is kept here to remove it on dispose.
        /// </summary>
        private EventHandler m_invalidatedTransformationHandler;

        /// <summary>
        /// Returns the transformation event handler.
        /// </summary>
        protected EventHandler InvalidatedTransformationHandler
        {
            get
            {
                return m_invalidatedTransformationHandler;
            }
        }

        /// <summary>
        /// Invalidates the transformation and notifies dependers.
        /// </summary>
        protected void InvalidateTransformation()
        {
            OnInvalidatedTransformation(this, new EventArgs());
        }

        /// <summary>
        /// Handler for events (that occur on changes in the object we're depending on).
        /// </summary>
        /// <param name="arg_source"></param>
        /// <param name="arg_eventArgs"></param>
        protected virtual void OnInvalidatedTransformation(
            object arg_source,
            EventArgs arg_eventArgs)
        {
            // Mark the local transformation as invalidated
            m_invalidatedMatrix = true;

            // Propagate the event
            InvalidateTransformationDependers();
        }

        #endregion

        #region Utility methods (efficiency overrides)

        /// <summary>
        /// Sets the world transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public override void SetAsDeviceWorldTransformation(Device arg_device)
        {
            // Ensure values are up-to-date
            LazyRecalculation();

            // Set world transform
            arg_device.Transform.World = m_matrix;
        }

        /// <summary>
        /// Sets the view transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public override void SetAsDeviceViewTransformation(Device arg_device)
        {
            // Ensure values are up-to-date
            LazyRecalculation();

            // Set view transform
            arg_device.Transform.View = m_matrix;
        }

        /// <summary>
        /// Sets the projection transformation matrix of the device.
        /// </summary>
        /// <param name="arg_device"></param>
        public override void SetAsDeviceProjectionTransformation(Device arg_device)
        {
            // Ensure values are up-to-date
            LazyRecalculation();

            // Set projection transform
            arg_device.Transform.Projection = m_matrix;
        }

        #endregion
    }
}
