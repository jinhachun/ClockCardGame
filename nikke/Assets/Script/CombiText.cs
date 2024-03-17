using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiText : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_Text _text_combi;
    [SerializeField] public TMPro.TMP_Text _text_rate;

    public void Set(string Combi, float Rate)
    {
        _text_combi.text = Combi;
        _text_rate.text = "X"+System.Math.Round(Rate,2);
    }
}
