using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private Transform endPosTransform;

    private Vector3 moveValue;
    private Vector3 curPosition;

    void Start()
    {
        GameManager.OncollectedCounterUpdated += MoveSky;
        moveValue = (endPosTransform.position - transform.position) / GameManager.instance.requiredRootsAmount / 3;
        curPosition = transform.position;
    }

    private void MoveSky()
    {
        if (curPosition != endPosTransform.position)
        {
            curPosition += moveValue;
        }
        transform.DOMove(curPosition + moveValue, 1f).SetEase(Ease.Linear);
    }
}
