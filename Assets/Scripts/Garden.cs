using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    [SerializeField] private List<Ridge> ridges;
    [SerializeField] private List<Root> roots;
    [SerializeField] private Transform rootParent;
    [SerializeField] private int obstaclesSpawnAmount;
    [SerializeField] private List<Obstacle> obstacles;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private float spawnDelay = 4f;
    [SerializeField] private  List<Raven> ravens;
    [SerializeField] private float ravensSpawnDelay;
    [SerializeField] private Transform ravenParent;
    [SerializeField] private List<Player> players;

    private Ridge randomEmptyRidge;
    private List<Ridge> emptyRidges = new List<Ridge>();
    private int[] randomXValues = { 18, 19, -13, -15, 17, -14 };

    void Start()
    {
        SpawnObstaclesOnStart();
        StartCoroutine(SpawnRoots());
        StartCoroutine(SpawnRavens());
    }

    private void SpawnObstaclesOnStart()
    {
        foreach (var obstacle in obstacles)
        {
            for (int i = 0; i < obstacle.spawnAmount; i++)
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

                int[] randomRotationZ = { 180, 0 };             
                var randomZ = randomRotationZ[Random.Range(0, randomRotationZ.Length)];
                var curObstacle = Instantiate(obstacle, randomEmptyRidge.transform.position, Quaternion.Euler(0, 0, randomZ), obstacleParent);
                float[] randomScaleX = { curObstacle.transform.localScale.x, -curObstacle.transform.localScale.x };
                var randomX = randomScaleX[Random.Range(0, randomScaleX.Length)];
                curObstacle.transform.localScale = new Vector3(randomX, curObstacle.transform.localScale.y, curObstacle.transform.localScale.z);
            }
        }
    }

    private IEnumerator SpawnRavens()
    {
        yield return new WaitForSeconds(ravensSpawnDelay);

        var randomXValue = Random.Range(0, randomXValues.Length);
        var randomY = Random.Range(7, 10);
        var randomX = randomXValues[randomXValue];

        if (ravens.Count > 0)
        {
            foreach (var raven in ravens)
            {
                var curRaven = Instantiate(raven, new Vector2(randomX, randomY), Quaternion.identity, ravenParent);
                curRaven.ridges = ridges;
                curRaven.players = players;
            }
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
                randomEmptyRidge.root = Instantiate(root, randomEmptyRidge.transform.position + root.spawnOffset, root.transform.rotation, rootParent);
            }
        }
    }
}

