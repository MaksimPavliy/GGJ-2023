using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character : MonoBehaviour
{
    [SerializeField] private List<Root> roots;
    [SerializeField] private Image wantedRoot;
    [SerializeField] private Image timerImage;
    [SerializeField] private float timer;
    [SerializeField] private bool leftCharacter;
    [SerializeField] private Canvas mainCanvas;

    [HideInInspector] public Root.RootType requiredRootType;

    private Coroutine getRootCoroutine;
    private Coroutine timerFillCoroutine;

    void Start()
    {
        Player.OnRootGiven += OnRootAquired;
        getRootCoroutine = StartCoroutine(AskForRoot());
    }

    private IEnumerator AskForRoot()
    {
        while (GameManager.instance.isPlaying)
        {
            if (timerFillCoroutine != null)
            {
                StopCoroutine(timerFillCoroutine);
            }
            timerFillCoroutine = StartCoroutine(FillTimerImage());
            Root root = Root.SpawnRootWithChance(roots);
            requiredRootType = root.rootType;
            wantedRoot.sprite = root.GetComponent<SpriteRenderer>().sprite;
           
            yield return new WaitForSeconds(timer);
        }
    }

    private IEnumerator FillTimerImage()
    {
        timerImage.fillAmount = 0;
        float normalizedFillTimer = 0;

        while (normalizedFillTimer <= 1f)
        {           
            timerImage.fillAmount = normalizedFillTimer;
            normalizedFillTimer += Time.deltaTime / timer;
            yield return null;
        }
    }

    private void OnRootAquired(Character character, Root root)
    {
        GameManager.instance.UpdateRootCounter();

        if(root.rootType == requiredRootType)
        {
            //play good animation
        }
        else
        {
            //bd animation
        }

        if (leftCharacter == character.leftCharacter)
        {
            mainCanvas.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            StopCoroutine(getRootCoroutine);
            getRootCoroutine = StartCoroutine(AskForRoot());
        }
    }
}
