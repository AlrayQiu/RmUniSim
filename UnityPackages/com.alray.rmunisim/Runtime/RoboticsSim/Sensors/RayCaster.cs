

using System;
using System.Linq;
using System.Threading.Tasks;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace com.alray.rmunisim.RoboticsSim.Infrastructure.Controllers.Chassis
{
    public class RayCaster : IRayCaster
    {
        public RayCaster(Transform transform)
        {
            _transform = transform;
        }

        readonly Transform _transform;
        public PointCloudData Cast(Contracts.DTOs.Ray[] rays)
        {

            if (rays.Length <= 0)
                return new();
            var commands = new NativeArray<RaycastCommand>(rays.Length, Allocator.TempJob);
            var results = new NativeArray<RaycastHit>(rays.Length, Allocator.TempJob);
            // 构造查询参数（层和触发器选项）
            var queryParams = new QueryParameters(
                layerMask: LayerMask.GetMask("Default"),
                hitTriggers: QueryTriggerInteraction.Ignore
            );
            // 构造射线命令（这里随便举例，实际可替换为你的逻辑）
            _transform.GetPositionAndRotation(out var origin, out var rotation);
            Parallel.For(0, rays.Length, i =>
                commands[i] = new RaycastCommand(
                    origin,
                    rotation
                        * new Vector3(rays[i].Direction.X, rays[i].Direction.Y, rays[i].Direction.Z),
                    queryParams,
                    rays[i].Distance)
            );

            // 调度批量射线检测
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 64);
            while (!handle.IsCompleted)
                continue;

            handle.Complete();
            var data = results
                        .AsParallel()
                        .WithDegreeOfParallelism(Environment.ProcessorCount)
                        .Where(p => p.distance != 0)
                        .Select(p => new System.Numerics.Vector3(p.point.x, p.point.y, p.point.z))
                        .ToArray();
            var pointCloudData = new PointCloudData(data);

            commands.Dispose();
            results.Dispose();

            return pointCloudData;

        }
    }
}