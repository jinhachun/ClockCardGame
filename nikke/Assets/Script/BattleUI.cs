using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public BattleUIValue chk;
    public TMPro.TMP_Text text;
    private string _value(BattleUIValue chk) {
        if (chk == BattleUIValue.Money) return Resource.Instance.money.ToString();
        else if (chk == BattleUIValue.Turn) return "TURN : " + BattleManager.Instance.Turn;
        else if (chk == BattleUIValue.Stage) return Resource.Instance.Area + " - " + Resource.Instance.Stage;
        return "";
    }
    
    public void FixedUpdate()
    {
        Invoke("TextUpdate", 0.5f);
    }
    public void TextUpdate()
    {
        text.text =_value(chk);
    }
}
public enum BattleUIValue
{
    Money,Turn,Stage
}
