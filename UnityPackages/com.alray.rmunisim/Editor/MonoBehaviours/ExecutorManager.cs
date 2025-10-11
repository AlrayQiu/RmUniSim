using System;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{
    [DisallowMultipleComponent]
    public class ExecutorManager : MonoBehaviour
    {
        private static ExecutorManager _instance;

        public IExecutor executor;

        public enum ExecutorTypes { Coroutine }

        public static ExecutorTypes ExecutorType;


        public static IExecutor GetOrCreateInstance()
        {
            if (_instance != null)
                return _instance.CheckAndReturn();

            _instance = FindFirstObjectByType<ExecutorManager>();
            if (_instance != null)
                GameObject.DestroyImmediate(_instance);

            GameObject go = new("ExecutorManager");
            _instance = go.AddComponent<ExecutorManager>();
            DontDestroyOnLoad(go);

            return _instance.CheckAndReturn();
        }

        private IExecutor CheckAndReturn()
        {
            if (executor != null)
                return executor;
            executor = ExecutorType switch
            {
                ExecutorTypes.Coroutine => new CoroutineExecutor(this),
                _ => throw new NotSupportedException()
            };
            return executor;
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}