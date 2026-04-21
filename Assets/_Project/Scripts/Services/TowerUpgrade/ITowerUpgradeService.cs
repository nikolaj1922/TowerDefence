namespace _Project.Scripts.Services.TowerUpgrade
{
    public interface ITowerUpgradeService
    {
        float GetUpgradeMultiplier(string upgradeId);
        int GetUpgradeLevel(string id);
        void SetUpgradeLevel(string id, int level);
    }
}