using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voron : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float speed;
    [SerializeField] private List<Ridge> ridges;


    void Start()
    {
        
    }

    void Update()
    {
       transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
    }
}
