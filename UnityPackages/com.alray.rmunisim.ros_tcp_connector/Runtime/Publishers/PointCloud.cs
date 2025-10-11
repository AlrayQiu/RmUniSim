

using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using com.alray.rmunisim.Contracts.DTOs;
using com.alray.rmunisim.Contracts.Interfaces;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace com.alray.rmunisim.middlewares.ros_tcp_connector
{
    using IPub = IPublisher<PointCloudData>;

    static class PointCloud
    {
        /// <summary>
        /// std_msgs/Float32ArrayMsg
        /// </summary>
        [Serializable]
        class ToFloat32MultiArray : IPub
        {
            ROSConnection ros = null;
            public string TopicName;

            public void Init()
            {
                ros = ROSConnection.GetOrCreateInstance();
                ros.RegisterPublisher<Float32Msg>(TopicName);
            }

            public unsafe void Publish(PointCloudData data)
            {
                uint N = (uint)data.data.Length;
                float[] dataRaw = new float[N];
                fixed (void* ptr_src = data.data)
                fixed (void* ptr_des = dataRaw)
                    Buffer.MemoryCopy(ptr_src, ptr_des, (int)(N * sizeof(Vector3)), (int)(N * sizeof(Vector3)));
                RosMessageTypes.Std.Float32MultiArrayMsg float32MultiArray = new(
                    layout: new MultiArrayLayoutMsg(
                        dim: new MultiArrayDimensionMsg[]
                        {
                            new()
                            {
                                label = "points",     // 表示有多少个 Vector3
                                size = N,             // Vector3 的数量
                                stride = 3            // 每个 Vector3 占 3 个 float
                            },
                            new()
                            {
                                label = "components", // 表示每个 Vector3 有多少个分量
                                size = 3,             // x, y, z
                                stride = 1            // 每个分量占 1 个 float
                            }
                        },
                        data_offset: 0

                ), dataRaw);
                ros.Publish(TopicName, float32MultiArray);
            }
        }

        /// <summary>
        /// sensor_msgs/PointCloud2Msg
        /// </summary>
        [Serializable]
        class ToPointCloud2 : IPub
        {
            ROSConnection ros = null;

            public string TopicName;
            public string FrameID;
            public TimeType timeType = TimeType.UTC;

            public void Init()
            {
                ros = ROSConnection.GetOrCreateInstance();
                ros.RegisterPublisher<PointCloud2Msg>(TopicName);
            }

            public unsafe void Publish(PointCloudData data)
            {
                uint N = (uint)data.data.Length;

                DateTime now = DateTime.UtcNow;
                TimeSpan span = now - new DateTime(1970, 1, 1);

                int sec = (int)span.TotalSeconds;
                uint nanosec = (uint)((span.TotalSeconds - sec) * 1_000_000_000);
                byte[] dataRaw = new byte[N * sizeof(Vector3)];
                fixed (void* ptr_src = data.data)
                fixed (void* ptr_des = dataRaw)
                    Buffer.MemoryCopy(ptr_src, ptr_des, (int)(N * sizeof(Vector3)), (int)(N * sizeof(Vector3)));
                PointCloud2Msg pointCloud2Msg = new(

                    header: new HeaderMsg
                    {
                        frame_id = FrameID,
                        stamp = new TimeMsg
                        {
                            nanosec = nanosec,
                            sec = sec
                        }
                    },
                    is_bigendian: false,
                    data: dataRaw,
                    point_step: (uint)sizeof(Vector3),
                    width: N,
                    fields: new PointFieldMsg[]
                    {
                        new ("x", 0, PointFieldMsg.FLOAT32, 1),
                        new ("y", 4, PointFieldMsg.FLOAT32, 1),
                        new ("z", 8, PointFieldMsg.FLOAT32, 1)
                    },
                    is_dense: true,
                    row_step: (uint)(N * sizeof(Vector3)),
                    height: 1
                );
                ros.Publish(TopicName, pointCloud2Msg);
            }

        }
    }
}