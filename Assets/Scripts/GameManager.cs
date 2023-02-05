using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying;
    public List<Level> levels;

    public static GameManager instance;
    public GameMode gameMode = GameMode.StoryMode;
    public NetworkMode networkMode = NetworkMode.Local;

    private int collectedRootsAmount;

    public static UnityAction OnCollectedCounterUpdated;
    public static UnityAction OnWin;
    public static UnityAction OnLose;
    public int currentLevelId = 0;
    [HideInInspector] public Level curLevel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        LoadLevel();
    }

    private void Start()
    {
        isPlaying = true;
    }

    public void UpdateRootCounter()
    {
        if (collectedRootsAmount < curLevel.requiredRootsAmount)
        {
            OnCollectedCounterUpdated?.Invoke();
        }
        collectedRootsAmount++;
        if (collectedRootsAmount >= curLevel.requiredRootsAmount)
        {
            WinGame();
        }
    }

    public void LoadLevel()
    {
        if (currentLevelId < levels.Count)
        {
            curLevel = Instantiate(levels.Find(x => x.levelId == currentLevelId));
        }
    }

    public void WinGame()
    {
        isPlaying = false;
        OnWin?.Invoke();
        currentLevelId++;
    }

    public void LoseGame()
    {
        isPlaying = false;
        OnLose?.Invoke();
    }

    private void SetGameMode(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }

    private void SetNetworkMode(NetworkMode networkMode)
    {
        this.networkMode = networkMode;
    }

    public enum GameMode
    {
        StoryMode,
        Versus
    }
    public enum NetworkMode
    {
        Local,
        Multiplayer
    }
}
