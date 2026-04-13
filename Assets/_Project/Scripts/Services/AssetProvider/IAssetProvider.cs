using UnityEngine;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<T> Load<T>(string path) where T : Object;
        UniTask<T[]> LoadAll<T>(string path) where T : Object;
    }
}