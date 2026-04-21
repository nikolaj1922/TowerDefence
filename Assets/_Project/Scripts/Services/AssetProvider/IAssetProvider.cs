using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<T> Load<T>(AssetReference reference) where T : class;
        UniTask<T[]> LoadByLabel<T>(string label);
        UniTask<GameObject> Instantiate(AssetReferenceGameObject reference, Transform parent = null);
        void Release(AssetReference reference);
        void ReleaseInstance(GameObject reference);
        void Clear();
    }
}