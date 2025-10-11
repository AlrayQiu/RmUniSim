
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;

namespace com.alray.rmunisim.RoboticsSim.Infrastructure.Sensors.Lidars
{

    public class Mid360<T> : ILidar<T> where T : IRayCaster
    {

        private int count = 0;
        private T _raycaster;
        private event Action<PointCloudData> Processor = null;

        private readonly Random rand = new();
        private readonly Queue<Vector3> _pointFrame = new();

        void IPushSensor<PointCloudData>.AddDataProcessor(Action<PointCloudData> processor) => Processor += processor;

        void IInitializable<T>.Init(T init_data)
        {
            _raycaster = init_data;
        }


        void RayCast(int rayCount)
        {

            if (rayCount <= 0)
                return;
            var commands = new Ray[rayCount];

            // 构造射线命令（这里随便举例，实际可替换为你的逻辑）
            for (int i = 0; i < rayCount; i++)
            {
                // 随机方位角 [0, 2π)
                float azimuth = (float)rand.NextDouble() * MathF.PI * 2f;

                // 随机仰角 [52°, -7°]，注意要转弧度
                float elevation = ((float)rand.NextDouble() * 59f - 7f) * MathF.PI / 180f;

                // 球坐标转方向向量
                float cosE = MathF.Cos(elevation);
                Vector3 direction = new(
                    MathF.Cos(azimuth) * cosE,
                    MathF.Sin(elevation),
                    MathF.Sin(azimuth) * cosE
                );

                commands[i].Direction = direction;
                commands[i].Distance = 60f;
            }

            foreach (var point in _raycaster.Cast(commands).data)
                _pointFrame.Enqueue(point);
        }
        void IExecutable.Update(float deltaTime)
        {
            int rayCount = (int)MathF.Round(200000f * deltaTime);
            int rayCountTemp = rayCount;
            if (rayCount + count > 20000)
            {
                rayCount = 20000 - count;
                RayCast(rayCount);
                if (Processor is not null)
                    Processor(new PointCloudData(_pointFrame.ToArray()));
                _pointFrame.Clear();
                count = rayCountTemp - rayCount;
                RayCast(count);
            }
            else
            {
                RayCast(rayCount);
                count += rayCount;
            }

        }
    }
}