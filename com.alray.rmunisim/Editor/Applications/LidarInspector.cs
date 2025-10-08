
using System;
using System.Collections.Generic;
using System.Linq;
using com.alray.rmunisim.Applications;
using com.alray.rmunisim.Applications.Helper;
using com.alray.rmunisim.RoboticsSim.Domain;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars;
using com.alray.rmunisim.Visualization.Domain;
using UnityEditor;
using UnityEngine;

namespace com.alray.rmunisim.Applications
{

    [CustomEditor(typeof(Lidar))]
    public class LidarInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var selector = (Lidar)target;
            int index = Mathf.Max(0, System.Array.IndexOf(LidarFactory.LidarTyps, selector.LidarType));
            index = EditorGUILayout.Popup("LidarType", index, LidarFactory.LidarTyps);
            selector.LidarType = LidarFactory.LidarTyps[index];


            // 渲染开关
            selector.EnableRender = EditorGUILayout.Toggle("Enable Render", selector.EnableRender);

            // 折叠区域
            if (selector.EnableRender)
            {
                if (ShaderSelector.AllShaders[VisualizationType.PointCloud].Length > 0)
                {

                    int shaderIndex = Mathf.Max(
                        0,
                        System.Array.IndexOf(
                            ShaderSelector.AllShaders[VisualizationType.PointCloud],
                            selector.Material.shader.name));
                    shaderIndex = Mathf.Clamp(shaderIndex, 0, ShaderSelector.AllShaders[VisualizationType.PointCloud].Length - 1);
                    shaderIndex = EditorGUILayout.Popup("Shader", shaderIndex, ShaderSelector.AllShaders[VisualizationType.PointCloud]);

                    Shader shader = Shader.Find(ShaderSelector.AllShaders[VisualizationType.PointCloud][shaderIndex]);
                    selector.Material = selector.Material.shader != shader ? new Material(shader) : selector.Material;
                    ShaderInspectorHelper.Draw(shader, selector.Material);
                }

            }

            // 保存修改
            if (GUI.changed)
            {
                EditorUtility.SetDirty(selector);
            }
        }
    }
}