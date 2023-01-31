using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private float growDuration;

    public int chance;

    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;

    public RootType rootType;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public enum RootType
    {
        Carrot,
        Potato
    }
}
