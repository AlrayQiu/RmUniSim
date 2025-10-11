

using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
namespace com.alray.rmunisim.Visualization.Infrastructure
{

    public class PointCloudDrawer : IDrawer<IPushSensor<PointCloudData>>
    {
        private Material material;
        private IPushSensor<PointCloudData> sensor;
        ComputeBuffer pointBuffer;

        public PointCloudDrawer()
        {
        }
        public IPollingUpdateBinder<Transform>.BinderContext? Context { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void UpdateBuffer(PointCloudData points)
        {
            var pointCount = points.data.Length;
            if (pointCount <= 0)
            {
                pointBuffer?.Dispose();
                pointBuffer = null;
                return;
            }

            pointBuffer?.Dispose();
            pointBuffer = new ComputeBuffer(pointCount, sizeof(float) * 3);
            pointBuffer.SetData(points.data);

            material.SetBuffer("_PointBuffer", pointBuffer);

        }

        public void Draw()
        {
            if (pointBuffer is not null)
                Graphics.DrawProcedural(material, new Bounds(Vector3.zero, Vector3.one * 100f), MeshTopology.Points, pointBuffer.count);
        }

        void IDrawer<IPushSensor<PointCloudData>>.Initialize(Material material, IPushSensor<PointCloudData> sensor)
        {
            this.material = material;
            this.sensor = sensor;
            sensor.AddDataProcessor(
               UpdateBuffer
            );
        }
    }
}