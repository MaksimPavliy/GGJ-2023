using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private Transform endPosTransform;

    private float dayTimer;

    void Start()
    {
        dayTimer = GameManager.instance.levelTimer;
        Sun.OnMiddleReached += MoveSky;
    }

    private void MoveSky()
    {
        transform.DOMove(endPosTransform.position, dayTimer / 2).SetEase(Ease.Linear);
    }
}
