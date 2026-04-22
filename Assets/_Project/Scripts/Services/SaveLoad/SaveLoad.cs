using UnityEngine;
using _Project.Scripts.Infrastructure.Constants;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        public PlayerProgress PlayerProgress { get; private set; }
        
        public void SaveProgress()
        {
            string json = JsonUtility.ToJson(PlayerProgress);
            PlayerPrefs.SetString(GameConstants.PLAYER_PROGRESS, json);
            PlayerPrefs.Save();
        }

        public UniTask LoadProgress()
        {
            if (PlayerPrefs.HasKey(GameConstants.PLAYER_PROGRESS))
            {
                string json = PlayerPrefs.GetString(GameConstants.PLAYER_PROGRESS);
                PlayerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            }
            else
            {
                InitProgress();
            }
            
            return UniTask.CompletedTask;
        }

        private void InitProgress()
        {
            PlayerProgress = new PlayerProgress
            {
                upgrades = new PlayerUpgrades()
            };
        }
    }
}