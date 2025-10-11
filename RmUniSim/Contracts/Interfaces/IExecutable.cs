using System.Data.Common;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface IExecutable
    {
        public void Update(float deltaTime);
    }
}