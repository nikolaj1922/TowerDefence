using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace _Project.Scripts.Services.NetworkChecker
{
    public class NetworkChecker : INetworkChecker
    {
        public async UniTask<bool> CheckNetwork()
        {
            using UnityWebRequest request = UnityWebRequest.Get("https://google.com");

            try
            {
                await request.SendWebRequest().ToUniTask();
                return request.result == UnityWebRequest.Result.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}