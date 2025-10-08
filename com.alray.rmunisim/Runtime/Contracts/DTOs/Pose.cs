using System;
using System.Numerics;
namespace com.alray.rmunisim.Contracts.DTOs
{
    [Serializable]
    public readonly struct Pose
    {
        public readonly string Header;
        public readonly long TimePoint;
        public readonly Vector3 Position;
        public readonly Quaternion Orientation;
    }
}