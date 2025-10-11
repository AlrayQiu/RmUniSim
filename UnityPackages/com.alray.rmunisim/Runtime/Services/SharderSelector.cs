
using System;
using System.Collections.ObjectModel;
using System.Linq;
using com.alray.rmunisim.Visualization.Domain;
using UnityEditor;

namespace com.alray.rmunisim.Services
{
    public static class ShaderSelector
    {
        public readonly static ReadOnlyDictionary<VisualizationType, ShaderInfo[]> AllShaderInfos = new(
             Enum.GetValues(typeof(VisualizationType))
             .Cast<VisualizationType>()
             .ToDictionary(
                 shaderType => shaderType,
                 shaderType =>
                     ShaderUtil.GetAllShaderInfo()
                     .Where(info => info.name.StartsWith($"RmUniSim/{shaderType}"))
                     .ToArray()
         ));

        public readonly static ReadOnlyDictionary<VisualizationType, string[]> AllShaders = new(
             Enum.GetValues(typeof(VisualizationType))
             .Cast<VisualizationType>()
             .ToDictionary(
                 shaderType => shaderType,
                 shaderType =>
                     ShaderUtil.GetAllShaderInfo()
                     .Select(info => info.name)
                     .Where(name => name.StartsWith($"RmUniSim/{shaderType}"))
                     .ToArray()
         ));

    }
}