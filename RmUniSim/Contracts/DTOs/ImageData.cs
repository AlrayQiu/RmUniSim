using System;
using System.Numerics;
using System.Runtime.InteropServices;
namespace com.alray.rmunisim.Contracts.DTOs
{
    public interface IImageData { };
    [Serializable]
    public unsafe class Image<T> : IDisposable where T : unmanaged, IImageData
    {
        internal Image(int sizeX, int sizeY)
        {
            this.data = Marshal.AllocHGlobal(sizeX * sizeY * sizeof(T));
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.stride = sizeX * sizeof(T);
        }
        private readonly nint data;

        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int stride;

        private bool _disposed = false;
        private void Free()
        {
            if (_disposed)
                return;
            _disposed = true;
            Marshal.FreeHGlobal(data);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Free();
        }
        ~Image()
        {
            Free();
        }
    }

    public static class ImageFactory
    {
        public static Image<Channel3<double>> RGB64(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel4<double>> RGBA64(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel1<double>> Depth64(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel3<float>> RGB32(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel4<float>> RGBA32(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel1<float>> Depth32(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel3<byte>> RGB8(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel3<byte>> RGBA8(int sizeX, int sizeY) => new(sizeX, sizeY);
        public static Image<Channel1<byte>> Depth8(int sizeX, int sizeY) => new(sizeX, sizeY);
    }

    public struct Channel1<T> : IImageData where T : unmanaged
    {
        public Channel1(T data) { Data = data; }

        public T Data;
    }

    public struct Channel3<T> : IImageData where T : unmanaged
    {
        public Channel3(T d1, T d2, T d3) { D1 = d1; D2 = d2; D3 = d3; }

        public T D1, D2, D3;
    }

    public struct Channel4<T> : IImageData where T : unmanaged
    {
        public Channel4(T d1, T d2, T d3, T d4) { D1 = d1; D2 = d2; D3 = d3; D4 = d4; }

        public T D1, D2, D3, D4;
    }
}