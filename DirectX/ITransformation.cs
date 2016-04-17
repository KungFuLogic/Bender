// CogMan/Bender/DirectX/ITransformation.cs © 2006 Andreas Wickman
using Matrix = Microsoft.DirectX.Matrix;

namespace CogMan.Bender.DirectX
{
    /// <summary>
    /// Defines the interface of a DirectX transformation
    /// (as the transformation stack expects it).
    /// </summary>
    public interface ITransformation: CogMan.Bender.Generic.ITransformation<Matrix>
    {
    }
}
