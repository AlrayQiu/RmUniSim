using System.Data.Common;
using System.Numerics;
using UnityEngine;

namespace com.alray.rmunisim.Contracts.Interfaces
{

    public interface IDrawer
    {
        void Draw();
    };

    public interface IDrawer<T> : IDrawer
    {
        void Initialize(Material material, T sensor);
    }
}