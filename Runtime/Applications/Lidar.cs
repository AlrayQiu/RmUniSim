

using System;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Helper;
using com.alray.rmunisim.Contracts.Interfaces;
using com.alray.rmunisim.RoboticsSim.Domain;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{
    public class Lidar : MonoBehaviour
    {
        public LidarTypes LidarType;

        /// <summary>
        /// 只是保存下引用，免得被GC回收了
        /// </summary>
        private object lidarObj;

        void Start()
        {
            lidarObj = LidarType switch
            {
                LidarTypes.Mid360 => LidarFactory<Mid360>.Build(this, transform, msg =>
                {
                    foreach (var p in msg.data) Debug.DrawLine(transform.position, new(p.X, p.Y, p.Z));
                }),
                _ => throw new NotImplementedException($"{LidarType.GetType().Name} error with value:${LidarType}")
            };
        }

        void Update()
        {

        }
    }
}