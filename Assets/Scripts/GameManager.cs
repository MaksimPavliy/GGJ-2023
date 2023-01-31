using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying;

    public static GameManager instance;
    public bool isLocalGame;

    private void Awake()
    {
        isPlaying = true;     

        if (instance == null)
        { 
            instance = this; 
        }

        else if (instance == this)
        { 
            Destroy(gameObject); 
        }
    }
}
