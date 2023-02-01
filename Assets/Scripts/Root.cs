using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private float rotDuration = 5;
    [SerializeField] private List<SpriteRenderer> renderers;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;
    public static UnityAction<Root> OnRootRotten;

    public Vector3 spawnOffset;
    public Vector3 growScale;
    public float growDuration;
    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    private int numOfJumps = 1;
    private bool hasGrown = false;
    private Coroutine rotCoroutine;

    private void Start()
    {
        if (rootType != RootType.ObstacleRoot)
        {
            StartCoroutine(Grow());
        }
    }

    //root still rot after pickup?
    //obstacles can fully restric access

    private IEnumerator Grow()
    {
        //add scale bounce on grow
        transform.DOScale(growScale, growDuration)
            .OnComplete(() => transform.DOScale(growScale + new Vector3(0.1f, 0.1f, 0.1f), 0.15f).SetLoops(2, LoopType.Yoyo));
        yield return new WaitForSeconds(growDuration);
        hasGrown = true;

        rotCoroutine = StartCoroutine(Rot());
    }

    private IEnumerator Rot()
    {
        yield return new WaitForSeconds(rotDuration);
        float alpha = 1;
        float startingRoat = rotDuration;

        while (alpha > 0)
        {
            ChangeRendererAlpha(alpha);
            startingRoat -= Time.deltaTime;
            alpha = startingRoat / rotDuration;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public void JumpToPlayer(Player player)
    {
        StopCoroutine(rotCoroutine);
        ChangeRendererAlpha(1);
        rotCoroutine = StartCoroutine(Rot());
        transform.SetParent(player.transform, true);
        transform.DOJump(player.rootPickupAnchor.position, jumpPower, numOfJumps, jumpDuration).OnComplete(() => player.SetState(Player.PlayerState.Carrying));
    }

    public void JumpToPot(Vector3 targetPos)
    {
        StopCoroutine(rotCoroutine);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => AddRootToPot());
    }

    private void AddRootToPot()
    {
        OnRootAddedToPot?.Invoke(this);
    }

    private void ChangeRendererAlpha(float value)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, value);
        }
    }

    public static Root SpawnRootWithChance(List<Root> roots)
    {
        int chanceSum = 0;
        for (int i = 0; i < roots.Count; i++)
        {
            Root root = roots[i];
            chanceSum += root.chance;
            if (i == 0)
            {
                root.minSpawnChance = 0;
                root.maxSpawnChance = root.chance;
            }
            else
            {
                root.minSpawnChance = roots[i - 1].maxSpawnChance;
                root.maxSpawnChance = root.minSpawnChance + root.chance;
            }
        }

        int rand = Random.Range(0, chanceSum);

        for (int i = 0; i < roots.Count; i++)
        {
            Root root = roots[i];
            if (rand >= root.minSpawnChance && rand < root.maxSpawnChance)
            {
                return root;
            }
        }
        return null;
    }

    public enum RootType
    {
        Carrot,
        Potato,
        Buryak,
        ObstacleRoot
    }
}
