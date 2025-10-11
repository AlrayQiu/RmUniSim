
using com.alray.rmunisim.Contracts.DTOs;

namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface ILidar<T> :
            IExecutable,
            IInitializable<T>,
            IPushSensor<PointCloudData>
            where T : IRayCaster
    {

    }

}