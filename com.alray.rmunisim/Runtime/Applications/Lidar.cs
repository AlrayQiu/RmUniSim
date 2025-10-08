

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using com.alray.rmunisim.RoboticsSim.Domain;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars;
using com.alray.rmunisim.Visualization.Infrastructure;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{
    [DisallowMultipleComponent]
    public class Lidar : MonoBehaviour
    {
        /// <summary>
        /// 由 Inspector 获取
        /// </summary>
        public string LidarType;
        public bool EnableRender = false;
        public Material Material;

        /// <summary>
        /// 雷达本身
        /// </summary>
        private ILidar lidarObj;
        private PointCloudDrawer pcdDrawer;


        void Start()
        {
            lidarObj = LidarFactory.lidarBuilders[LidarType](
                this,
                transform
                );

            if (EnableRender)
            {
                pcdDrawer = DrawerManagerSingleton.Instance
                     .RegistryDrawer<PointCloudDrawer, IPushSensor<PointCloudData>>(
                         $"{this.name}_{this.LidarType}_pointCloud",
                         Material,
                         lidarObj
                     );
            }
        }

        void OnRenderObject()
        {
            pcdDrawer?.Draw();
        }



    }
}