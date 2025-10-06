
using System.Collections;
using UnityEngine;

namespace com.alray.rmunisim.Contracts.Interfaces
{
    /// <summary>
    /// Bind the target to the superior implementation and update it in the background at a specific frame rate. 
    /// The behavior after binding is determined by the implementation
    /// </summary>
    /// <typeparam name="T">type to bind</typeparam>
    public interface IPollingUpdateBinder<T>
    {

        struct BinderContext
        {
            public T BindTarget;
            public MonoBehaviour BindBehaviour;
            public float UpdateInterval;
            public IEnumerator enumerator;

            public readonly void Dispose()
            {
                BindBehaviour.StopCoroutine(enumerator);
            }
        }

        BinderContext? Context { get; set; }
        void UpdateBinder();
    }

    /// <summary>
    /// Bind the target to a UnityObject, with the implementation class determining the timing of its invocation
    /// </summary>
    /// <typeparam name="T">type to bind</typeparam>
    public interface IEventUpdateBinder<T>
    {

        struct BinderContext
        {
            public T BindTarget;
            public MonoBehaviour BindBehaviour;
            public IEnumerator enumerator;

            public readonly void Dispose()
            {
                BindBehaviour.StopCoroutine(enumerator);
            }
        }

        BinderContext? Context { get; set; }
        IEnumerator UpdateBinder();
    }

}