using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRule : Card
{
    public void Set(RuleStruct str)
    {
        this._name.text = str._name;
        this.name = str._name;

        this._tier.text = str._level.ToString();
        this.tier = str._level;

        this._text.text = str._text;
    }
}
