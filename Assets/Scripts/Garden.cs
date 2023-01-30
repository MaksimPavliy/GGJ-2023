using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    [SerializeField] private List<Ridge> ridges;
    [SerializeField] private int spawnAmount;
    [SerializeField] List<Root> roots;

    int randomEmptyRidgeNumber;

    void Start()
    {
        int spawned = 0;

        while (spawned < 5)
        {
            int randomRidgeNumber = Random.Range(0, ridges.Count);
            if (ridges[randomRidgeNumber].IsEmpty)
            {
                /*Instantiate();*/
                spawned++;
            }
        }
    }

    private void FindRandomEmptyRidge()
    {
        
        randomEmptyRidgeNumber = Random.Range(0, ridges.Count);
    }

    void Update()
    {

    }
}
