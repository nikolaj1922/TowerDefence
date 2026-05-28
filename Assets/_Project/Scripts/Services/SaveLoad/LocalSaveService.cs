using Cysharp.Threading.Tasks;
using _Project.Scripts.Infrastructure.Constants;
using UnityEngine;

namespace _Project.Scripts.Services.SaveLoad
{
    public class LocalSaveService : ISaveService
    {
        public UniTask Initialize() => UniTask.CompletedTask;

        public UniTask<PlayerProgress> Load() =>
            UniTask.FromResult(PlayerPrefs.HasKey(GameConstants.PLAYER_PROGRESS) 
                ? GetProgress()
                : InitProgress());

        public UniTask Save(string progressJson)
        {
            PlayerPrefs.SetString(GameConstants.PLAYER_PROGRESS, progressJson);
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }
        
        public PlayerProgress GetProgress()
        {
            if (PlayerPrefs.HasKey(GameConstants.PLAYER_PROGRESS))
            {
                string json = PlayerPrefs.GetString(GameConstants.PLAYER_PROGRESS);
                return JsonUtility.FromJson<PlayerProgress>(json);
            }

            return null;
        }

        private PlayerProgress InitProgress()
        {
            return new PlayerProgress
            {
                upgrades = new PlayerUpgrades()
            };
        }
    }
}