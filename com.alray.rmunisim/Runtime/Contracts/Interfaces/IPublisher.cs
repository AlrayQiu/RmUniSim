
namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface IPublisher<T> where T : struct
    {
        void Publish(T data);
    }
}