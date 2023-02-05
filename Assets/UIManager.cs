using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas tutorialCanvas;
    public Canvas LoseCanvas;
    public Canvas WinCanvas;

    public static UIManager instance;

    void Awake()
    {
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
