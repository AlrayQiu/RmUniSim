
using System.Numerics;
using com.alray.rmunisim.Contracts.DTOs;

namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface IRayCaster
    {
        PointCloudData Cast(Ray[] rays);
    }

}