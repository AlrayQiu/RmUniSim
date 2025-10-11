
namespace com.alray.rmunisim.Contracts.Interfaces
{
    public interface IInitializable
    {
        void Init();
    }

    public interface IInitializable<T>
    {
        void Init(T init_data);
    }
    public interface IInitializable<T, F>
    {
        void Init(T init_data, T init_data2);
    }
    public interface IInitializable<T, F, V>
    {
        void Init(T init_data, F init_data2, V init_data3);
    }
}