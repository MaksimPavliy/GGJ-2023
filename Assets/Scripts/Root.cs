using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public RootTypes RootType => rootType;

    private RootTypes rootType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum RootTypes
    {
        Carrot,
        Potato
    }
}
