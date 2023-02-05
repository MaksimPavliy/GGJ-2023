using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private float rotDuration = 5;
    [SerializeField] private SpriteRenderer topRenderer;
    [SerializeField] private SpriteRenderer bottomRenderer;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;
    public static UnityAction<Root> OnRootRotten;

    public Vector3 spawnOffset;
    public Vector3 growScale;
    public float growDuration;
    public int chance;
    public RootType rootType;

    private int numOfJumps = 1;
    private bool hasGrown = false;
    private Coroutine rotCoroutine;

    private void Start()
    {
        Raven.OnScared += RotOnRavenScared;
        ChangeRendererAlpha(0, true);
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        ChangeRendererAlpha(1, true);

        Sequence scaleSequence = DOTween.Sequence();

        yield return new WaitForSeconds(growDuration - 1);

        scaleSequence.Append(transform.DOScale(growScale, 0.4f));
        scaleSequence.Append(transform.DOScale(growScale + new Vector3(0.1f, 0.1f, 0.1f), 0.2f));
        scaleSequence.Append(transform.DOScale(growScale - new Vector3(0.035f, 0.035f, 0.035f), 0.2f));
        scaleSequence.Append(transform.DOScale(growScale, 0.2f));
        scaleSequence.Play();
    
        hasGrown = true;
        if (rootType != RootType.Sornyak)
        {
            rotCoroutine = StartCoroutine(Rot(rotDuration));
        }
    }

    private IEnumerator Rot(float duration)
    {
        yield return new WaitForSeconds(duration);
        float alpha = 1;
        float startingRoat = duration;

        while (alpha > 0)
        {
            topRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, alpha);
            startingRoat -= Time.deltaTime;
            alpha = startingRoat / duration;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public IEnumerator JumpToPlayer(Player player, float digDuration)
    {
        StopRotting();
        yield return new WaitForSeconds(digDuration);
        SetRenderersOrder(4);
        if (rootType != RootType.Sornyak)
        {
            transform.SetParent(player.transform, true);
            ChangeRendererAlpha(0, false);
            bottomRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, 1);
            var pickupRotation = player.transform.rotation == Quaternion.Euler(0, 0, 0) ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
            transform.DOJump(player.rootPickupAnchor.position, jumpPower, numOfJumps, jumpDuration)
                .Join(transform.DORotate(pickupRotation, jumpDuration))
                .OnComplete(() => player.SetState(Player.PlayerState.Moving));
        }
        else
        {
            player.SetState(Player.PlayerState.Moving);
            Destroy(gameObject);
        }
    }

    public void JumpToRaven(Raven raven)
    {
        ChangeRendererAlpha(0, false);
        bottomRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, 1);
        SetRenderersOrder(5);
        transform.SetParent(raven.transform, true);

        float duration = 0.75f;
        transform.DOJump(raven.rootPickupAnchor.position, jumpPower, numOfJumps, duration)
            .OnComplete(() => raven.FlyAwayRandomly());
    }

    public IEnumerator JumpToPot(Vector3 targetPos, Character character)
    {
        StopRotting();
        ChangeRendererAlpha(0, true);
        transform.SetParent(character.transform, true);
        transform.position = character.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(1);
        ChangeRendererAlpha(1, true);
        ChangeRendererAlpha(0, false);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => AddRootToPot());
    }

    private void AddRootToPot()
    {
        OnRootAddedToPot?.Invoke(this);
    }

    private void ChangeRendererAlpha(float value, bool both)
    {
        if (both)
        {
            topRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, value);
            bottomRenderer.color = new Color(bottomRenderer.color.r, bottomRenderer.color.g, bottomRenderer.color.b, value);
        }
        else
        {
            topRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, value);
        }
    }

    public void StopRotting()
    {
        if (rotCoroutine != null && topRenderer)
        {
            StopCoroutine(rotCoroutine);
            topRenderer.color = new Color(topRenderer.color.r, topRenderer.color.g, topRenderer.color.b, 1);
        }
    }

    public void RotOnRavenScared(Raven raven)
    {
        if (raven.targetRidge.root == this)
        {
            StopRotting();
            rotCoroutine = StartCoroutine(Rot(rotDuration));
        }
    }

    private void SetRenderersOrder(int order)
    {

        topRenderer.sortingOrder = order;
        bottomRenderer.sortingOrder = order;
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

    private void OnDestroy()
    {
        DOTween.Kill(this);
        StopRotting();
    }

    public enum RootType
    {
        Carrot,
        Potato,
        Buryak,
        Sornyak
    }
}
