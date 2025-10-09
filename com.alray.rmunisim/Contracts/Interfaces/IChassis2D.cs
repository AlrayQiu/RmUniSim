using System.Numerics;

namespace com.alray.rmunisim.Contracts.Interfaces
{

    public interface IChassis2D
    {
        /// <summary>
        /// chassis angular velocity in radians per second.
        /// </summary>
        float AngularVelocity { get; }

        /// <summary>
        /// chassis linear velocity in meters per second.
        /// </summary>
        Vector2 LinearVelocity { get; }
    }
}