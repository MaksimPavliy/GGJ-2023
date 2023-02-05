using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Raven : MonoBehaviour
{
    [SerializeField] private float flyTime;
    [SerializeField] float minDistanceFromPlayers;
    [SerializeField] private AnimationReferenceAsset fly, dive;
    [SerializeField] private float resetDuration;

    public Transform rootPickupAnchor;
    public List<Ridge> ridges;
    public List<Player> players;
    public static UnityAction<Raven> OnScared;
    public Ridge targetRidge;

    private Root pickedRoot;
    private int[] randomXValues = { 18, 19, -13, -15, 17, -14 };
    private bool canBeScared = false;
    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState animationState;
    private Coroutine peckingCoroutine;
    private Vector3 peckOffset;

    void Start()
    {
        Root.OnRootDigged += OnTargetRidgeDigged;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        StartCoroutine(FindRidgeToSteal());
    }

    private void Update()
    {
        if (canBeScared)
        {
            foreach (var player in players)
            {
                if (Vector2.Distance(transform.position, player.transform.position) < minDistanceFromPlayers)
                {
                    StopCoroutine(peckingCoroutine);
                    FlyAwayRandomly();
                    canBeScared = false;
                }
            }
        }
    }

    public void FlyAwayRandomly()
    {
        Debug.Log(pickedRoot);
        
        if (!pickedRoot)
        {
            OnScared?.Invoke(this);
        }
        animationState.SetAnimation(0, fly, true);
        var randomXValue = Random.Range(0, randomXValues.Length);
        var randomY = Random.Range(7, 11);
        var randomX = randomXValues[randomXValue];
        ChangeRotation(new Vector3(randomX, randomY));

        transform.DOMove(new Vector2(randomX, randomY), flyTime)
            .OnComplete(() => StartCoroutine(ResetRaven()));
    }

    private void FlyToEmptyRidge()
    {
        targetRidge.root.StopRotting();
        ChangeRotation(targetRidge.transform.position);
        animationState.SetAnimation(0, fly, true);
        transform.DOMove(targetRidge.transform.position + peckOffset, flyTime)
            .OnComplete(() => peckingCoroutine = StartCoroutine(StartPecking()));
    }


    private IEnumerator FindRidgeToSteal()
    {
        while (!targetRidge && GameManager.instance.isPlaying)
        {
            yield return new WaitForSeconds(1);
            targetRidge = ridges.Find(x => x.root && x.root.rootType != Root.RootType.Sornyak && x.root.HasGrown);
        }
        FlyToEmptyRidge();
    }

    private IEnumerator ResetRaven()
    {
        yield return new WaitForSeconds(resetDuration);
        if (pickedRoot)
        {
            Destroy(pickedRoot.gameObject);
        }
        targetRidge = null;
        StartCoroutine(FindRidgeToSteal());
    }

    private IEnumerator StartPecking()
    {
        canBeScared = true;
        ChangeRotation(targetRidge.transform.position);
        animationState.SetAnimation(0, dive, true);
        yield return new WaitForSeconds(dive.Animation.Duration);
        canBeScared = false;
        pickedRoot = targetRidge.root;
        if(pickedRoot)
        pickedRoot.JumpToRaven(this);
    }

    private void ChangeRotation(Vector3 targetPos)
    {
        if (transform.position.x > targetPos.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            peckOffset = new Vector3(0.6f, 0.2f);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            peckOffset = new Vector3(-0.6f, 0.2f);
        }
    }

    private void OnTargetRidgeDigged(Root root)
    {
        if (targetRidge && targetRidge.root == root)
        {
            StartCoroutine(FindRidgeToSteal());
        }
    }
}
