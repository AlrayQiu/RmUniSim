using System;
using System.Numerics;
namespace com.alray.rmunisim.Contracts.DTOs
{
    [Serializable]
    public class LidarData
    {
        public LidarData(Vector3[] data)
        {
            this.data = data;
        }
        public readonly Vector3[] data;
    }
}