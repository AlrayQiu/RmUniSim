
using System;

namespace com.alray.rmunisim.Contracts.Interfaces
{

    public interface IPollingSensor<TSensorData>
    {
        public TSensorData RequestData();
    }

    public interface IPushSensor<TSensorData>
    {
        public void AddDataProcessor(Action<TSensorData> processor);
    }
}