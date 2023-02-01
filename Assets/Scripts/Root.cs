using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private Transform bottomPrefab;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private float rotDuration = 5;
    [SerializeField] private List<SpriteRenderer> renderers;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;
    public static UnityAction<Root> OnRootRotten;

    public Vector3 spawnOffset;
    public float growDuration;
    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    private int numOfJumps = 1;
    private bool hasGrown = false;
    private Coroutine rotCorotine;

    private void Start()
    {
        if (rootType != RootType.ObstacleRoot)
        {
            StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow()
    {
        transform.DOScale(1, growDuration);
        yield return new WaitForSeconds(growDuration);
        hasGrown = true;

        rotCorotine = StartCoroutine(Rot());
    }

    private IEnumerator Rot()
    {
        yield return new WaitForSeconds(rotDuration);
        float alpha = 1;
        float startingRoat = rotDuration;

        while (alpha > 0)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, alpha);
            }
            startingRoat -= Time.deltaTime;
            alpha = startingRoat / rotDuration;
            yield return new WaitForEndOfFrame();
        }
        OnRootRotten?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void JumpToPlayer(Player player)
    {
        StopCoroutine(rotCorotine);
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, 1);
        }
        Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y + carryOffset);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => player.SetState(Player.PlayerState.Carrying));
        transform.SetParent(player.transform);
        bottomPrefab.position = new Vector3(bottomPrefab.position.x, bottomPrefab.position.y, -1);
    }

    public void JumpToPot(Vector3 targetPos)
    {
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => AddRootToPot());
    }

    private void AddRootToPot()
    {       
        OnRootAddedToPot?.Invoke(this);
    }

    public enum RootType
    {
        Carrot,
        Potato,
        ObstacleRoot
    }
}
