
namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface ISubscriber<T> where T : struct
    {
        T Subscript(T data);
    }
}