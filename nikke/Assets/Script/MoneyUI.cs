using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public bool chk;
    public TMPro.TMP_Text text;
    private string _value;

    public void FixedUpdate()
    {
        Invoke("TextUpdate", 0.5f);
    }
    public void TextUpdate()
    {
        if (chk) _value = Resource.Instance.money.ToString();
        else _value = "TURN : "+BattleManager.Instance.Turn;
        text.text =_value;
    }
}
