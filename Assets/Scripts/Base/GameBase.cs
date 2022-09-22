using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;

public class GameBase : Singleton<GameBase>
{
    [Header("Base Managers")] public DataManager DataManager;
    public LevelManager LevelManager;
    public MenuManager MenuManager;
    public AudioManager AudioManager;
    public CameraManager CameraManager;
    //public PoolManager PoolManager;

    [Header("Game Stats")] public GameStat GameStat;
    public int Timer;
    public bool ShowFps;
    public bool ShowConsole;
    [Header("Objects")] public GameObject RunTimeConsole;

    protected override async void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif

        base.Awake();
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        GameStat = GameStat.Start;
        RunTimeConsole.SetActive(ShowConsole);

        DataManager = GetComponentInChildren<DataManager>();
        LevelManager = GetComponentInChildren<LevelManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        CameraManager = GetComponentInChildren<CameraManager>();
        MenuManager = GetComponentInChildren<MenuManager>();
        //PoolManager = GetComponentInChildren<PoolManager>();


        await DataManager.Setup();
        //await PoolManager.Setup();
        await MenuManager.Setup();

        await CameraManager.Setup();
        await AudioManager.Setup();

        await LevelManager.Setup();

        Debug.Log("GameBase Setup Complete");
    }

    private void OnEnable()
    {
        EventManager.OnBeforeLoadedLevel += ResetStat;
        EventManager.FirstTouch += StartGame;
    }

    protected override void OnDisable()
    {
        EventManager.OnBeforeLoadedLevel -= ResetStat;
        EventManager.FirstTouch -= StartGame;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else Time.timeScale = 1;
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    float deltaTime = 0.0f;

    void OnGUI()
    {
        if (ShowFps)
            ShowFPS();
    }

    private void ShowFPS()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
#endif

    public void ChangeStat(GameStat stat)
    {
        if (Equals(GameStat, stat))
            return;

        GameStat = stat;
    }

    private void StartGame()
    {
        ChangeStat(GameStat.Start);
        StartCoroutine(Counter());
    }

    private void ResetStat()
    {
        GameStat = GameStat.Start;
        Timer = 0;
    }

    private IEnumerator Counter()
    {
        while (Base.IsPlaying())
        {
            Timer++;
            yield return new WaitForSeconds(1);
        }
    }
}

public static class EventManager
{
    public static Action<GameStat> BeforeFinishGame;
    public static Action<GameStat> FinishGame;

    public static Action NextLevel;
    public static Action RestartLevel;

    public static Action FirstTouch;
    public static Action<bool> OnPause;

    public static Action OnBeforeLoadedLevel;
    public static Action OnAfterLoadedLevel;
}

public static class Base
{
    public static void ChangeStat(GameStat stat)
    {
        GameBase.Instance.ChangeStat(stat);
    }

    public static Transform GetLevelHolder()
    {
        return GameBase.Instance.LevelManager.LevelHolder;
    }

    public static bool IsPlaying()
    {
        return GameBase.Instance.GameStat == GameStat.Playing;
    }

    public static int GetTimer()
    {
        return GameBase.Instance.Timer;
    }

    public async static void FinisGame(GameStat gameStat, float time = 0f)
    {
        if (GameBase.Instance.GameStat == GameStat.Playing |
            GameBase.Instance.GameStat == GameStat.FinishLine)
            GameBase.Instance.ChangeStat(gameStat);

        EventManager.BeforeFinishGame?.Invoke(gameStat);
        await Task.Delay((int)time * 1000);
        if (!Application.isPlaying) return;
        EventManager.FinishGame?.Invoke(gameStat);
    }

    public static void StartGameAddFunc(Action func)
    {
        EventManager.FirstTouch += func;
    }

    public static void NextLevelAddFunc(Action func)
    {
        EventManager.NextLevel += func;
    }

    public static void RestartLevelAddFunc(Action func)
    {
        EventManager.RestartLevel += func;
    }

    public static void FinishGameAddFunc(Action<GameStat> func)
    {
        EventManager.FinishGame += func;
    }
}