using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class PotRoot : MonoBehaviour
{
    public Root.RootType rootType;
    public int requiredAmount;
    public TextMeshPro amountText;
    [HideInInspector] public int currentAmount = 0;
}
