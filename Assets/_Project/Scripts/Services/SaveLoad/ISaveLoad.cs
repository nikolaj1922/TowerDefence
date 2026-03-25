namespace _Project.Scripts.Services.SaveLoad
{
    public interface ISaveLoad
    {
        PlayerProgress PlayerProgress { get; }
        
        void AddMetaCoins(int coins);
        void SaveProgress();
        void LoadProgress();
    }
}