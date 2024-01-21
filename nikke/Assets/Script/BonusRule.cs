using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRule : Card
{
    [SerializeField] TMPro.TMP_Text Rate;
    public void Set(RuleStruct str)
    {
        string tmpName = DataManager.RuleName(str._Number, Resource.Instance.Kor);
        string tmpInfo = DataManager.RuleInfo(str._Number, Resource.Instance.Kor);
        this._name.text = tmpName;
        this.name = tmpName;


        this._img.sprite = str._img;

        this._tier.text = "+"+str._level.ToString();
        this.tier = str._level;

        this._text.text = tmpInfo;

        this.Rate.text = "X"+Resource.Instance.Rules[this.name].ToString();
    }
}
