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
    public static class CommMiddlewareCache<T> where T : struct
    {

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
                    publisherCache.Add(new PublisherEntry { typeName = publisherType, instance = CommMiddlewareService<T>.MiddlewareBuilders[publisherType]() });

                return publisherCache.First(x => x.typeName == publisherType).instance;
            }
        }
    }
}