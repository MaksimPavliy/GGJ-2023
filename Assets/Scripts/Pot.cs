using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pot : MonoBehaviour
{
    [SerializeField] private List<PotRoot> potRoots;
    [SerializeField] private List<Transform> rootUIHolders;

    private PotRoot rootToRefresh;

    void Start()
    {
        Root.OnRootAddedToPot += RefreshUI;
        foreach(var potRoot in potRoots)
        {
            potRoot.amountText.text = potRoot.currentAmount + " / " + potRoot.requiredAmount;
        }
    }

    public bool rootInRecipe(Root.RootType rootType)
    {
        if (potRoots.Find(x => x.rootType == rootType) != null)
        {
            return true;
        }
        return false;
    }

    private void RefreshUI(Root root)
    { 
        rootToRefresh = potRoots.Find(x => x.rootType == root.rootType);
        Destroy(root.gameObject);
        rootToRefresh.currentAmount++;
        rootToRefresh.amountText.text = rootToRefresh.currentAmount + " / " + rootToRefresh.requiredAmount;
    }
}

