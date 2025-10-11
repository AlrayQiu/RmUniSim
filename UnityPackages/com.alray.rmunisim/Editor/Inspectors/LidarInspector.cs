
using System.Collections.Generic;
using System.Linq;
using com.alray.rmunisim.Applications.Helper;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using com.alray.rmunisim.RoboticsSim.Infrastructure.Controllers.Chassis;
using com.alray.rmunisim.Services;
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
            int index = Mathf.Max(0, System.Array.IndexOf(LidarService<RayCaster>.Lidars, selector.LidarType));
            index = EditorGUILayout.Popup("LidarType", index, LidarService<RayCaster>.Lidars);
            selector.LidarType = LidarService<RayCaster>.Lidars[index];


            // 渲染开关
            selector.EnableRender = EditorGUILayout.Toggle("Enable Render", selector.EnableRender);

            // 折叠区域
            if (selector.EnableRender)
            {
                if (ShaderSelector.AllShaders[VisualizationType.PointCloud].Length <= 0)
                    return;

                int shaderIndex = Mathf.Max(
                    0,
                   selector.Material ? System.Array.IndexOf(
                        ShaderSelector.AllShaders[VisualizationType.PointCloud],
                        selector.Material.shader.name) : 0);
                shaderIndex = Mathf.Clamp(shaderIndex, 0, ShaderSelector.AllShaders[VisualizationType.PointCloud].Length - 1);
                shaderIndex = EditorGUILayout.Popup("Shader", shaderIndex, ShaderSelector.AllShaders[VisualizationType.PointCloud]);

                Shader shader = Shader.Find(ShaderSelector.AllShaders[VisualizationType.PointCloud][shaderIndex]);
                selector.Material = (selector.Material == null || selector.Material.shader != shader) ? new Material(shader) : selector.Material;
                ShaderInspectorHelper.Draw(shader, selector.Material);

            }

            selector.EnablePublish = EditorGUILayout.Toggle("Enable Publisher", selector.EnablePublish);
            // 折叠区域
            if (selector.EnablePublish)
            {
                int publisherIndex = Mathf.Max(
                    0,
                    System.Array.IndexOf(CommMiddlewareService<PointCloudData>
                        .PublisherMiddlewares,
                        selector.PublisherType));
                publisherIndex = EditorGUILayout.Popup("Publisher", publisherIndex, CommMiddlewareService<PointCloudData>.PublisherMiddlewares);
                string publisherType = CommMiddlewareService<PointCloudData>.PublisherMiddlewares[publisherIndex];

                selector.publisher =
                    selector.PublisherType != publisherType
                    ? selector.commCache.Get(publisherType) : selector.publisher;
                selector.PublisherType = publisherType;
                if (selector.publisher != null)
                {
                    SerializedProperty publisherProp = serializedObject.FindProperty("publisher");
                    EditorGUILayout.PropertyField(publisherProp, new GUIContent($"{selector.publisher.GetType().Name} settings"), true);
                }
                serializedObject.ApplyModifiedProperties();
            }

            // 保存修改
            if (GUI.changed)
            {
                EditorUtility.SetDirty(selector);
            }
        }
    }
}