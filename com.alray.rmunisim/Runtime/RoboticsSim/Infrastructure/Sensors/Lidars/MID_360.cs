
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars
{

    public class Mid360 : ILidar
    {
        public IEventUpdateBinder<Transform>.BinderContext? Context { get; set; }

        Action<PointCloudData> Processors;
        public void AddDataProcessor(Action<PointCloudData> processor) => Processors += processor;

        int count = 0;

        Queue<System.Numerics.Vector3> pointsFrame = new();

        void RayCast(int rayCount)
        {

            if (rayCount <= 0)
                return;
            var commands = new NativeArray<RaycastCommand>(rayCount, Allocator.TempJob);
            var results = new NativeArray<RaycastHit>(rayCount, Allocator.TempJob);

            // 构造查询参数（层和触发器选项）
            var queryParams = new QueryParameters(
                layerMask: LayerMask.GetMask("Default"),
                hitTriggers: QueryTriggerInteraction.Ignore
            );
            // 构造射线命令（这里随便举例，实际可替换为你的逻辑）
            var origin = Context.HasValue ? Context.Value.BindTarget.position : Vector3.zero;
            var rotation = Context.HasValue ? Context.Value.BindTarget.rotation : Quaternion.identity;
            for (int i = 0; i < rayCount; i++)
            {
                // 随机方位角 [0, 2π)
                float azimuth = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

                // 随机仰角 [52°, -7°]，注意要转弧度
                float elevation = UnityEngine.Random.Range(-7f, 52f) * Mathf.Deg2Rad;

                // 球坐标转方向向量
                float cosE = Mathf.Cos(elevation);
                Vector3 direction = new(
                    Mathf.Cos(azimuth) * cosE,
                    Mathf.Sin(elevation),
                    Mathf.Sin(azimuth) * cosE
                );

                commands[i] = new RaycastCommand(origin, rotation * direction, queryParams, 60f);
            }

            // 调度批量射线检测
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 64);
            while (!handle.IsCompleted)
                continue;
            handle.Complete();
            // 处理结果
            for (int i = 0; i < rayCount; i++)
            {
                if (results[i].collider == null)
                    continue;
                pointsFrame.Enqueue(new(results[i].point.x, results[i].point.y, results[i].point.z));
            }

            commands.Dispose();
            results.Dispose();

        }

        IEnumerator IEventUpdateBinder<Transform>.UpdateBinder()
        {
            while (true)
            {
                // 计算本帧需要的射线数量
                int rayCount = Mathf.RoundToInt(200000f * Time.deltaTime);
                int rayCountTemp = rayCount;
                if (rayCount + count > 20000)
                {
                    rayCount = 20000 - count;
                    RayCast(rayCount);
                    if (Processors is not null)
                        Processors(new PointCloudData(pointsFrame.ToArray()));
                    pointsFrame.Clear();
                    count = rayCountTemp - rayCount;
                    RayCast(count);
                }
                else
                {
                    RayCast(rayCount);
                    count += rayCount;
                }

                // 等待下一帧
                yield return null;
            }
        }
    }
}