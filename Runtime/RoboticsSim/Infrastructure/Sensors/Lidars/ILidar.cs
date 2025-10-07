

using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;

namespace com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars
{
    public interface ILidar : IPushSensor<LidarData>, IEventUpdateBinder<Transform> { }
}