namespace _Project.Scripts.Services.Upgrade
{
    public interface IUpgradeService
    {
        float GetUpgradeMultiplier(string upgradeId);
        int GetUpgradeLevel(string id);
        void SetUpgradeLevel(string id, int level);
    }
}