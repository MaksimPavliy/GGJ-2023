using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Color dayStartColor;
    [SerializeField] private Color dayEndColor;
    [SerializeField] private Color dayMiddleColor;
    [SerializeField] private Vector3 sunMidPos;
    [SerializeField] private Vector3 sunEndPos;

    private Light sunlight;
    private Vector3 moveValueMid;
    private Vector3 moveValueEnd;
    private Vector3 curPosition;
    private Color colorValueMid;
    private Color colorValueEnd;
    private Color curColor;

    private bool midPassed;

    private void Start()
    {
        sunlight = GetComponent<Light>();
        GameManager.OnCollectedCounterUpdated += MoveSun;
        moveValueMid = (sunMidPos - transform.position) / (GameManager.instance.curLevel.requiredRootsAmount / 2);
        colorValueMid = (dayMiddleColor - dayStartColor) / (GameManager.instance.curLevel.requiredRootsAmount / 2);
        moveValueEnd = (sunEndPos - sunMidPos) / (GameManager.instance.curLevel.requiredRootsAmount / 2);
        colorValueEnd = (dayMiddleColor - dayEndColor) / (GameManager.instance.curLevel.requiredRootsAmount / 2);
        curPosition = transform.position;
        curColor = sunlight.color;
    }

    private void MoveSun()
    {
        Sequence sunSequence = DOTween.Sequence();

        if (curPosition != sunMidPos && !midPassed)
        {
            curPosition += moveValueMid;
            curColor += colorValueMid;
            sunSequence.Append(transform.DOMoveX(curPosition.x, 1.5f).SetEase(Ease.InQuad));
            sunSequence.Join(transform.DOMoveY(curPosition.y, 1.5f).SetEase(Ease.InSine));
            sunSequence.Join(sunlight.DOColor(curColor, 1.5f).SetEase(Ease.Linear));
            sunSequence.Play();
        }
        else if (curPosition != sunEndPos)
        {
            midPassed = true;
            curPosition = new Vector3(curPosition.x + moveValueEnd.x, curPosition.y - Math.Abs(moveValueEnd.y));
            curColor -= colorValueEnd;
            sunSequence.Append(transform.DOMoveX(curPosition.x, 1.5f).SetEase(Ease.InQuad));
            sunSequence.Join(transform.DOMoveY(curPosition.y, 1.5f).SetEase(Ease.InSine));
            sunSequence.Join(sunlight.DOColor(curColor, 1.5f).SetEase(Ease.Linear));
            sunSequence.Play();
        }
    }
}
