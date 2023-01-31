using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    [SerializeField] private List<Ridge> ridges;
    [SerializeField] private List<Root> roots;
    [SerializeField] private int startSpawnAmount;
    [SerializeField] private Transform rootsParent;
    
    private Ridge randomEmptyRidge;
    private Root root;
    private List<Ridge> emptyRidges = new List<Ridge>();
    private Coroutine spawnCoroutine;

    void Start()
    {
        for(int i = 0; i < startSpawnAmount; i++) { 

            FindRandomEmptyRidge();
            if (randomEmptyRidge)
            {
               root = SpawnRootWithChance(roots);
               randomEmptyRidge.root = Instantiate(root, randomEmptyRidge.transform.position, root.transform.rotation, rootsParent);
            }
        }
        spawnCoroutine = StartCoroutine(spawnRoots());
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
        emptyRidges[rand].isEmpty = false;
        randomEmptyRidge = emptyRidges[rand];
    }

    private IEnumerator spawnRoots()
    {
        while (GameManager.instance.isPlaying)
        {
            yield return new WaitForSeconds(5f);

            FindRandomEmptyRidge();
            if (randomEmptyRidge)
            {
                root = SpawnRootWithChance(roots);
                randomEmptyRidge.root = Instantiate(root, randomEmptyRidge.transform.position, root.transform.rotation, rootsParent);
            }
        }
    }

    private Root SpawnRootWithChance(List<Root> roots) 
    {
        int chanceSum = 0;
        for(int i = 0; i < roots.Count; i++)
        {
            Root root = roots[i];
            chanceSum += root.chance;
            if(i == 0)
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
            if(rand >= root.minSpawnChance && rand < root.maxSpawnChance)
            {
                return root;
            }
        }
        return null;
    }
}

