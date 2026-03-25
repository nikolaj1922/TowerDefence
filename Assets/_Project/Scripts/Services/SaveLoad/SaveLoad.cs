using UnityEngine;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        public PlayerProgress PlayerProgress { get; private set; }
        
        public void AddMetaCoins(int amount)
        {
            PlayerProgress.metaCoinsCount += amount;
            SaveProgress();
        }
        
        public void SaveProgress()
        {
            string json = JsonUtility.ToJson(PlayerProgress);
            PlayerPrefs.SetString(GameConstants.PLAYER_PROGRESS, json);
            PlayerPrefs.Save();
        }

        public void LoadProgress()
        {
            if (PlayerPrefs.HasKey(GameConstants.PLAYER_PROGRESS))
            {
                string json = PlayerPrefs.GetString(GameConstants.PLAYER_PROGRESS);
                PlayerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            }
            else
            {
                PlayerProgress = new PlayerProgress
                {
                    metaCoinsCount = 0
                };
            }
        }
    }
}