using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;
namespace com.alray.rmunisim.Services
{
    public static class CommMiddlewareService<T> where T : struct
    {
        public static readonly string[] PublisherMiddlewares =
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null)!; }
            })
            .Where(t => t != null && !t.IsAbstract && !t.IsInterface)
            .Where(t => typeof(IPublisher<T>).IsAssignableFrom(t))
            .Where(t => typeof(IInitializable).IsAssignableFrom(t))
            .Select(a => $"{a.Namespace.Split('.').Last()}/{a.Name}")
            .ToArray();
        public static readonly ReadOnlyDictionary<string, Func<IPublisher<T>>> MiddlewareBuilders = new(
            AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null)!; }
            })
            .Where(t => t != null && !t.IsAbstract && !t.IsInterface)
            .Where(t => typeof(IPublisher<T>).IsAssignableFrom(t))
            .Where(t => typeof(IInitializable).IsAssignableFrom(t))
            .Select(t => ($"{t.Namespace.Split('.').Last()}/{t.Name}", (Func<IPublisher<T>>)(() => (IPublisher<T>)Activator.CreateInstance(t)!)))
            .ToDictionary(nf => nf.Item1, nf => nf.Item2)
        );

        public class PublisherCache
        {

            [Serializable]
            public class PublisherEntry
            {
                public string typeName;
                [SerializeReference]
                public IPublisher<T> instance;
            }
            [SerializeReference]
            public List<PublisherEntry> publisherCache = new();

            public IPublisher<T> Get(string publisherType)
            {
                if (!publisherCache.Any(x => x.typeName == publisherType))
                    publisherCache.Add(new PublisherEntry { typeName = publisherType, instance = MiddlewareBuilders[publisherType]() });

                return publisherCache.First(x => x.typeName == publisherType).instance;
            }
        }
    }
}