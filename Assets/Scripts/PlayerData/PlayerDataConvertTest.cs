using UnityEngine;
using TMPro;

/// <summary>
/// this class is only for testing, do not use in main game scene
/// </summary>
public class PlayerDataConvertTest : MonoBehaviour
{
    public PlayerDataManager dataManager;
    public PlayerData dummyData1;
    public PlayerData dummyData2;
    public PlayerData dummyData3;

    public TextMeshProUGUI liveDataDisplay;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void LoadDummy1()
    {
        dataManager.playerData = dummyData1;
        liveDataDisplay.text = JsonUtility.ToJson(dataManager.playerData);
    }

    public void LoadDummy2()
    {
        dataManager.playerData = dummyData2;
        liveDataDisplay.text = JsonUtility.ToJson(dataManager.playerData);
    }

    public void LoadDummy3()
    {
        dataManager.playerData = dummyData3;
        liveDataDisplay.text = JsonUtility.ToJson(dataManager.playerData);
    }

    public void LoadPlayerPref()
    {
        string jsonData = EncryptedPlayerPrefs.GetString("playerData");
        dataManager.playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        liveDataDisplay.text = jsonData;
        Debug.Log("player data loaded");
    }

    public void SavePlayerPref()
    {
        string jsonData = JsonUtility.ToJson(dataManager.playerData);
        EncryptedPlayerPrefs.SetString("playerData", jsonData);
        Debug.Log("player data saved");
    }

    public void SavePlayfab()
    {
        dataManager.SaveAllDataToPlayfab();
    }
}
