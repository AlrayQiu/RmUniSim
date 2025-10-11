using System.Collections.Generic;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawerManager
{
    private static readonly DrawerManager _instance = new();

    public static DrawerManager Instance => _instance;

    public static Dictionary<string, List<IDrawer>> Drawers;

    private DrawerManager()
    {
        Drawers = new();
    }

    // 示例方法
    public T RegistryDrawer<T, TSensorType>(string name, Material material, TSensorType sensor) where T : IDrawer<TSensorType>, new()
    {
        if (!Drawers.TryGetValue(name, out var drawer1))
            Drawers[name] = new();
        T drawer = new();
        drawer.Initialize(material, sensor);
        Drawers[name].Add(drawer);
        return drawer;
    }
}
