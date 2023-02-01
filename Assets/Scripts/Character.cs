using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private List<Sprite> rootSprites;
    [SerializeField] private Image wantedRoot;
    [SerializeField] private Image timerImage;
    [SerializeField] private float timer;

    void Start()
    {
        StartCoroutine(AskForRoot());
    }

    private IEnumerator AskForRoot()
    {
        while (GameManager.instance.levelTimer > 0)
        {
            int rand = Random.Range(0, rootSprites.Count);
            Sprite root = rootSprites[rand];
            wantedRoot.sprite = root;

            yield return new WaitForSeconds(timer);
        }
    }
}
