namespace _Project.Scripts.Services.SaveLoad
{
    public interface ILocalSaveService
    {
        PlayerProgress Load();
        void Save(string progressJson);
        PlayerProgress GetProgress();
    }
}