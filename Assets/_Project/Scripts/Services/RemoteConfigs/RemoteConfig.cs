using System;

namespace _Project.Scripts.Services.RemoteConfigs
{
    [Serializable]
    public class RemoteConfig<T>
    {
        public T[] items;
    }
}