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
        bool target = rateTarget == RateTarget.Species;
        float rate = target ? CardDatabase.Instance.SpeciesCombiRate((int)combi) : CardDatabase.Instance.TypeCombiRate((int)combi);
        text.text = System.Math.Round(rate,2).ToString();
    }
}
