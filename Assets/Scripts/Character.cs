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
    [SerializeField] List<Image> xSigns;

    [HideInInspector] public Root.RootType requiredRootType;

    private Coroutine getRootCoroutine;
    private Coroutine timerFillCoroutine;
    private int xCounter = 0;
    public AudioSource HappySound;
    public AudioSource AngrySound;

    void Start()
    {
        GameManager.OnWin += OnLevelEnded;
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
            xSigns[xCounter].color = Color.red; 
            UpdateXCounter();
            //palyBadAnimation;
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
        if (leftCharacter == character.leftCharacter)
        {
            if (root.rootType == requiredRootType || root.rootType == Root.RootType.Buryak)
            {
                //play good animation
                /*HappySound.Play();*/
            }
            else
            {
                /*AngrySound.Play();*/
                xSigns[xCounter].color = Color.red;
                UpdateXCounter();
                //bad animation
            }
            GameManager.instance.UpdateRootCounter();
            mainCanvas.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            StopCoroutine(getRootCoroutine);
            getRootCoroutine = StartCoroutine(AskForRoot());
        }
    }

    private void UpdateXCounter()
    {
        xCounter++;

        if (xCounter >= 3)
        {
            GameManager.instance.LoseGame();
        }
    }

    private void OnLevelEnded()
    {
        mainCanvas.gameObject.SetActive(false);
    }
}
