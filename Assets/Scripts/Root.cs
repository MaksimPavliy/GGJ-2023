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
    [SerializeField] private float jumpPower = 2;
    [SerializeField] private float jumpDuration = 1;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;

    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    private int numOfJumps = 1;
    private bool hasGrown = false;

    private void Start()
    {
        Grow();
    }

    private void Grow()
    {
        float timer = 0;
        transform.DOScale(1, growDuration);

        while(timer < growDuration)
        {
            timer += Time.deltaTime;
        }
        hasGrown = true;
    }

    public void JumpToPlayer(Player player)
    {
        Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y + carryOffset);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => player.SetState(Player.PlayerState.Carrying));
        transform.SetParent(player.transform);
        bottomPrefab.position = new Vector3(bottomPrefab.position.x, bottomPrefab.position.y, -1);
    }

    public void JumpToPot(Pot pot)
    {
        Vector2 targetPos = new Vector2(pot.transform.position.x, pot.transform.position.y);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => OnRootAddedToPot?.Invoke(this));
    }

    public enum RootType
    {
        Carrot,
        Potato
    }
}
