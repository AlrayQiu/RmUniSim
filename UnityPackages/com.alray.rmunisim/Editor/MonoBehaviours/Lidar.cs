

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Controllers.Chassis;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars;
using com.alray.rmunisim.Services;
using com.alray.rmunisim.Visualization.Infrastructure;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{
    [DisallowMultipleComponent]
    public class Lidar : MonoBehaviour
    {
        /// <summary>
        /// Caches
        /// </summary>
        [SerializeReference]
        public CommMiddlewareCache<PointCloudData>.PublisherCache commCache = new();

        /// <summary>
        /// 由 Inspector 获取
        /// </summary>
        public string LidarType;
        public string PublisherType;
        public bool EnableRender = false;
        public bool EnablePublish = false;
        public Material Material;

        /// <summary>
        /// 功能类
        /// </summary>
        private ILidar<RayCaster> lidarObj;
        private PointCloudDrawer pcdDrawer;
        [SerializeReference]
        public IPublisher<PointCloudData> publisher;


        void Start()
        {

            lidarObj = LidarService<RayCaster>.Builders[LidarType]();
            lidarObj.Init(new RayCaster(transform));
            ExecutorManager.GetOrCreateInstance().Add(lidarObj);

            if (EnableRender)
            {
                pcdDrawer = DrawerManager.Instance
                     .RegistryDrawer<PointCloudDrawer, IPushSensor<PointCloudData>>(
                        $"{name}_{LidarType}_pointCloud",
                        Material,
                        lidarObj
                     );
            }
            if (EnablePublish)
            {
                publisher.Init();
                lidarObj.AddDataProcessor(publisher.Publish);
            }
        }

        void OnRenderObject()
        {
            pcdDrawer?.Draw();
        }

    }
}