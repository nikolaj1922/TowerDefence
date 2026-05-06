using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Constants;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Scripts.Services.SaveLoad
{
    public class RemoteSaveService : IRemoteSaveService
    {
        public async UniTask Initialize()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        public async UniTask Save(string progressJson)
        {
            var playerData = new Dictionary<string, object>{
                {GameConstants.PLAYER_PROGRESS, progressJson}
            };
            
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
            Debug.Log($"Saved data {string.Join(',', playerData)}");
        }
        
        public async UniTask<PlayerProgress> Load()
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
                GameConstants.PLAYER_PROGRESS
            });

            if (!playerData.TryGetValue(GameConstants.PLAYER_PROGRESS, out Item item)) {
                Debug.Log($"Player progress JSON not found!");
                return null;
            }

            string json = item.Value.GetAsString();
            
            Debug.Log($"Loaded remote progress: {json}");

            return JsonUtility.FromJson<PlayerProgress>(json);
        }
    }
}