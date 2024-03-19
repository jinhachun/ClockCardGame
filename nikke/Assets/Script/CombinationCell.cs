using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationCell : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    public enum Combi { NoPair,OnePair,TwoPair,Triple, Fullhouse, Fourcard,FiveCard}
    public enum RateTarget { Species,Type}
    public Combi combi;
    public RateTarget rateTarget;
    private void OnEnable()
    {
        var rate = rateTarget == RateTarget.Species ?Resource.Instance.combiRate: Resource.Instance.combiRate;
        text.text = System.Math.Round(rate[(int)combi],2).ToString();
    }
}
