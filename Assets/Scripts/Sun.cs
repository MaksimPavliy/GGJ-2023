using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sun : MonoBehaviour
{
    [SerializeField] private Color dayStartColor;
    [SerializeField] private  Color dayEndColor;
    [SerializeField] private Color dayMiddleColor;
    [SerializeField] private Transform sunEndTransform;

    private Light sunlight;
    private float dayTimer;

    private void Start()
    {
        sunlight = GetComponent<Light>();
        dayTimer = GameManager.instance.levelTimer;
        ImmitateDateTimeFlow();
    }

    private void ImmitateDateTimeFlow()
    {
        transform.DOJump(sunEndTransform.position, 3f, 1, dayTimer);
        sunlight.DOColor(dayMiddleColor, dayTimer / 2)
            .OnComplete(() => sunlight.DOColor(dayEndColor, dayTimer / 2));


    }
}
