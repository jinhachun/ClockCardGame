using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AttDefCal : MonoBehaviour
{
    [SerializeField] TMP_Text Text_Att;
    [SerializeField] TMP_Text Text_Def;
    [SerializeField] TMP_Text Text_Rate;
    private int att => BattleManager.Instance.Att;
    private int def => BattleManager.Instance.Def;
    private double rate => BattleManager.Instance.Rate;

    public void textChange(TMP_Text txt, int a)
    {
        if (!txt.text.Equals(a.ToString())) txt.text = a.ToString();
    }
    public void textChangeRate(TMP_Text txt, double a)
    {
        if (!txt.text.Equals("X "+a.ToString())) txt.text = "X "+a.ToString();
    }
    public void Update()
    {
        textChange(Text_Att,att);
        textChange(Text_Def, def);
        textChangeRate(Text_Rate, rate);
    }
}
