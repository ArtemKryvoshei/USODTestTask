using System.Threading.Tasks;


namespace Core.SaveLoadSystem
{
    public interface ISaveLoadSystem
    {
        float CurrentLoadProgress { get; }
        float MaxLoadProgress { get; }

        Task<GameData> LoadGameData(float fakeDelaySeconds = 1f);
        Task SaveData(GameData dataToSave, float fakeDelaySeconds = 1f);
    }
}