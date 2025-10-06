
using UnityEngine;
using System.Collections;
using com.alray.rmunisim.Contracts.Interfaces;

namespace com.alray.rmunisim.Contracts.Helper
{

    public static class PollingBinderHelper<T, Self> where Self : IPollingUpdateBinder<T>
    {
        public static Self Bind(T target, UnityEngine.MonoBehaviour behaviour, Self self, float updateFPS)
        {
            self.Context?.Dispose();
            static IEnumerator enumerator(float updateFPS, Self self)
            {
                while (true)
                {
                    self.UpdateBinder();
                    yield return new WaitForSeconds(updateFPS);
                }
            }

            var e = enumerator(1 / updateFPS, self);
            self.Context = new()
            {
                BindBehaviour = behaviour,
                BindTarget = target,
                enumerator = e,
            };

            behaviour.StartCoroutine(e);

            return self;
        }

        public static void Dispose(Self self) => self.Context?.Dispose();
    }

    public static class EventBinderHelper<T, Self> where Self : IEventUpdateBinder<T>
    {
        public static Self Bind(T target, UnityEngine.MonoBehaviour behaviour, Self self)
        {
            self.Context?.Dispose();

            self.Context = new()
            {
                BindBehaviour = behaviour,
                BindTarget = target,
                enumerator = self.UpdateBinder(),
            };

            behaviour.StartCoroutine(self.Context.Value.enumerator);

            return self;
        }

        public static void Dispose(Self self) => self.Context?.Dispose();
    }
}