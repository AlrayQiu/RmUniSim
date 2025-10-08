using System;
using System.Collections.Frozen;
using System.Numerics;
namespace com.alray.rmunisim.Services
{
    public static class CommMiddlewareService<T> where T : struct
    {
        public static readonly FrozenDictionary<string, string[]> PublisherMiddlewares = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null)!; }
            })
            .Where(t => t != null && !t.IsAbstract && !t.IsInterface)
            .Where(t => typeof(IPublisher<T>).IsAssignableFrom(t))
            .GroupBy(t => t.Namespace ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(t => t.Name).ToArray()
            ).ToFrozenDictionary();
    }
}