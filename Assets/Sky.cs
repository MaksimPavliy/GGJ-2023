using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private float endPosY;

    private float moveValue;
    private Vector3 curPosition;

    void Start()
    {
        GameManager.OncollectedCounterUpdated += MoveSky;
        moveValue = (transform.position.y - endPosY) / GameManager.instance.requiredRootsAmount;
        curPosition = transform.position;
    }   

    private void MoveSky()
    {
        if (curPosition.y != endPosY)
        {
            curPosition.y -= moveValue;
        }
        transform.DOMoveY(curPosition.y, 1f).SetEase(Ease.Linear);
    }
}
