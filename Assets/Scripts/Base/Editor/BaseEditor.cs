using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class BaseEditor : OdinEditorWindow
{
    [MenuItem("Base/Editor")]
    private static void OpenWindow()
    {
        GetWindow<BaseEditor>().Show();
    }

    [InfoBox("F1 = GAME PAUSE // GAME RESUME")] [Title("Data System")]
    public Data playerData;

    [Button]
    public void ClearData()
    {
        DataExtension.ClearData();
        playerData = DataExtension.GetData();
    }

    [Button]
    public void SaveData()
    {
        DataExtension.SaveData(playerData);
    }


    [Title("Level System")]
    [Button]
    public void NextLevel()
    {
        EventManager.NextLevel?.Invoke();
    }

    [Button]
    public void RestartLevel()
    {
        EventManager.RestartLevel?.Invoke();
    }

    [Button]
    public void WinLevel()
    {
        EventManager.FinishGame?.Invoke(GameStat.Win);
    }

    [Button]
    public void LoseLevel()
    {
        EventManager.FinishGame?.Invoke(GameStat.Lose);
    }

    [Title("Settings")]
    [Button]
    private void GoPlayerControllerData()
    {
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/PlayerData.asset");
    }

    [Button]
    private void GoGameData()
    {
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/GameData.asset");
    }

    [Title("Game Datas")] public GameData GameData;


    protected override void OnEnable()
    {
        base.OnEnable();
        playerData = DataExtension.GetData();
        // if (Application.isPlaying)
        //     RefreshPools();
    }

    // [Title("Pool System")]
    // public string[] PoolNames;
    //
    // [Button]
    // public void ClearPool()
    // {
    //     RefreshPools();
    // }

    // private void RefreshPools()
    // {
    //     PoolNames = PoolManager.Instance.GetPoolNames();
    // }
}