
namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface IPublisher<T> : IInitializable where T : struct
    {
        void Publish(T data);
    }

}