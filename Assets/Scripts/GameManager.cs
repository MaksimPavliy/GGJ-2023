using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying;

    public static GameManager instance;
    public GameMode gameMode = GameMode.StoryMode;
    public NetworkMode networkMode = NetworkMode.Local;
    public float levelTimer;

    public static UnityAction OnTimerRunOut;

    private void Awake()
    {
        StartCoroutine(DecreaseLeveTimer());

        if (instance == null)
        { 
            instance = this; 
        }

        else if (instance == this)
        { 
            Destroy(gameObject); 
        }
    }

    private IEnumerator DecreaseLeveTimer()
    {
        while(levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        /*OnTimerRunOut?.Invoke();*/
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
