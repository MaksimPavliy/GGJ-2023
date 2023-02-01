using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ridge : MonoBehaviour
{
    [HideInInspector] public bool isEmpty = true;
    [HideInInspector] public BoxCollider2D bc;

    public Root root { get; set; }

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Root.OnRootRotten += IsEmpty;
    }

    public bool CanBeDigged()
    {
        if(root && root.HasGrown)
        {
            return true;
        }
        return false;
    }

    private void IsEmpty(Root root)
    {
        if (this.root == root)
        {
            isEmpty = !isEmpty;
        }
    }
}
