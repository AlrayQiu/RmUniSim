
using System;
using System.Collections;
using System.Diagnostics;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{
    public class CoroutineExecutor : IExecutor
    {

        public event Action<float> Update = null;
        public Stopwatch stopwatch = new();

        public CoroutineExecutor(MonoBehaviour behaviour)
        {
            behaviour.StartCoroutine(Launch());
        }

        public IEnumerator Launch()
        {
            stopwatch.Restart();
            while (true)
            {
                yield return null;
                float elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000f;
                stopwatch.Restart();
                if (elapsedSeconds > 0.1f) elapsedSeconds = 0.1f;
                Update?.Invoke(elapsedSeconds);
            }
        }

        public void Add(IExecutable action) => Update += action.Update;
    }
}