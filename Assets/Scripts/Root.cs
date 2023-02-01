using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private float growDuration;
    [SerializeField] private Transform bottomPrefab;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private float rotDuration = 5;
    [SerializeField] private List<Renderer> renderers;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;
    public static UnityAction<Root> OnRootRotten;

    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    private int numOfJumps = 1;
    private bool hasGrown = false;

    private void Start()
    {
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        transform.DOScale(1, growDuration);
        yield return new WaitForSeconds(growDuration);
        hasGrown = true;

        StartCoroutine(Rot());
    }

    private IEnumerator Rot()
    {
        yield return new WaitForSeconds(rotDuration / 2);

        float timeBeforeRot = rotDuration / 2;
        float flickeringInterval = timeBeforeRot / 5;

        while (timeBeforeRot > 0)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                if (renderers[i].enabled)
                {
                    renderers[i].enabled = false;
                    yield return new WaitForSeconds(0.1f);
                }                
                else
                {
                    renderers[i].enabled = true;
                    yield return new WaitForSeconds(flickeringInterval);
                }
            }
            timeBeforeRot -= flickeringInterval;
            flickeringInterval -= flickeringInterval / 7.5f;
            
        }
        OnRootRotten?.Invoke(this);
        Destroy(gameObject);
    }

    public void JumpToPlayer(Player player)
    {
        Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y + carryOffset);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => player.SetState(Player.PlayerState.Carrying));
        transform.SetParent(player.transform);
        bottomPrefab.position = new Vector3(bottomPrefab.position.x, bottomPrefab.position.y, -1);
    }

    public void JumpToPot(Vector3 targetPos)
    {
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => OnRootAddedToPot?.Invoke(this));
    }

    public enum RootType
    {
        Carrot,
        Potato,
        ObstacleRoot
    }
}
