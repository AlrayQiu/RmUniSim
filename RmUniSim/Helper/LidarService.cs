using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;

namespace com.alray.rmunisim.Services
{
    public class LidarService<T> where T : IRayCaster
    {
        public static string[] Lidars =
            AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null)!; }
            })
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t =>
            {
                if (!t.IsGenericTypeDefinition)
                {
                    return typeof(ILidar<T>).IsAssignableFrom(t);
                }
                try
                {
                    var constructed = t.MakeGenericType(typeof(T));
                    return typeof(ILidar<T>).IsAssignableFrom(constructed);
                }
                catch { return false; }
            })
            .Select(t => t.IsGenericType ? t.Name.Split('`')[0] : t.Name)
            .ToArray();

        public static ReadOnlyDictionary<string, Func<ILidar<T>>> Builders = new(
                   AppDomain.CurrentDomain
                   .GetAssemblies()
                   .SelectMany(a =>
                   {
                       try { return a.GetTypes(); }
                       catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null)!; }
                   })
                   .Where(t => t.IsClass && !t.IsAbstract)
                   .Where(t =>
                   {
                       if (!t.IsGenericTypeDefinition)
                       {
                           return typeof(ILidar<T>).IsAssignableFrom(t);
                       }
                       try
                       {
                           var constructed = t.MakeGenericType(typeof(T));
                           return typeof(ILidar<T>).IsAssignableFrom(constructed);
                       }
                       catch { return false; }
                   })
                    .ToDictionary(
                        t => t.IsGenericType ? t.Name.Split('`')[0] : t.Name,
                        t =>
                        {
                            try
                            {
                                Type targetType = t.IsGenericTypeDefinition ? t.MakeGenericType(typeof(T)) : t;
                                return (Func<ILidar<T>>)(() => (ILidar<T>)Activator.CreateInstance(targetType)!);
                            }
                            catch
                            {
                                return () => throw new InvalidOperationException($"无法创建类型 {t.FullName} 的实例");
                            }
                        }
                    ));

    }
}