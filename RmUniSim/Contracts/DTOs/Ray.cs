using System;
using System.Numerics;
namespace com.alray.rmunisim.Contracts.DTOs
{
    [Serializable]
    public struct Ray
    {
        public Ray(Vector3 position, Vector3 direction, float distance)
        {
            Direction = direction;
            Distance = distance;
        }
        public Vector3 Direction;
        public float Distance;
    }
}