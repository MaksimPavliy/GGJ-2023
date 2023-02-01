using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    [SerializeField] private List<Ridge> ridges;
    [SerializeField] private List<Root> roots;
    [SerializeField] private Transform rootsParent;
    [SerializeField] private int obstaclesSpawnAmount;
    [SerializeField] private Root rootObstacle;
    [SerializeField] private float spawnDelay = 2f;

    private Ridge randomEmptyRidge;
    private List<Ridge> emptyRidges = new List<Ridge>();

    void Start()
    {
        SpawnRootObstaclesOnStart(obstaclesSpawnAmount);
        StartCoroutine(SpawnRoots());
    }

    private void SpawnRootObstaclesOnStart(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            FindRandomEmptyRidge();
            if (!randomEmptyRidge.canHaveObstacle)
            {
                i--;
                continue;
            }
            randomEmptyRidge.isEmpty = false;
            
            randomEmptyRidge.root = null;
            randomEmptyRidge.bc.isTrigger = false;
            Instantiate(rootObstacle, randomEmptyRidge.transform.position + rootObstacle.spawnOffset, rootObstacle.transform.rotation, rootsParent);
        }
    }

    private void FindRandomEmptyRidge()
    {
        emptyRidges = ridges.FindAll(ridge => ridge.isEmpty);
        if (emptyRidges.Count == 0)
        {
            randomEmptyRidge = null;
            return;
        }
        int rand = Random.Range(0, emptyRidges.Count);
        randomEmptyRidge = emptyRidges[rand];
    }

    private IEnumerator SpawnRoots()
    {
        Root root;

        while (GameManager.instance.isPlaying)
        {
            yield return new WaitForSeconds(spawnDelay);

            FindRandomEmptyRidge();
            if (randomEmptyRidge)
            {
                root = Root.SpawnRootWithChance(roots);
                randomEmptyRidge.isEmpty = false;
                randomEmptyRidge.root = Instantiate(root, randomEmptyRidge.transform.position + root.spawnOffset, root.transform.rotation, rootsParent);
            }
        }
    }
}

