

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Helper;
using com.alray.rmunisim.Contracts.Interfaces;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars;
using UnityEngine;

namespace com.alray.rmunisim.RoboticsSim.Domain
{
    public static class LidarFactory<TLidar> where TLidar : ILidar, new()
    {
        public static TLidar Build(MonoBehaviour behaviour, Transform lidarTrans)
        {
            TLidar lidar = new();
            EventBinderHelper<Transform, TLidar>.Bind(lidarTrans, behaviour, lidar);
            return lidar;
        }
    }
    public static class LidarFactory
    {

        public readonly static ReadOnlyDictionary<string, Func<MonoBehaviour, Transform, ILidar>> lidarBuilders =
        new(
        AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ILidar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t =>
            {
                var factoryType = typeof(LidarFactory<>).MakeGenericType(t);
                var buildMethod =
                    factoryType.GetMethod("Build", BindingFlags.Public | BindingFlags.Static)
                    ?? throw new InvalidOperationException($"Build method not found for {t.Name}");
                // 使用闭包包装反射调用
                ILidar builder(MonoBehaviour mono, Transform trans) =>
                    (ILidar)buildMethod.Invoke(null, new object[] { mono, trans });
                return new KeyValuePair<string, Func<MonoBehaviour, Transform, ILidar>>(t.Name, builder);
            })
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
        );

        public readonly static string[] LidarTyps = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(a => a.GetTypes())
          .Where(t => typeof(ILidar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
          .Select(t => t.Name)
          .ToArray();

    }
}