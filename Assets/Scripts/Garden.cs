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
                randomEmptyRidge.isEmpty = false;
                root = SpawnRootWithChance(roots);
                root.rootLocation = randomEmptyRidge;
                randomEmptyRidge.root = Instantiate(root, randomEmptyRidge.transform.position + root.spawnOffset, root.transform.rotation, rootsParent);
            }
        }
    }

    private Root SpawnRootWithChance(List<Root> roots)
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
}

