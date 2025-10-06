

using System;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Helper;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;

namespace com.alray.rmunisim.RoboticsSim.Domain
{
    public static class LidarFactory<TLidar> where TLidar : IPushSensor<LidarData>, IEventUpdateBinder<Transform>, new()
    {
        public static TLidar Build(MonoBehaviour behaviour, Transform lidarTrans, Action<LidarData> dataProcessor)
        {
            TLidar lidar = new();
            lidar.AddDataProcessor(dataProcessor);
            EventBinderHelper<Transform, TLidar>.Bind(lidarTrans, behaviour, lidar);
            return lidar;
        }
    }
}