
namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface ISubscriber<T> : IInitializable where T : struct
    {
        T Subscript();
    }
}