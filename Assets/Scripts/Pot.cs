using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Pot : MonoBehaviour
{
    [SerializeField] private List<PotRoot> potRoots;
    [SerializeField] private List<Transform> rootUIHolders;
    [SerializeField] private Transform rootTargetTransform;

    public Transform RootTargetTransform => rootTargetTransform;

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
        if(potRoots.Find(x => x.rootType == rootType))
        {
            return true;
        }
        return false;
    }

    private void RefreshUI(Root root)
    {
        PotRoot rootToRefresh = potRoots.Find(x => x.rootType == root.rootType);
        Destroy(root.gameObject);
        if (rootToRefresh.currentAmount < rootToRefresh.requiredAmount)
        {
            rootToRefresh.currentAmount++;
        }
        rootToRefresh.amountText.text = rootToRefresh.currentAmount + " / " + rootToRefresh.requiredAmount;
        rootToRefresh.amountText.transform.DOScale(1.35f, 0.3f).SetLoops(2, LoopType.Yoyo);     
    }
}

