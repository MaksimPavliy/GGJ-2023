using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Sun : MonoBehaviour
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

    private void Start()
    {
        sunlight = GetComponent<Light>();
        GameManager.OncollectedCounterUpdated += MoveSun;
        moveValueMid = (sunMidPos - transform.position) / (GameManager.instance.requiredRootsAmount / 2);
        colorValueMid = (dayMiddleColor - dayStartColor) / (GameManager.instance.requiredRootsAmount / 2);
        moveValueEnd = sunEndPos - transform.position / (GameManager.instance.requiredRootsAmount / 2);
        colorValueEnd = (dayMiddleColor - dayEndColor) / (GameManager.instance.requiredRootsAmount / 2);
        curPosition = transform.position;
        curColor = sunlight.color;
    }

    private void MoveSun()
    {
        Sequence sunSequence = DOTween.Sequence();

        if (curPosition != sunMidPos)
        {
            curPosition += moveValueMid;
            curColor += colorValueMid;
            sunSequence.Append(transform.DOMoveX(curPosition.x, 2f).SetEase(Ease.InSine));
            sunSequence.Join(transform.DOMoveY(curPosition.y, 2f).SetEase(Ease.InSine));
            sunSequence.Join(sunlight.DOColor(curColor, 2).SetEase(Ease.Linear));
            sunSequence.Play();
        }
        else if (curPosition != sunEndPos)
        {
            curPosition = new Vector3(curPosition.x + moveValueMid.x, curPosition.y - moveValueMid.y);
            curColor -= colorValueMid;
            sunSequence.Append(transform.DOMoveX(curPosition.x, 2f).SetEase(Ease.InSine));
            sunSequence.Join(transform.DOMoveY(curPosition.y, 2f).SetEase(Ease.InSine));
            sunSequence.Join(sunlight.DOColor(curColor, 2).SetEase(Ease.Linear));
            sunSequence.Play();
        }
    }
}
