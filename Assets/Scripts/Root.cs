using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private float growDuration;
    [SerializeField] Transform bottomPrefab;

    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    [SerializeField] private float jumpPower = 2;
    [SerializeField] private float jumpDuration = 1;
    private int numOfJumps = 1;

    private void Start()
    {
        Player.OnRootDigged += JumpToPlayer;
    }

    private void JumpToPlayer(Player player, Root targetRoot)
    {
        if (this == targetRoot)
        {
            Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y + carryOffset);
            transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => player.SetState(Player.PlayerState.Carrying));
            transform.SetParent(player.transform);
            bottomPrefab.position = new Vector3(bottomPrefab.position.x, bottomPrefab.position.y, -1);
        }
    }

    public enum RootType
    {
        Carrot,
        Potato
    }
}
